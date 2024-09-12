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
	public bool isBorder = false;
	public Vector2Int chunkReside;

	public Node(Vector2Int coord, Vector2 pos, int index, Vector2Int chunk)
	{
		this.coord = coord;
		this.pos = pos;
		this.index = index;
		this.chunkReside = chunk;
		//Create blank occupation
		occupations = new Occupation[3];
		for (int o = 0; o < 3; o++) {occupations[o] = new Occupation(); UnOccupating(o);}
		//Make new flow
		flows = new Flows();
		ResetFlow();
	}

	
	[Serializable]
	public class Occupation
	{
		public GameObject obj;
		public Structure component;
		public ushort flowCostMod;
	}

	public bool HaveOccupation(int layer)
	{
		if(occupations[layer].obj == null) return false;
		if(occupations[layer].component == null) return false;
		return true;
	}

	public bool AllowOccupation(GameObject checkObj, out string status)
	{
		Structure structure = checkObj.GetComponent<Structure>();
		if(structure == null) Debug.LogError("The [" + checkObj.name + "] structure is missing structure component");
		/// STOP - If there an structure already occupied on that layer
		if(occupations[structure.layer].obj != null) 
		{
			status = "Structure [" + occupations[structure.layer].obj.name + "] already exist at layer " + structure.layer; 
			return false;
		}
		/// STOP - If try to build tower but foundation not occupied
		if(structure.layer == 2 && occupations[1].obj == null) 
		{
			status = "There no FOUNDATION for tower [" + checkObj.name +"]"; 
			return false;
		}
		/// STOP - If try to build tower but the foundation not allow
		if(structure.layer == 2 && !occupations[1].component.towerable) 
		{
			status = "The foundation [" + occupations[1].obj.name + "] does not support tower"; 
			return false;
		}
		status = "Structure [" + checkObj.name + "] allow to occupied";
		return true;
	}

	public bool Occupating(GameObject occupator)
	{
		Structure structure = occupator.GetComponent<Structure>();
		structure.nodeReside = this;
		occupations[structure.layer].obj = occupator;
		occupations[structure.layer].component = structure;
		occupations[structure.layer].flowCostMod = structure.flowCostMod;
		return true;
	}

	public void UnOccupating(int layer)
	{
		occupations[layer].obj = null;
		occupations[layer].component = null;
		occupations[layer].flowCostMod = 0;
	}

	[Serializable]
	public class Flows
	{
		public ushort cost;
		public ushort prior;
		public Vector2 direction;
		public Vector2Int nextNode;
	}

	public Flows flows;

	public void ResetFlow()
	{
		flows.prior = ushort.MaxValue;
		flows.cost = (ushort)(occupations[0].flowCostMod + occupations[1].flowCostMod + occupations[2].flowCostMod);
	}
}