using System.Collections.Generic;
using Unity.Mathematics;     
using UnityEngine;
using UnityEditor;

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
	[SerializeField] GameObject nodePrf;
	[SerializeField] GameObject nodeGrouper;
	[SerializeField] int mapSize; public int MapSize {get => mapSize;}
	[SerializeField] float spacing; public float Spacing {get => spacing;}
    public Dictionary<Vector2Int, Node> nodeIndexs = new Dictionary<Vector2Int, Node>();
	public List<Node> nodes = new List<Node>();
	
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
			CreateNode(new Vector2Int(x + shiftedChunk.x,y + shiftedChunk.y), chunk, nodePrf);
		}
		RenewBorder();
	}

	void RenewBorder()
	{
		foreach (Node node in nodes)
		{
			//Destroy all node currently have border
			if(node.isBorder)
			{
				node.isBorder = false;
				DestroyOnNode(node, 1);
			}
			//This node is border if it dont have neighbor
			List<Node> neighbor = GetNeighbor(node, true, true, false);
			if(neighbor.Count < 8)
			{
				node.isBorder = true;
				GameObject borderCreated = BuildOnNode(node, borderPrf, 1);
				borderCreated.transform.SetParent(borderGrouper.transform);
			}
		}
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


	public GameObject BuildOnNode(Node node, GameObject structure, int layer)
	{
		GameObject builded = Instantiate(structure, node.pos, quaternion.identity);
		node.occupations[layer] = builded;
		return builded;
	}

	public void DestroyOnNode(Node node, GameObject structure) //? Destroy by search the building occupaid it
	{
		foreach (GameObject o in node.occupations) {if(o == structure) Destroy(o);}
	}
	public void DestroyOnNode(Node node, int layer) //? Destroy by get the structure at given layer
	{
		Destroy(node.occupations[layer]);
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
		//Create the ground if needed
		if(ground != null)
		{
			ground = Instantiate(ground, worldPos, Quaternion.identity);
			//Setup the ground object just create
			ground.name = nodes.Count + " | Node " + createCoord;
			ground.transform.SetParent(nodeGrouper.transform);
		}
		//Make an new node and index it
		Node createdNode = new Node(createCoord, worldPos, nodes.Count, ground, chunk);
		nodes.Add(createdNode);
		nodeIndexs.Add(createCoord, createdNode);
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

	void OnDrawGizmos()
	{
		if(!debug) return;
		foreach (Node n in nodes)
		{
			Handles.Label(n.pos, "u" + n.chunkLocate + "\n c" + n.coord + "\n");
		}
	}
}