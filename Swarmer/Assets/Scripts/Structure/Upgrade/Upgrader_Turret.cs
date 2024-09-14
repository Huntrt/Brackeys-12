using UnityEngine;

public class Upgrader_Turret : Upgrader
{
	[SerializeField] Tower_Turret tower;
	[SerializeField] Tower_Turret.Stats towerUpgrade;
	[SerializeField] Strike_Projectile.Stats strikeUpgrade;

	void OnValidate()
	{
		tower = GetComponent<Tower_Turret>();
	}

	public override void ApplyUpgrade()
	{
		base.ApplyUpgrade();
		tower.towerStats.Damage += Modify(tower.towerStats.Damage, towerUpgrade.Damage);
		tower.towerStats.FireRate += Modify(tower.towerStats.FireRate, towerUpgrade.FireRate);
		tower.towerStats.Range += Modify(tower.towerStats.Range, towerUpgrade.Range);

		tower.towerStats.Accuracy += Modify(tower.towerStats.Accuracy, towerUpgrade.Accuracy);
		tower.towerStats.Projectile += Modify(tower.towerStats.Projectile, towerUpgrade.Projectile);
		
		tower.projectileStats.Speed += Modify(tower.projectileStats.Speed, strikeUpgrade.Speed);
		tower.projectileStats.Lifetime += Modify(tower.projectileStats.Lifetime, strikeUpgrade.Lifetime);
		tower.projectileStats.Piercing += Modify(tower.projectileStats.Piercing, strikeUpgrade.Piercing);

		tower.projectileStats.Exp_Damage += Modify(tower.projectileStats.Exp_Damage, strikeUpgrade.Exp_Damage);
		tower.projectileStats.Exp_Radius += Modify(tower.projectileStats.Exp_Radius, strikeUpgrade.Exp_Radius);

		tower.projectileStats.Hom_Accurate += Modify(tower.projectileStats.Hom_Accurate, strikeUpgrade.Hom_Accurate);
		tower.projectileStats.Hom_Radius += Modify(tower.projectileStats.Hom_Radius, strikeUpgrade.Hom_Radius);

	}
}
