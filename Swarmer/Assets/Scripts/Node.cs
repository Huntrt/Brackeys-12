using UnityEngine;
using System;

[Serializable]
public class Node
{
	public Vector2Int coord;
	public Vector2 pos;
	public int index;
	public GameObject[] occupations = new GameObject[3];
	//? 0 = ground | 1 = foundation | 2 = tower
	public bool towerable = false;
	public Vector2Int chunkLocate;

	public Node(Vector2Int coord, Vector2 pos, int index, GameObject ground, Vector2Int chunk)
	{
		this.coord = coord;
		this.pos = pos;
		this.index = index;
		this.occupations[0] = ground;
		this.chunkLocate = chunk;
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