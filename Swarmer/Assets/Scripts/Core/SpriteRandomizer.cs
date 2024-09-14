using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
	[SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer sr;
	[SerializeField] Sprite defaultSprite;
	[SerializeField] float defaultChance;

	void OnEnable()
	{
		if(Random.Range(0f,100f) > defaultChance)
		{
			sr.sprite = defaultSprite;
			return;
		}
		sr.sprite = sprites[Random.Range(0, sprites.Length)];
	}
}
