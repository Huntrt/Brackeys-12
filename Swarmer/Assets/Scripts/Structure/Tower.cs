using UnityEngine;

public class Tower : MonoBehaviour
{
    public delegate void OnTowerStatsChange(string statsName, float modifier);
	public OnTowerStatsChange onTowerAndStrikeStatsChange;
	[SerializeField] protected TowerInfoController infoControl;
	[SerializeField] public RangeDetector rangeDetector;

	public virtual void ShowInfo(string statsName, float modifier)
	{

	}

	protected void DamageEnemy(GameObject enemyObj, float damage)
	{
		enemyObj.GetComponent<Health>().Damaging(damage);
	}
}