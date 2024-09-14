using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Health health;
	[SerializeField] Pathing movement;

	void OnEnable()
	{
		health.onDeath += OnDeath;
		EnemyScaling.Enhancement enhancement = EnemyScaling.i.PickEnhancement();
		health.MaxHealth += enhancement.health; 
		health.SetFullHealth();
		movement.speed += enhancement.speed;
	}

	void OnDisable()
	{
		health.onDeath -= OnDeath;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.collider.CompareTag("Heart"))
		{
			Player.i.heart.DamageHeart(1);
			health.Die();
		}
	}

	public void OnDeath()
	{
		if(GameLoop.i != null) GameLoop.i.KillCouting();
	}
}
