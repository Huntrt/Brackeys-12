using System.Collections.Generic;
using Unity.Mathematics;     
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;

public class Map : MonoBehaviour
{
	#region Set this class to singleton
	static Map _i; public static Map i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindObjectOfType<Map>();
			}
			return _i;
		}
	}
	#endregion

	public bool debug;
	[SerializeField] GameObject borderPrf;
	[SerializeField] GameObject borderGrouper;
	[SerializeField] GameObject groundPrf;
	[SerializeField] GameObject groundGrouper;
	[SerializeField] int mapSize; public int MapSize {get => mapSize;}
	[SerializeField] float spacing; public float Spacing {get => spacing;}
	[SerializeField] GameObject mapCreateEffect;
    public Dictionary<Vector2Int, Node> nodeIndexs = new Dictionary<Vector2Int, Node>();
	public List<Node> nodes = new List<Node>();
	public delegate void OnMapCreated(Vector2Int chunk); public OnMapCreated onMapCreated;
	public Vector2 maxMapSize, minMapSize;
	
	//Function to make any value take into account of spacing
	public static float Spaced(float value) {return (value) * i.spacing;}

	public static Vector2 SnapPosition(Vector2 position)
	{
		return (Vector2)WorldToCoordinates(position) * Map.i.spacing;
	}

	public static Vector2Int WorldToCoordinates(Vector2 worldPos)
	{
		//Make world position take in account of spacing
		Vector2 spacedWorldPos = worldPos / Map.i.Spacing;
		//Rounding the positon to be coordinates
		Vector2Int coord = new Vector2Int(Mathf.RoundToInt(spacedWorldPos.x), Mathf.RoundToInt(spacedWorldPos.y));
		return coord;
	}

	public void CreateMap(Vector2Int chunk)
	{
		if(mapSize%2 == 0) {Debug.LogError("Map size should not be even");}
		//Shift the chunk to map size
		Vector2Int shiftedChunk = chunk * (mapSize*2+1);
		//Create node for given chunk
		for (int x = -mapSize; x <= mapSize; x++) for (int y = -mapSize; y <= mapSize; y++)
		{
			CreateNode(new Vector2Int(x + shiftedChunk.x,y + shiftedChunk.y), chunk, groundPrf);
		}
		Instantiate(mapCreateEffect, (Vector2)(chunk*(mapSize+mapSize/2)), mapCreateEffect.transform.rotation);
		RenewBorder();
		onMapCreated?.Invoke(chunk);
	}

	void RenewBorder()
	{
		foreach (Node node in nodes)
		{
			//Destroy all node border to be renew
			if(node.renewBorder)
			{
				node.renewBorder = false;
				BuilderManager.DemolishAtNode(node, 1);
			}
			//This node is border if it dont have neighbor
			List<Node> neighbor = GetNeighbor(node, true, true, false);
			if(neighbor.Count < 8)
			{
				//Build an renewable border
				BuildBorder(node, true);
			}
		}
	}

	public void BuildBorder(Node node, bool isRenewable)
	{
		node.renewBorder = isRenewable;
		GameObject borderCreated = BuilderManager.BuildAtNode(node, borderPrf);
		borderCreated.transform.SetParent(borderGrouper.transform);
	}

	public Node FindNode(Vector2Int coord, out Node finded)
	{
		if(nodeIndexs.ContainsKey(coord)) {finded = nodeIndexs[coord]; return nodeIndexs[coord];} 
		else {finded = null; return null;}
	}
	public Node FindNode(Vector2Int coord)
	{
		if(nodeIndexs.ContainsKey(coord)) return nodeIndexs[coord]; else return null;
	}

	public void CreateNode(Vector2Int createCoord, Vector2Int chunk, GameObject ground = null)
	{
		//Prevent new node duplication
		if(FindNode(createCoord) != null)
		{
			Debug.LogWarning("Node at [" + createCoord + "] already exist");
			return;
		}
		Vector2 worldPos = new Vector3(createCoord.x, createCoord.y, 0) * spacing;
		//Make an new node and index it
		Node createdNode = new Node(createCoord, worldPos, nodes.Count, chunk);
		///Build the ground at layer zero on created node
		if(ground != null) 
		{
			GameObject groundBuilded = BuilderManager.BuildAtNode(createdNode, ground);
			//Setup the ground object just create
			groundBuilded.name = nodes.Count + " | Node " + createCoord;
			groundBuilded.transform.SetParent(groundGrouper.transform);
		}
		//Save create node and it index
		nodes.Add(createdNode);
		nodeIndexs.Add(createCoord, createdNode);
		//Update map size if created node position reached min/max map size
		Vector2 nodePos = createdNode.pos;
		if(nodePos.x < Map.i.minMapSize.x) Map.i.minMapSize.x = nodePos.x;
		if(nodePos.y < Map.i.minMapSize.y) Map.i.minMapSize.y = nodePos.y;
		if(nodePos.x > Map.i.maxMapSize.x) Map.i.maxMapSize.x = nodePos.x;
		if(nodePos.y > Map.i.maxMapSize.y) Map.i.maxMapSize.y = nodePos.y;
	}

	public List<Node> GetNeighbor(Node owner, bool cardinal, bool diagonal, bool self)
	{
		List<Node> neighbors = new List<Node>();
		Node finded = null;
		if(cardinal)
		{
			if(FindNode(owner.coord + Vector2Int.up, out finded)!=null) neighbors.Add(finded);
			if(FindNode(owner.coord + Vector2Int.down, out finded)!=null) neighbors.Add(finded);
			if(FindNode(owner.coord + Vector2Int.left, out finded)!=null) neighbors.Add(finded);
			if(FindNode(owner.coord + Vector2Int.right, out finded)!=null) neighbors.Add(finded);
		}
		if(diagonal)
		{
			if(FindNode(owner.coord + (Vector2Int.up + Vector2Int.left), out finded)!=null) neighbors.Add(finded);
			if(FindNode(owner.coord + (Vector2Int.up + Vector2Int.right), out finded)!=null) neighbors.Add(finded);
			if(FindNode(owner.coord + (Vector2Int.down + Vector2Int.left), out finded)!=null) neighbors.Add(finded);
			if(FindNode(owner.coord + (Vector2Int.down + Vector2Int.right), out finded)!=null) neighbors.Add(finded);
		}
		if(self)
		{
			neighbors.Add(owner);
		}
		return neighbors;
	}

	public List<Node> GetVacants(Vector2Int ignoreChunk)
	{
		List<Node> vacants = new List<Node>();
		foreach (Node node in nodes)
		{
			if(node.occupations[1].obj == null && node.chunkReside != ignoreChunk)
			{
				vacants.Add(node);
			}
		}
		return vacants;
	}
	
	public List<Node> GetVacantsInChunk(Vector2Int chunk)
	{
		List<Node> vacants = new List<Node>();
		foreach (Node node in nodes)
		{
			if(node.occupations[1].obj == null && node.chunkReside == chunk)
			{
				vacants.Add(node);
			}
		}
		return vacants;
	}

	// void OnDrawGizmos()
	// {
	// 	if(!debug) return;
	// 	foreach (Node n in nodes)
	// 	{
	// 		Handles.Label(n.pos, "u" + n.chunkReside + "\n c" + n.coord + "\n");
	// 	}
	// }
}