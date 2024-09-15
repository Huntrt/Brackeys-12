using UnityEngine;

public class Strike_Homing : MonoBehaviour
{
	[SerializeField] Strike_Projectile projectile;
	[SerializeField] float accuracy, range;
	GameObject target; bool seeked;

	void OnEnable()
	{
		if(projectile.stats.HomingExt)
		{
			accuracy = projectile.stats.Hom_Accurate;
			range = projectile.stats.Hom_Radius;
		}
	}

	void Update()
    {
		///If no target -> search -> active? ┬Y-> get it location -> look at it
		///									 └N-> search another one -> get it location -> look at it
		///-> stop looking if get to release range
		///This process will reset when target got deactive
		//If allow to homing and haven't seek any enemy
		if(projectile.stats.HomingExt && !seeked) {Homing();}
    }


	void Homing()
	{
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
		if(detected != null)
		{
			//If there is no target than mark the nearest enemy as target
			if(target == null) {target = detected;}
			//Get the direction of projectile
			transform.up = Vector2.Lerp(transform.up, target.transform.position - transform.position, accuracy * Time.deltaTime);
		}
	}
}
