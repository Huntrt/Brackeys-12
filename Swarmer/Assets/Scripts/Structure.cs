using UnityEngine;

public class Structure : MonoBehaviour
{
	[Range(0,2)]
    public int layer;
	public ushort flowCostMod;
	public bool towerable;
}
