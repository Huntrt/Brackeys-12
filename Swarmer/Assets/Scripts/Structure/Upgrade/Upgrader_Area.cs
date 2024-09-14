using UnityEngine;

public class Upgrader_Area : Upgrader
{
	[SerializeField] Tower_Area tower;
	[SerializeField] Tower_Area.Stats towerUpgrade;


	void OnValidate()
	{
		tower = GetComponent<Tower_Area>();
	}

	public override void ApplyUpgrade()
	{
		base.ApplyUpgrade();
		tower.towerStats.Damage += Modify(tower.towerStats.Damage, towerUpgrade.Damage);
		tower.towerStats.FireRate += Modify(tower.towerStats.FireRate, towerUpgrade.FireRate);
		tower.towerStats.Range += Modify(tower.towerStats.Range, towerUpgrade.Range);

		tower.towerStats.Radius += Modify(tower.towerStats.Radius, towerUpgrade.Radius);
		tower.towerStats.Repeat += Modify(tower.towerStats.Repeat, towerUpgrade.Repeat);
		tower.towerStats.Amount += Modify(tower.towerStats.Amount, towerUpgrade.Amount);
	}
}
