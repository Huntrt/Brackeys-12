using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
	[SerializeField] float duration, timer;
	[SerializeField] Animation anim;

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

	public void SelfDeactive()
	{
		if(anim != null) {anim.Stop(); anim.Rewind();}
		gameObject.SetActive(false);
	}
}
