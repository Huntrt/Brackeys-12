using UnityEngine;

public class Tower_Turret : MonoBehaviour
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
	float curFirerate;
	[SerializeField] GameObject targetEnemy;
	[SerializeField] Transform firepoint, aimer;
	[SerializeField] LineRenderer rangeDisplay;
	[SerializeField] bool showRange;

	void OnEnable()
	{
		turretStats.onStatsChange += DrawRange;
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
		if(curFirerate >= 1/turretStats.FireRate)
		{
			for (int p = 0; p < turretStats.Projectile; p++)
			{
				Striking();
			}
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

	void RangeCheck()
	{
		rangeDisplay.gameObject.SetActive(showRange);
		//Show the range
		if(showRange) rangeDisplay.transform.localScale = Vector2.one * turretStats.Range;
		//Detect all the enemy got hit by range 
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, turretStats.Range, Vector2.zero, 0, General.i.enemyLayer);
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
		int step = 64; float radius = turretStats.Range/4;
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
		turretStats.onStatsChange -= DrawRange;
	}
}
