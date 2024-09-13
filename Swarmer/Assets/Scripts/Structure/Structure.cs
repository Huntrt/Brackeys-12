using UnityEngine;

public class Structure : MonoBehaviour
{
	[SerializeField] string displayName; public string DisplayName {get => displayName;}
	[SerializeField] string description; public string Description {get => description;}
	[Range(0,2)]
    public int layer;
	public ushort flowCostMod;
	public bool towerable;
	public Category[] categories;
	public Node nodeReside;

	public enum Category {wall, tower}

	public bool HaveCatalog(Category neededCategory)
	{
		if(categories == null || categories.Length < 0)
		{
			return false;
		}
		for (int c = 0; c < categories.Length; c++)
		{
			if(categories[c] == neededCategory) {return true;}
		}
		return false;
	}
}
