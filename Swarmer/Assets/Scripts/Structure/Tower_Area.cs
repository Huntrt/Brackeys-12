using UnityEngine;

public class Tower_Area : Tower
{
	[System.Serializable] public class Stats : Combat.Stats
	{
		[SerializeField] float radius; public float Radius {get => radius; set {radius = value; onStatsChange?.Invoke("radius", value);}}
		[SerializeField] float repeat; public float Repeat {get => repeat; set {repeat = value; onStatsChange?.Invoke("repeat", value);}}
		[SerializeField] float amount; public float Amount {get => amount; set {amount = value; onStatsChange?.Invoke("amount", value);}}

		public Stats SetStats(Stats statsGiven)
		{
			Radius = statsGiven.Radius;
			Repeat = statsGiven.Repeat;
			Amount = statsGiven.Amount;
			return this;
		}
	}

	public override void ShowInfo(string statsName, float modifier)
	{
		base.ShowInfo(statsName, modifier);
		infoControl.Inform("Damage", towerStats.Damage);
		infoControl.Inform("Firerate", towerStats.FireRate);
		infoControl.Inform("Range", towerStats.Range);
		infoControl.Inform("Strike Radius", towerStats.Radius);
		infoControl.Inform("Strike Amount", towerStats.Amount);
		infoControl.Inform("Repeat Attack", towerStats.Repeat);
		TowerInfoManager.i.UpdateInfo(infoControl.infos);
		TowerInfoManager.i.ShowInfo(true);
	}

	public Tower_Area.Stats towerStats;
	[SerializeField] GameObject strikeEffect;
	[SerializeField] GameObject targetEnemy;
	[SerializeField] Transform firepoint, aimer;
	float curFirerate;

	void OnEnable()
	{
		towerStats.onStatsChange += StatsChangeCaller;
		onTowerAndStrikeStatsChange += ShowInfo; //Show new info when stats change 
	}

	//When both tower and it strike stats got change
	void StatsChangeCaller(string statsName, float modifier) {onTowerAndStrikeStatsChange?.Invoke(statsName, modifier);}

	void OnDisable()
	{
		towerStats.onStatsChange -= StatsChangeCaller;
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
			//Strike base on repeat count
			for (int p = 0; p < towerStats.Repeat; p++) {Striking();}
			//Reset timer
			curFirerate -= curFirerate;
		}
	}

	void Striking()
	{
		//Create an circle it for given
		RaycastHit2D[] hits = Physics2D.CircleCastAll(aimer.transform.position, towerStats.Radius, Vector2.zero, 0, General.i.enemyLayer);
		int enemyHitted = Mathf.RoundToInt(towerStats.Amount);
		//Deal damage to enemy hit and create the effect
		if(hits.Length > 0) foreach (RaycastHit2D hit in hits)
		{
			print(hit.transform.name);
			DamageEnemy(hit.collider.gameObject, towerStats.Damage);
			Instantiate(strikeEffect, hit.transform.position, strikeEffect.transform.rotation);
			enemyHitted--;
			if(enemyHitted <= 0) return;
		}
	}
}
