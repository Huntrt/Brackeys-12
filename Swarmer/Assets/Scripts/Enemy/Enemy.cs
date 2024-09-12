using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Health health;

	void OnEnable()
	{
		health.onDeath += OnDeath;
	}

	void OnDisable()
	{
		health.onDeath -= OnDeath;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.collider.CompareTag("Heart"))
		{
			Player.i.DamageHeart(1);
		}
	}

	public void OnDeath()
	{
		GameLoop.i.KillCouting();
	}
}
