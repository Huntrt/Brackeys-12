using System.Collections.Generic;
using UnityEngine;
using System;

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

	[SerializeField] GameObject nodeObj;
	[SerializeField] GameObject nodeGrouper;
	[SerializeField] Vector2Int mapSize; public Vector2Int MapSize {get => mapSize;}
	[SerializeField] float spacing; public float Spacing {get => spacing;}
    public Dictionary<Vector2Int, Node> nodeIndexs = new Dictionary<Vector2Int, Node>();
	public List<Node> nodes = new List<Node>();

	void Start()
	{
		for (int x = -mapSize.x; x <= mapSize.x; x++) for (int y = -mapSize.y; y <= mapSize.y; y++)
		{
			CreateNode(new Vector2Int(x,y), nodeObj);
		}
	}
	
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

	public Node FindNode(Vector2Int coord, out Node finded)
	{
		if(nodeIndexs.ContainsKey(coord))
		{
			finded = nodeIndexs[coord];
			return nodeIndexs[coord];
		}
		else
		{
			finded = null;
			return null;
		}
	}

	public Node FindNode(Vector2Int coord)
	{
		if(nodeIndexs.ContainsKey(coord)) return nodeIndexs[coord]; else return null;
	}

	public void CreateNode(Vector2Int createCoord, GameObject ground = null)
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
		Node createdNode = new Node(createCoord, worldPos, nodes.Count, ground);
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
}

[Serializable]
public class Node
{
	public Vector2Int coord;
	public Vector2 pos;
	public int index;
	public GameObject[] occupations = new GameObject[3];
	//? 0 = ground | 1 = foundation | 2 = tower
	public bool towerable = false;

	public Node(Vector2Int coord, Vector2 pos, int index, GameObject ground)
	{
		this.coord = coord;
		this.pos = pos;
		this.index = index;
		this.occupations[0] = ground;
		flows = new Flows();
	}


	[Serializable]
	public class Flows
	{
		public ushort cost;
		public ushort prior;
		public Vector2 direction;

		public void Renew()
		{
			prior = ushort.MaxValue;
			cost = 10;
		}
	}

	public Flows flows;
}