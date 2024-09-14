using UnityEngine;

public class Tower : MonoBehaviour
{
    public delegate void OnTowerStatsChange(string statsName, float modifier);
	public OnTowerStatsChange onTowerAndStrikeStatsChange;
	[SerializeField] protected TowerInfoController infoControl;
	public RangeDetector rangeDetector;
	public Upgrader upgrader;

	void OnValidate()
	{
		infoControl = GetComponent<TowerInfoController>();
		rangeDetector = GetComponent<RangeDetector>();
		upgrader = GetComponent<Upgrader>();
	}

	public virtual void ShowInfo(string statsName, float modifier)
	{

	}

	public void Upgrading()
	{
		upgrader.ApplyUpgrade();
	}

	protected void DamageEnemy(GameObject enemyObj, float damage)
	{
		enemyObj.GetComponent<Health>().Damaging(damage);
	}
}
