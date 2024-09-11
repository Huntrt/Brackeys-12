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

	public void OnDeath()
	{
		GameLoop.i.KillCouting();
	}
}
