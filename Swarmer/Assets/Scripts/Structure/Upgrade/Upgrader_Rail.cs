using UnityEngine;

public class Upgrader_Rail : Upgrader
{
	[SerializeField] Tower_Rail tower;
	[SerializeField] Tower_Rail.Stats towerUpgrade;


	void OnValidate()
	{
		tower = GetComponent<Tower_Rail>();
	}

	public override void ApplyUpgrade()
	{
		base.ApplyUpgrade();
		tower.towerStats.Damage += Modify(tower.towerStats.Damage, towerUpgrade.Damage);
		tower.towerStats.FireRate += Modify(tower.towerStats.FireRate, towerUpgrade.FireRate);
		tower.towerStats.Range += Modify(tower.towerStats.Range, towerUpgrade.Range);

		tower.towerStats.Piercing += Modify(tower.towerStats.Piercing, towerUpgrade.Piercing);
		tower.towerStats.Size += Modify(tower.towerStats.Size, towerUpgrade.Size);
		tower.towerStats.AtkCount += Modify(tower.towerStats.AtkCount, towerUpgrade.AtkCount);
		tower.towerStats.Spread += Modify(tower.towerStats.Spread, towerUpgrade.Spread);
		tower.towerStats.LaserLength += Modify(tower.towerStats.LaserLength, towerUpgrade.LaserLength);
	}
}
