using UnityEngine;

public class Pathing : MonoBehaviour
{
	[SerializeField] Rigidbody2D rb;
	public float speed;
	[SerializeField] Vector2Int coord;
	[SerializeField] Node nodeReside;
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
			nodeReside = finded;
			transform.up = nodeReside.flows.direction;
		}
	}

	void FixedUpdate()
	{
		//Moving the object
		rb.MovePosition(rb.position + (nodeReside.flows.direction.normalized * speed) * Time.fixedDeltaTime);
	}
}