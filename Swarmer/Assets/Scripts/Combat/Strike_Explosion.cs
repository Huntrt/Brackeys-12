using UnityEngine;

public class Strike_Explosion : MonoBehaviour
{
    [SerializeField] float damage, radius;
	[SerializeField] ParticleSystem effect;

	public void Explode(float damage, float radius)
	{
		//Set given stats
		this.damage = damage;
		this.radius = radius;
		//Create circle cast that deal damage to all enemy inside it
		RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, 0, General.i.enemyLayer);
		if(hits.Length > 0) foreach (RaycastHit2D hit in hits) {hit.collider.GetComponent<Health>().Damaging(damage);}
		//Increase effect particle radius as given radius stats
		ParticleSystem.ShapeModule shape = effect.shape; shape.radius = radius;
		//Emit the effect with particle amount scaled with explode radius
		effect.Emit(Mathf.RoundToInt(radius * 10));
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere(transform.position, radius);
	}
}
