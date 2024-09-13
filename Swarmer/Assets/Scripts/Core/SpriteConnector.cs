using UnityEngine;

public class SpriteConnector : MonoBehaviour
{
    [SerializeField] Sprite all, blank, d, dl, dr, l, lr, r, u, ud, ul, ur;
	[SerializeField] SpriteRenderer sr;
	[SerializeField] GameObject connectThisObj;

	void OnEnable()
	{
		Node nodeResign = Map.i.FindNode(Map.WorldToCoordinates(transform.position));
		Vector2 nodeC = nodeResign.coord;
		foreach (Node neighbor in Map.i.GetNeighbor(nodeResign, true, true, false))
		{
			Vector2Int neighborC = neighbor.coord;
			if(neighbor.HaveOccupation(1)) if(neighbor.occupations[1].obj == connectThisObj)
			{
				Vector2 connectedNeighbor = Vector2.zero;
				connectedNeighbor += neighbor.coord - nodeResign.coord;
				
			}
		}
		// if(neighborC == nodeC + Vector2.up) sr.sprite = u;
		// if(neighborC == nodeC + Vector2.down) sr.sprite = d;
		// if(neighborC == nodeC + Vector2.left) sr.sprite = l;
		// if(neighborC == nodeC + Vector2.right) sr.sprite = r;

		// if(neighborC == nodeC + Vector2.up + Vector2.down) sr.sprite = ud;

		// if(neighborC == nodeC + Vector2.up + Vector2.left) sr.sprite = ul;
		// if(neighborC == nodeC + Vector2.up + Vector2.right) sr.sprite = ur;

		// if(neighborC == nodeC + Vector2.down + Vector2.left) sr.sprite = dl;
		// if(neighborC == nodeC + Vector2.down + Vector2.right) sr.sprite = dr;

		// if(neighborC == nodeC + Vector2.left) sr.sprite = d;
	}
}
