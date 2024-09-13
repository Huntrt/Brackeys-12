using UnityEngine;

public class RangeDetector : MonoBehaviour
{
	[SerializeField] Tower tower;
	[SerializeField] LineRenderer render;
	[SerializeField] bool showRange;
	float range;

	void OnEnable()
	{
		tower.onTowerStatsChange += DrawRange;
	}

	void OnDisable()
	{
		tower.onTowerStatsChange -= DrawRange;
	}

	void Update()
	{
		//temp: range display test
		if(Input.GetKeyDown(KeyCode.R))
		{
			RangeDisplay(!render.gameObject.activeInHierarchy);
		}
	}

	public void RangeDisplay(bool show)
	{
		if(show) DrawRange("range", 0);
		render.gameObject.SetActive(show);
	}

    public GameObject Detecting(float towerRange)
	{
		range = towerRange;
		GameObject detected = null;
		//Detect all the enemy got hit by range 
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, range, Vector2.zero, 0, General.i.enemyLayer);
		///Target the nearest enemy in range
		float nearestDistance = int.MaxValue;
		Transform nearestEnemy = null;
		//Go through all hitted enemy
		if(hits.Length > 0)
		{
			foreach (RaycastHit2D hit in hits)
			{
				//Get the enemy that nearest
				float dist = Vector2.Distance(transform.position, hit.transform.position);
				if(nearestDistance > dist) {nearestDistance = dist; nearestEnemy = hit.transform;}
			}
			detected = nearestEnemy.gameObject;
		}
		return detected;
	}

	public void DrawRange(string statsName, float modifier)
	{
		if(statsName != "range") return;
		int step = 32;
		render.positionCount = step;
		for (int s = 0; s < step; s++)
		{
			float progress = (float)s/(step-1);
			float radian = progress * 2 * Mathf.PI;
			float x = Mathf.Cos(radian) * range;
			float y = Mathf.Sin(radian) * range;
			Vector3 pos = new Vector3(x,y,0);

			render.SetPosition(s, pos);
		}
	}

}
