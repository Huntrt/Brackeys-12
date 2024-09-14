using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
	[SerializeField] float duration, timer;

	void OnEnable()
	{
		timer = duration;
	}

	void Update()
	{
		if(duration < 0) return;
		timer -= Time.deltaTime;
		if(timer <= 0) Destroy(gameObject);
	}
}
