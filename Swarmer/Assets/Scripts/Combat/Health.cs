using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float curHealth, maxHealth;
	public float MaxHealth {get; set;}
	public delegate void OnHurt(float taken); public delegate void OnHeal(float taken);
	public OnHurt onHurt; public OnHeal onHeal;

	void OnEnable()
	{
		curHealth = maxHealth;
	}

	public void Damaging(float taken)
	{
		curHealth -= taken;
		onHurt?.Invoke(taken);
		if(curHealth <= 0)
		{
			Die();
		}
	}

	public void Healing(float taken)
	{
		curHealth += taken;
		curHealth = Mathf.Clamp(curHealth, 0, maxHealth);
		onHeal?.Invoke(taken);
	}

	public void Die()
	{
		//temp: pool later
		Destroy(gameObject);
	}
}
