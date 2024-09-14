using UnityEngine;

public class Tower_Pattern : Tower
{
	[System.Serializable] public class Stats : Combat.Stats
	{
		[SerializeField] float spread; public float Spread {get => spread; set {spread = value; onStatsChange?.Invoke("spread", value);}}
		[SerializeField] float projectile; public float Projectile {get => projectile; set {projectile = value; onStatsChange?.Invoke("projectile", value);}}

		public Stats SetStats(Stats statsGiven)
		{
			Spread = statsGiven.Spread;
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
		infoControl.Inform("Piercing", projectileStats.Piercing);
		infoControl.Inform("Projectile Spread", towerStats.Spread);
		infoControl.Inform("Projectile Count", towerStats.Projectile);
		infoControl.Inform("Projectile Speed", projectileStats.Speed);
		infoControl.Inform("Projectile Life Time", projectileStats.Lifetime);
		if(projectileStats.ExplosionExt)
		{
			infoControl.Inform("Explosion Damage", projectileStats.Exp_Damage);
			infoControl.Inform("Explosion Radius", projectileStats.Exp_Radius);
		}
		if(projectileStats.HomingExt)
		{
			infoControl.Inform("Homing Accuracy", projectileStats.Hom_Accurate);
			infoControl.Inform("Seek Radius", projectileStats.Hom_Radius);
		}
		TowerInfoManager.i.UpdateInfo(infoControl.infos);
		TowerInfoManager.i.ShowInfo(true);
	}

	public Tower_Pattern.Stats towerStats;
	public Strike_Projectile.Stats projectileStats;
	[SerializeField] GameObject strike;
	[SerializeField] GameObject targetEnemy;
	[SerializeField] Transform firepoint, aimer;
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
			//If there is only 1 attack create 1 and set it rotation as anchor
			if(towerStats.Projectile <= 1) {Striking(aimer.rotation);}
			//If there is multiple attack
			else
			{
				//Get the spread by using it with amount
				float spread = towerStats.Projectile*towerStats.Spread;
				//Range are the focus stats got divide by 2 since it affect 2 direction
				float range = spread / 2;
				//Get the 180 to -180 rotation of the anchor
				float rot = aimer.localEulerAngles.z; float center = (rot > 180) ? rot-360 : rot;
				//Get the start and end rotation by decrease and increase the center with range
				float start = center - range; float end = center + range;
				//Get the length between each step on the spread stat
				//-1 amount cuase has 1 extra than step, e.g: A = attack | s = step | A <-s-> A <-s-> A
				float step = spread / (towerStats.Projectile-1);
				//Begin the frist angle at start
				float angle = start;
				//For each of the attack need to create
				for (int i = (int)towerStats.Projectile - 1; i >= 0 ; i--)
				{
					//Striking attack with the rotation has get
					Striking(Quaternion.Euler(0,0,angle));
					//Proceed to the next step
					angle += step;
				}
			}
			//Reset timer
			curFirerate -= curFirerate;
		}
	}

	void Striking(Quaternion rotation)
	{
		//Create the strike
		GameObject striked = Instantiate(strike, firepoint.position, rotation);
		striked.SetActive(false);
		///Set the strike stats 
		striked.GetComponent<Strike_Projectile>().stats = new Strike_Projectile.Stats(projectileStats, towerStats.Damage);
		striked.SetActive(true);
	}
}
