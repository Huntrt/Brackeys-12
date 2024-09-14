using UnityEngine;

public class HealthEffect : MonoBehaviour
{
	[SerializeField] Animation hurtAnimation;
	[SerializeField] Health health;

	void OnValidate()
	{
		health = GetComponent<Health>();
	}

	void OnEnable()
	{
		health.onHurt += StartHurtEffect;
	}

	void OnDisable()
	{
		health.onHurt -= StartHurtEffect;
	}

	void StartHurtEffect(float taken)
	{
		hurtAnimation.Stop();
		hurtAnimation.Play();
	}
}
