using UnityEngine;

	//[SerializeField] float a; public float A {get => a; set {a = value; onStatsChange?.Invoke("a", value);}}
public class Tower_Rail : Tower
{
	[System.Serializable] public class Stats : Combat.Stats
	{
		[SerializeField] float piercing; public float Piercing {get => piercing; set {piercing = value; onStatsChange?.Invoke("piercing", value);}}
		[SerializeField] float size; public float Size {get => size; set {size = value; onStatsChange?.Invoke("size", value);}}
		[SerializeField] float atkCount; public float AtkCount {get => atkCount; set {atkCount = value; onStatsChange?.Invoke("atkCount", value);}}
		[SerializeField] float spread; public float Spread {get => spread; set {spread = value; onStatsChange?.Invoke("patternSize", value);}}
		[SerializeField] float laserLength; public float LaserLength {get => laserLength; set {laserLength = value; onStatsChange?.Invoke("laserLength", value);}}

		public Stats SetStats(Stats statsGiven)
		{
			Piercing = statsGiven.Piercing;
			Size = statsGiven.Size;
			AtkCount = statsGiven.AtkCount;
			Spread = statsGiven.Spread;
			LaserLength = statsGiven.laserLength;
			return this;
		}
	}

	public override void ShowInfo(string statsName, float modifier)
	{
		base.ShowInfo(statsName, modifier);
		infoControl.Inform("Damage", towerStats.Damage);
		infoControl.Inform("Firerate", towerStats.FireRate);
		infoControl.Inform("Range", towerStats.Range);
		infoControl.Inform("Piercing", towerStats.Piercing);
		infoControl.Inform("Laser Size", towerStats.Size);
		infoControl.Inform("Laser Count", towerStats.AtkCount);
		infoControl.Inform("Pattern Spread", towerStats.Spread);
		infoControl.Inform("Laser Length", towerStats.LaserLength);
		TowerInfoManager.i.UpdateInfo(infoControl.infos);
		TowerInfoManager.i.ShowInfo(true);
	}

	public Tower_Rail.Stats towerStats;
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

	Quaternion atkDir;

	void Firing()
	{
		//Timer to strike
		curFirerate += Time.deltaTime;
		if(curFirerate >= 1/towerStats.FireRate)
		{
			//If there is only 1 attack create 1 and set it rotation as anchor
			if(towerStats.AtkCount <= 1) {Striking(aimer.rotation);}
			//If there is multiple attack
			else
			{
				//Get the spread by using it with amount
				float spread = towerStats.AtkCount*towerStats.Spread;
				//Range are the focus stats got divide by 2 since it affect 2 direction
				float range = spread / 2;
				//Get the 180 to -180 rotation of the anchor
				float rot = aimer.localEulerAngles.z; float center = (rot > 180) ? rot-360 : rot;
				//Get the start and end rotation by decrease and increase the center with range
				float start = center - range; float end = center + range;
				//Get the length between each step on the spread stat
				//-1 amount cuase has 1 extra than step, e.g: A = attack | s = step | A <-s-> A <-s-> A
				float step = spread / (towerStats.AtkCount-1);
				//Begin the frist angle at start
				float angle = start;
				//For each of the attack need to create
				for (int i = (int)towerStats.AtkCount - 1; i >= 0 ; i--)
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
		//Create the strike effect and use it transfrom as attack direction
		GameObject createdEffect = Instantiate(strikeEffect, aimer.position, rotation);
		Vector2 atkDir = createdEffect.transform.up;
		//Get line render for laser
		LineRenderer lineEffect = createdEffect.GetComponent<LineRenderer>();
		//Piercing through all enemy that got in circle cast
		int pierced = (int)towerStats.Piercing;
		RaycastHit2D[] hits = Physics2D.CircleCastAll(aimer.position, towerStats.Size/10, atkDir, towerStats.LaserLength, General.i.enemyLayer);
		foreach (RaycastHit2D hit in hits)
		{
			//Deal damage to enemy until out of pierce
			DamageEnemy(hit.collider.gameObject, towerStats.Damage);
			pierced--;
			//End laser at the last enemy hit
			if(pierced <= 0)  {lineEffect.SetPosition(1, hit.point); break;}
		}
		//Resize the laser width
		lineEffect.startWidth = towerStats.Size/10; lineEffect.endWidth = towerStats.Size/10;
		//Line start at aimer and end as the max laserlength given
		lineEffect.SetPosition(0, aimer.position); if(pierced > 0) lineEffect.SetPosition(1, atkDir * towerStats.LaserLength);
	}
}
