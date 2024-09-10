using UnityEngine;

public class FlowPathing : MonoBehaviour
{
	[SerializeField] Rigidbody2D rb;
	public float speed;
	[SerializeField] Vector2Int coord;
	[SerializeField] Node resignNode;
    Map m;

	void Start()
	{
		m = Map.i;
	}

	void Update()
	{
		coord = Map.WorldToCoordinates(transform.position);
		Node finded; if(m.FindNode(coord, out finded) != null)
		{
			resignNode = finded;
			transform.up = resignNode.flows.direction;
		}
	}

	void FixedUpdate()
	{
		//Moving the object
		rb.MovePosition(rb.position + (resignNode.flows.direction.normalized * speed) * Time.fixedDeltaTime);
	}
}