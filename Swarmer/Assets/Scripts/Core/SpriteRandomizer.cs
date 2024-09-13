using UnityEngine;

public class SpriteRandomizer : MonoBehaviour
{
	[SerializeField] Sprite[] sprites;
    [SerializeField] SpriteRenderer sr;

	void OnEnable()
	{
		sr.sprite = sprites[Random.Range(0, sprites.Length)];
	}
}
