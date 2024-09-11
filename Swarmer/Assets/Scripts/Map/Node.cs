using UnityEngine;
using System;

[Serializable]
public class Node
{
	public Vector2Int coord;
	public Vector2 pos;
	public int index;
	public Occupation[] occupations;
	//? 0 = ground | 1 = foundation | 2 = tower
	public bool towerable = false, isBorder = false;
	public Vector2Int chunkLocate;

	public Node(Vector2Int coord, Vector2 pos, int index, Vector2Int chunk)
	{
		this.coord = coord;
		this.pos = pos;
		this.index = index;
		this.chunkLocate = chunk;
		//Cretae blank occupation
		occupations = new Occupation[3];
		for (int o = 0; o < 3; o++) {occupations[o] = new Occupation();}
		//Make new flow
		flows = new Flows();
		ResetFlow();
	}
	
	[Serializable]
	public class Occupation
	{
		public GameObject structure;
		public ushort flowCostMod;
	}

	public void Occupating(GameObject structureObj)
	{
		Structure structure = structureObj.GetComponent<Structure>();
		occupations[structure.layer].structure = structureObj;
		occupations[structure.layer].flowCostMod = structure.flowCostMod;
	}

	public void UnOccupating(int layer)
	{
		occupations[layer].structure = null;
		occupations[layer].flowCostMod = 0;
	}

	[Serializable]
	public class Flows
	{
		public ushort cost;
		public ushort prior;
		public Vector2 direction;
	}

	public Flows flows;

	public void ResetFlow()
	{
		flows.prior = ushort.MaxValue;
		flows.cost = (ushort)(occupations[0].flowCostMod + occupations[1].flowCostMod + occupations[2].flowCostMod);
	}
}