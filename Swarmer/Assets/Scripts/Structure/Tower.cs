using UnityEngine;

public class Tower : MonoBehaviour
{
    public delegate void OnTowerStatsChange(string statsName, float modifier);
	public OnTowerStatsChange onTowerStatsChange;
	[SerializeField] protected TowerInfoController infoControl;

	public virtual void ShowInfo(string statsName, float modifier)
	{

	}
}
