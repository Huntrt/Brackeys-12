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

	public override void ShowInfo(string statsName, float modifier)
	{
		base.ShowInfo(statsName, modifier);
		infoControl.Inform("Damage", towerStats.Damage);
		infoControl.Inform("Firerate", towerStats.FireRate);
		infoControl.Inform("Range", towerStats.Range);
		infoControl.Inform("Inaccuracy", towerStats.Accuracy);
		infoControl.Inform("Piercing", projectileStats.Piercing);
		infoControl.Inform("Projectile Count", towerStats.Projectile);
		infoControl.Inform("Projectile Speed", projectileStats.Speed);
		infoControl.Inform("Projectile Life Time", projectileStats.Lifetime);
		TowerInfoManager.i.UpdateInfo(infoControl.infos);
		TowerInfoManager.i.ShowInfo(true);
	}

	public Tower_Turret.Stats towerStats;
	public Strike_Projectile.Stats projectileStats;
	[SerializeField] GameObject strike;
	[SerializeField] GameObject targetEnemy;
	[SerializeField] Transform firepoint, aimer;
	[SerializeField] RangeDetector rangeDetector;
	float curFirerate;

	void OnEnable()
	{
		towerStats.onStatsChange += StatsChangeCaller;
		projectileStats.onStatsChange += StatsChangeCaller;
		onTowerAndStrikeStatsChange += ShowInfo; //Show new info when stats change 
	}

	//When both tower and it strike stats got change
	void StatsChangeCaller(string statsName, float modifier) {onTowerAndStrikeStatsChange?.Invoke(statsName, modifier);}

	void OnDisable()
	{
		towerStats.onStatsChange -= StatsChangeCaller;
		projectileStats.onStatsChange -= StatsChangeCaller;
		onTowerAndStrikeStatsChange -= ShowInfo;
	}

	void Update()
	{
		targetEnemy = rangeDetector.Detecting(towerStats.Range);
		if(targetEnemy != null)
		{
			//Aimer look at target enemy
			aimer.transform.up = (Vector2)(targetEnemy.transform.position - aimer.transform.position);
			Firing();
		}
	}

	void Firing()
	{
		//Timer to strike
		curFirerate += Time.deltaTime;
		if(curFirerate >= 1/towerStats.FireRate)
		{
			//Strike base on projectile count
			for (int p = 0; p < towerStats.Projectile; p++) {Striking();}
			//Reset timer
			curFirerate -= curFirerate;
		}
	}

	void Striking()
	{
		//Adjust the barrel with accuracy to get firing direction
		Quaternion accurate = Quaternion.Euler(0,0,Random.Range(-towerStats.Accuracy, towerStats.Accuracy) + aimer.localEulerAngles.z);
		//Create the strike
		GameObject striked = Instantiate(strike, firepoint.position, Quaternion.identity);
		striked.SetActive(false);
		//Rotate the strike as accuracy
		striked.transform.rotation = accurate;
		///Set the strike stats 
		striked.GetComponent<Strike_Projectile>().stats = new Strike_Projectile.Stats(projectileStats, towerStats.Damage);
		striked.SetActive(true);
	}
}
