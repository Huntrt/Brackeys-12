using System.Collections.Generic;
using UnityEngine;

public class Strike_Projectile : MonoBehaviour
{
	[System.Serializable] public class Stats
	{
		public delegate void OnStatsChange(string stats, float modifier); public OnStatsChange onStatsChange;
		[SerializeField] float damage; public float Damage {get => damage; set {damage = value; onStatsChange?.Invoke("damage", value);}}
		[SerializeField] float speed; public float Speed {get => speed; set {speed = value; onStatsChange?.Invoke("speed", value);}} 
		[SerializeField] float lifetime; public float Lifetime {get => lifetime; set {lifetime = value; onStatsChange?.Invoke("lifetime", value);}}
		[SerializeField] float piercing; public float Piercing {get => piercing; set {piercing = value; onStatsChange?.Invoke("piercing", value);}}
		
		public Stats(Stats given, float damage)
		{
			Damage = damage;
			Speed = given.Speed;
			Lifetime = given.Lifetime;
			Piercing = given.piercing;
		}
	}
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
			//Counting enemt pierced and end when ran out of pierce
			pierces.Add(other.collider);
			Physics2D.IgnoreCollision(col, other.collider);
			if(pierces.Count >= stats.Piercing) End();
		}
	}

	public void End()
	{
		//temp: bullet pool
		Destroy(gameObject);
	}
}