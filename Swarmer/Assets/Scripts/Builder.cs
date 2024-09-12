using UnityEngine;

public class Builder : MonoBehaviour
{
	#region Set this class to singleton
	static Builder _i; public static Builder i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindObjectOfType<Builder>();
			}
			return _i;
		}
	}
	#endregion

	public static GameObject BuildAtNode(Node node, GameObject structure, out string status)
	{
		//Stop build if not allow to occupation the node
		if(!node.AllowOccupation(structure, out status)) return null;
		//Create structure and occupating the node
		GameObject builded = Instantiate(structure, node.pos, Quaternion.identity);
		node.Occupating(builded);
		return builded;
	}
	public static GameObject BuildAtNode(Node node, GameObject structure)
	{
		string status;
		//Stop build if not allow to occupation the node
		if(!node.AllowOccupation(structure, out status)) return null;
		//Create structure and occupating the node
		GameObject builded = Instantiate(structure, node.pos, structure.transform.rotation);
		node.Occupating(builded);
		return builded;
	}

	public static void DemolishAtNode(Node node, GameObject structure) //? Destroy by search the building occupaid it
	{
		//Find the occcupation of given node
		for (int o = 0; o < node.occupations.Length; o++) if(node.occupations[o].obj == structure) 
		{
			//Destroy the structure above foundation
			if(o == 1 && node.occupations[2].obj != null) DemolishAtNode(node, 2);
			Destroy(node.occupations[o].obj); 
			node.UnOccupating(o);
		}
	}
	public static void DemolishAtNode(Node node, int layer) //? Destroy by get the structure at given layer
	{
		//Destroy the structure above foundation layer
		if(layer == 1 && node.occupations[2].obj != null) DemolishAtNode(node, 2);
		Destroy(node.occupations[layer].obj);
		node.UnOccupating(layer);
	}
}
