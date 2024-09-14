using UnityEngine;

public class Upgrader : MonoBehaviour
{
	[SerializeField] int level; public int Level {get => level;}
	[SerializeField] int initialCost;
	[SerializeField] int curCost; public int CurCost {get => level;}
	[SerializeField] float costScale;

	void OnEnable()
	{
		curCost = initialCost;
	}

	public virtual void ApplyUpgrade()
	{
		curCost += (int)Misc.Percent((float)curCost, costScale);
	}

	public float Modify(float stat, float percent)
	{
		return Misc.Percent(stat, percent);
	}
}