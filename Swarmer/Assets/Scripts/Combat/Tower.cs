using UnityEngine;

public class Tower : MonoBehaviour
{
    [System.Serializable] public class Stats
	{
		public delegate void OnStatsChange(string stats, float modifier); public OnStatsChange onStatsChange;
		[SerializeField] float firerate; //FRT
		public float Firerate {get {return firerate;} set {firerate = value; onStatsChange?.Invoke("firerate", value);}} 
		[SerializeField] float accuracy; //ACC
		public float Accuracy {get {return accuracy;} set {accuracy = value; onStatsChange?.Invoke("accuracy", value);}} 
		[SerializeField] float range; //RNG 
		public float Range {get {return range;} set {range = value; onStatsChange?.Invoke("range", value);}} 
		[SerializeField] float damage; //DMG
		public float Damage {get {return damage;} set {damage = value; onStatsChange?.Invoke("damage", value);}} 
		[SerializeField] float speed; //SPD
		public float Speed {get {return speed;} set {speed = value; onStatsChange?.Invoke("speed", value);}} 
		[SerializeField] float lifetime; //LFT
		public float Lifetime {get {return lifetime;} set {lifetime = value; onStatsChange?.Invoke("lifetime", value);}} 
	}
	public Stats stats = new Stats();
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
