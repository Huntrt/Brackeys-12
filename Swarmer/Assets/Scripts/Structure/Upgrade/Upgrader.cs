using UnityEngine;

public class Upgrader : MonoBehaviour
{
	[SerializeField] int level; public int Level {get => level;}
	[SerializeField] int initialCost;
	[SerializeField] float costScale;
	[SerializeField] float curCost; public int CurCost {get => Mathf.RoundToInt(curCost);}

	void OnEnable()
	{
		curCost = initialCost;
	}

	public virtual void ApplyUpgrade()
	{
		curCost += Misc.Percent(curCost, costScale);
		level++;
	}

	public float Modify(float stat, float percent)
	{
		return Misc.Percent(stat, percent);
	}
}