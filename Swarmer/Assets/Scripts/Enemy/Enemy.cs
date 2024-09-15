using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField] int loot;
    [SerializeField] Health health;
	[SerializeField] Pathing movement;

	void OnEnable()
	{
		health.onDeath += OnDeath;
		if(GameLoop.i != null) GameLoop.onLevelComplete += LevelCleanup;
		//Enhance enemy stats
		if(EnemyScaling.i == null) return;
		EnemyScaling.Enhancement enhancement = EnemyScaling.i.PickEnhancement();
		health.MaxHealth += enhancement.health; 
		health.SetFullHealth();
		movement.speed += enhancement.speed;
		loot += Mathf.RoundToInt(enhancement.loot);
	}

	void OnDisable()
	{
		health.onDeath -= OnDeath;
		if(GameLoop.i != null) GameLoop.onLevelComplete -= LevelCleanup;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.collider.CompareTag("Heart"))
		{
			Player.i.heart.DamageHeart(1);
			if(health != null) health.Die();
		}
	}

	public void LevelCleanup(int lv)
	{
		if(health != null) health.Die(false);
	}

	public void OnDeath()
	{
		if(Economy.i != null) Economy.i.Earn(loot);
		if(GameLoop.i != null) GameLoop.i.KillCouting();
	}
}
