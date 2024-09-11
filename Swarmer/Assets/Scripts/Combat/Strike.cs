using System.Collections.Generic;
using UnityEngine;

public class Strike : MonoBehaviour
{
    public Tower.Stats stats;
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