using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
using System;

public class NodeMap : MonoBehaviour
{
	//test: test variable 
	public GameObject testOccupation;
	public Vector2Int mapSize;
	public float spacing;
    public Dictionary<Vector2Int, Node> coordIndexes = new Dictionary<Vector2Int, Node>();
	public List<Node> nodes = new List<Node>();

	void Start()
	{
		for (int x = -mapSize.x; x <= mapSize.x; x++) for (int y = -mapSize.y; y <= mapSize.y; y++)
		{
			CreateNode(new Vector2Int(x,y), testOccupation);
		}
	}

	public Node FindNode(Vector2Int coord)
	{
		if(coordIndexes.ContainsKey(coord)) return coordIndexes[coord]; else return null;
	}

	public void CreateNode(Vector2Int createCoord, GameObject occupating = null)
	{
		//Prevent new node duplication
		if(FindNode(createCoord) != null)
		{
			Debug.LogWarning("Node at [" + createCoord + "] already exist");
			return;
		}
		Vector2 worldPos = new Vector3(createCoord.x, createCoord.y, 0) * spacing;
		//Create the occupation if needed
		if(occupating != null)
		{
			occupating = Instantiate(occupating, worldPos, Quaternion.identity);
		}
		//Make an new node and index it
		Node createdNode = new Node(createCoord, occupating, worldPos);
		nodes.Add(createdNode);
		coordIndexes.Add(createCoord, createdNode);
	}
}

[Serializable]
public class Node
{
	public Vector2Int coord;
	public GameObject occupation;
	public Vector2 pos;

	public Node(Vector2Int coord, GameObject occupation, Vector2 pos)
	{
		this.coord = coord;
		this.occupation = occupation;
		this.pos = pos;
	}
}