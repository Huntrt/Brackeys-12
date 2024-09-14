using System.Collections.Generic;
using UnityEngine;

public class Strike_Projectile : MonoBehaviour
{
	[System.Serializable] public class Stats
	{
		public delegate void OnStatsChange(string stats, float modifier); public OnStatsChange onStatsChange;
		float damage; public float Damage {get => damage; set {damage = value; onStatsChange?.Invoke("damage", value);}}
		[SerializeField] float speed; public float Speed {get => speed; set {speed = value; onStatsChange?.Invoke("speed", value);}} 
		[SerializeField] float lifetime; public float Lifetime {get => lifetime; set {lifetime = value; onStatsChange?.Invoke("lifetime", value);}}
		[SerializeField] float piercing; public float Piercing {get => piercing; set {piercing = value; onStatsChange?.Invoke("piercing", value);}}
		[Header("Explosion Extension")]
		[SerializeField] bool explosionExt; public bool ExplosionExt {get => explosionExt;}
		[SerializeField] float exp_damage; public float Exp_Damage {get => exp_damage; set {exp_damage = value; onStatsChange?.Invoke("exp_damage", value);}}
		[SerializeField] float exp_radius; public float Exp_Radius {get => exp_radius; set {exp_radius = value; onStatsChange?.Invoke("exp_radius", value);}}
		[Header("Homing Extension")]
		[SerializeField] bool homingExt; public bool HomingExt {get => homingExt;}
		[SerializeField] float hom_accurate; public float Hom_Accurate {get => hom_accurate; set {hom_accurate = value; onStatsChange?.Invoke("hom_accurate", value);}}
		[SerializeField] float hom_radius; public float Hom_Radius {get => hom_radius; set {hom_radius = value; onStatsChange?.Invoke("hom_radius", value);}}

		public Stats(Stats given, float damage)
		{
			Damage = damage;
			Speed = Mathf.Clamp(given.Speed, 0, 30);
			Lifetime = given.Lifetime;
			Piercing = given.piercing;
			explosionExt = given.explosionExt;
			Exp_Damage = given.Exp_Damage;
			Exp_Radius = given.Exp_Radius;
			homingExt = given.HomingExt;
			Hom_Accurate = given.Hom_Accurate;
			Hom_Radius = given.Hom_Radius;
		}
	}
	public bool explodeOnDeath, explodeOnHit; [SerializeField] GameObject explosionPrf;
	public Stats stats;
	[SerializeField] Rigidbody2D rb;
	[SerializeField] Collider2D col;
	float curLifetime;
	List<Collider2D> pierces = new List<Collider2D>();

	void OnEnable()
	{	
		//Reset all pierced target
		foreach (Collider2D pierce in pierces) {Physics2D.IgnoreCollision(col, pierce, false);}
		pierces = new List<Collider2D>();
		//Reset life time
		curLifetime -= curLifetime;
	}

	void Update()
	{
		//End strike when it out of life time
		curLifetime += Time.deltaTime;
		if(curLifetime >= stats.Lifetime) {End();}
	}

	void FixedUpdate()
	{
		//Moving the strike
		rb.MovePosition(rb.position + ((Vector2)transform.up * stats.Speed) * Time.fixedDeltaTime);
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		//Damage the enemy when collide with it
		if(other.collider.CompareTag("Enemy"))
		{
			other.transform.GetComponent<Health>().Damaging(stats.Damage);
			//Create explosion when hit
			if(explodeOnHit) CreateExplosion(other.transform.position);
			//Counting enemt pierced and end when ran out of pierce
			pierces.Add(other.collider);
			Physics2D.IgnoreCollision(col, other.collider);
			if(pierces.Count >= stats.Piercing) End();
		}
	}

	public void End()
	{
		if(explodeOnDeath) CreateExplosion(transform.position);
		//temp: bullet pool
		Destroy(gameObject);
	}

	public void CreateExplosion(Vector2 pos)
	{
		if(!stats.ExplosionExt) return;
		GameObject explosion = Instantiate(explosionPrf, pos, explosionPrf.transform.rotation);
		explosion.GetComponent<Strike_Explosion>().Explode(stats.Exp_Damage, stats.Exp_Radius);
	}
}