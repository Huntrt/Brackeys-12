using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] int loot;
    [SerializeField] Health health;
	[SerializeField] Pathing movement;

	void OnEnable()
	{
		health.onDeath += OnDeath;
		EnemyScaling.Enhancement enhancement = EnemyScaling.i.PickEnhancement();
		health.MaxHealth += enhancement.health; 
		health.SetFullHealth();
		movement.speed += enhancement.speed;
		loot += Mathf.RoundToInt(enhancement.loot);
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
		if(Economy.i != null) Economy.i.Earn(loot);
		if(GameLoop.i != null) GameLoop.i.KillCouting();
	}
}
