using UnityEngine;

public class Tower_Turret : Tower
{
	[System.Serializable] public class Stats : Combat.Stats
	{
		[SerializeField] float accuracy; public float Accuracy {get => accuracy; set {accuracy = value; onStatsChange?.Invoke("accuracy", value);}}
		[SerializeField] float projectile; public float Projectile {get => projectile; set {projectile = value; onStatsChange?.Invoke("projectile", value);}}

		public Stats SetStats(Stats statsGiven)
		{
			accuracy = statsGiven.Accuracy;
			Projectile = statsGiven.Projectile;
			return this;
		}
	}

	public Tower_Turret.Stats turretStats;
	public Strike_Projectile.Stats projectileStats;
	[SerializeField] GameObject strike;
	[SerializeField] GameObject targetEnemy;
	[SerializeField] Transform firepoint, aimer;
	[SerializeField] RangeDetector rangeDetector;
	float curFirerate;

	void OnEnable()
	{
		turretStats.onStatsChange += StatsChangeCaller;
		projectileStats.onStatsChange += StatsChangeCaller;
		turretStats.onStatsChange += ShowInfo;
		projectileStats.onStatsChange += ShowInfo;
	}

	void StatsChangeCaller(string statsName, float modifier) {onTowerStatsChange?.Invoke(statsName, modifier);}

	void OnDisable()
	{
		turretStats.onStatsChange -= StatsChangeCaller;
		projectileStats.onStatsChange -= StatsChangeCaller;
		turretStats.onStatsChange -= ShowInfo;
		projectileStats.onStatsChange -= ShowInfo;
	}

	void Update()
	{
		targetEnemy = rangeDetector.Detecting(turretStats.Range);
		if(targetEnemy != null)
		{
			//Aimer look at target enemy
			aimer.transform.up = (Vector2)(targetEnemy.transform.position - aimer.transform.position);
			Firing();
		}
	}

	void Firing()
	{
		if(targetEnemy == null) return;
		//Timer to strike
		curFirerate += Time.deltaTime;
		if(curFirerate >= 1/turretStats.FireRate)
		{
			//Strike base on projectile count
			for (int p = 0; p < turretStats.Projectile; p++) {Striking();}
			//Reset timer
			curFirerate -= curFirerate;
		}
	}

	void Striking()
	{
		//Adjust the barrel with accuracy to get firing direction
		Quaternion accurate = Quaternion.Euler(0,0,Random.Range(-turretStats.Accuracy, turretStats.Accuracy) + aimer.localEulerAngles.z);
		//Create the strike
		GameObject striked = Instantiate(strike, firepoint.position, Quaternion.identity);
		striked.SetActive(false);
		//Rotate the strike as accuracy
		striked.transform.rotation = accurate;
		///Set the strike stats 
		striked.GetComponent<Strike_Projectile>().stats = new Strike_Projectile.Stats(projectileStats, turretStats.Damage);
		striked.SetActive(true);
	}

	public override void ShowInfo(string statsName, float modifier)
	{
		base.ShowInfo(statsName, modifier);
		infoControl.Inform("Damage", turretStats.Damage);
		infoControl.Inform("Firerate", turretStats.FireRate);
		infoControl.Inform("Range", turretStats.Range);
		infoControl.Inform("Inaccuracy", turretStats.Accuracy);
		infoControl.Inform("Piercing", projectileStats.Piercing);
		infoControl.Inform("Projectile Count", turretStats.Projectile);
		infoControl.Inform("Projectile Speed", projectileStats.Speed);
		infoControl.Inform("Projectile Life Time", projectileStats.Lifetime);
		TowerInfoManager.i.UpdateInfo(infoControl.infos);
		TowerInfoManager.i.ShowInfo(true);
	}
}
