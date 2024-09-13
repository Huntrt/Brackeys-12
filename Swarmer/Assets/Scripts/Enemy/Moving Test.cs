using UnityEngine;

public class MovingTest : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
	[SerializeField] Transform pA, pB;
	[SerializeField] float speed;
	[SerializeField] Vector2 dir;

	void OnValidate()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		if(rb.position.x <= pA.position.x)
		{
			dir = Vector2.right;
		}
		if(rb.position.x >= pB.position.x)
		{
			dir = Vector2.left;
		}
		//Moving the object
		rb.MovePosition(rb.position + (dir * speed) * Time.fixedDeltaTime);
	}
}
