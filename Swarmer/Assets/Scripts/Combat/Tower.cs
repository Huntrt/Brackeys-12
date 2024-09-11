using UnityEngine;

public class Tower : MonoBehaviour
{
    [System.Serializable] public class Stats
	{
		public delegate void OnStatsChange(string stats, float modifier); public OnStatsChange onStatsChange;
		[SerializeField] float firerate; //FRT
		[SerializeField] float accuracy; //ACC
		[SerializeField] float range; //RNG 
		[SerializeField] int projectile; //PRO
		[SerializeField] float damage; //DMG
		[SerializeField] float piercing; //PIE
		[SerializeField] float speed; //SPD
		[SerializeField] float lifetime; //LFT
		public float Firerate {get {return firerate;} set {firerate = value; onStatsChange?.Invoke("firerate", value);}} 
		public float Accuracy {get {return accuracy;} set {accuracy = value; onStatsChange?.Invoke("accuracy", value);}} 
		public float Range {get {return range;} set {range = value; onStatsChange?.Invoke("range", value);}} 
		public int Projectile {get {return projectile;} set {projectile = value; onStatsChange?.Invoke("projectile", value);}} 
		public float Damage {get {return damage;} set {damage = value; onStatsChange?.Invoke("damage", value);}} 
		public float Piercing {get {return piercing;} set {piercing = value; onStatsChange?.Invoke("piercing", value);}} 
		public float Speed {get {return speed;} set {speed = value; onStatsChange?.Invoke("speed", value);}} 
		public float Lifetime {get {return lifetime;} set {lifetime = value; onStatsChange?.Invoke("lifetime", value);}} 

		public Stats(Stats statsGiven)
		{
			firerate = statsGiven.Firerate;
			accuracy = statsGiven.Accuracy;
			range = statsGiven.Range;
			projectile = statsGiven.projectile;
			damage = statsGiven.Damage;
			piercing = statsGiven.piercing;
			speed = statsGiven.speed;
			lifetime = statsGiven.Lifetime;
		}
	}
	public Stats stats;
	[SerializeField] GameObject strike;
	float curFirerate;
	[SerializeField] GameObject targetEnemy;
	[SerializeField] Transform firepoint, aimer;
	[SerializeField] LineRenderer rangeDisplay;
	[SerializeField] bool showRange;

	void OnEnable()
	{
		stats.onStatsChange += DrawRange;
		ToggleRangeDisplay(true);
	}

	void Update()
	{
		RangeCheck();
		Firing();
	}

	void Firing()
	{
		if(targetEnemy == null) return;
		curFirerate += Time.deltaTime;
		if(curFirerate >= 1/stats.Firerate)
		{
			for (int p = 0; p < stats.Projectile; p++)
			{
				Striking();
			}
			curFirerate -= curFirerate;
		}
	}

	void Striking()
	{
		Quaternion accurate = Quaternion.Euler(0,0,Random.Range(-stats.Accuracy, stats.Accuracy) + aimer.localEulerAngles.z);
		GameObject striked = Instantiate(strike, firepoint.position, Quaternion.identity);
		striked.SetActive(false);
		striked.transform.rotation = accurate; 
		striked.GetComponent<Strike>().stats = new Stats(stats);
		striked.SetActive(true);
	}

	void RangeCheck()
	{
		rangeDisplay.gameObject.SetActive(showRange);
		//Show the range
		if(showRange) rangeDisplay.transform.localScale = Vector2.one * stats.Range;
		//Detect all the enemy got hit by range 
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, stats.Range, Vector2.zero, 0, General.i.enemyLayer);
		///Target the nearest enemy in range
		float nearestDistance = int.MaxValue;
		Transform nearestEnemy = null;
		if(hits.Length > 0)
		{
			foreach (RaycastHit2D hit in hits)
			{
				if(nearestDistance > Vector2.Distance(transform.position, hit.transform.position))
				{
					nearestDistance = Vector2.Distance(transform.position, hit.transform.position);
					nearestEnemy = hit.transform;
				}
			}
			targetEnemy = nearestEnemy.gameObject;
			aimer.transform.up = (Vector2)(targetEnemy.transform.position - aimer.transform.position);
		}
		else
		{
			targetEnemy = null;
		}
	}

	public void ToggleRangeDisplay(bool state)
	{
		DrawRange("range", 0);
		rangeDisplay.gameObject.SetActive(state);
	}

	public void DrawRange(string statChanged, float value)
	{
		if(statChanged != "range") return;
		int step = 64; float radius = stats.Range/4;
		rangeDisplay.positionCount = step;
		for (int s = 0; s < step; s++)
		{
			float progress = (float)s/(step-1);
			float radian = progress * 2 * Mathf.PI;
			float x = Mathf.Cos(radian) * radius;
			float y = Mathf.Sin(radian) * radius;
			Vector3 pos = new Vector3(x,y,0);

			rangeDisplay.SetPosition(s, pos);
		}
	}

	void OnDisable()
	{
		stats.onStatsChange -= DrawRange;
	}
}
