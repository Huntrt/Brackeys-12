using System.Linq;
using UnityEngine;

public class Structure : MonoBehaviour
{
	[Range(0,2)]
    public int layer;
	public ushort flowCostMod;
	public bool towerable;
	public Category[] categories;
	public Node nodeReside;

	public enum Category {wall, tower}

	public bool HaveCatalog(Category neededCategory)
	{
		return categories.Contains(neededCategory);
	}
}
