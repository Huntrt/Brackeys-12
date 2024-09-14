using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float curHealth, maxHealth;
	public float MaxHealth {get; set;}
	public delegate void OnHurt(float taken); public delegate void OnHeal(float taken); public delegate void OnDeath();
	public OnHurt onHurt; public OnHeal onHeal; public OnDeath onDeath;

	void OnEnable()
	{
		curHealth = maxHealth;
	}

	public void Damaging(float taken)
	{
		taken = Mathf.Clamp(taken, 0, Mathf.Infinity);
		curHealth -= taken;
		onHurt?.Invoke(taken);
		if(curHealth <= 0)
		{
			Die();
		}
	}

	public void Healing(float taken)
	{
		taken = Mathf.Clamp(taken, 0, Mathf.Infinity);
		curHealth += taken;
		curHealth = Mathf.Clamp(curHealth, 0, maxHealth);
		onHeal?.Invoke(taken);
	}

	public void Die()
	{
		onDeath?.Invoke();
		//temp: pool later
		Destroy(gameObject);
	}
}
