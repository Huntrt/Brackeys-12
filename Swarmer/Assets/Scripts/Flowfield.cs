using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Flowfield : MonoBehaviour
{
	Map m;
	[SerializeField] bool debug;

	void Start()
	{
		m = Map.i;
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{
			CostToGoal(General.i.MousePos());
		}
	}

	void CostToGoal(Vector2 goalPos)
	{
		//Renew all the node flows
		foreach (Node node in m.nodes) {node.flows.Renew();}
		//Convert goal to coordinates
		Vector2Int goalCoord = Map.WorldToCoordinates(goalPos);
		//Convert coordinates to goal node
		Node goalNode = m.FindNode(goalCoord);
		//Make the goal node highest priority
		goalNode.flows.cost = 0; 
		goalNode.flows.prior = 0;
		Queue<Node> nodeToCheck = new Queue<Node>();
		//Starting queue with the goal node
		nodeToCheck.Enqueue(goalNode);

		while (nodeToCheck.Count > 0)
		{
			Node curNode = nodeToCheck.Dequeue();

			List<Node> neighbors = m.GetNeighbor(curNode, true, false, false);
			for (int n = 0; n < neighbors.Count; n++)
			{
				Node neighbor = neighbors[n];
				if(neighbor.flows.cost >= ushort.MaxValue) {continue;}
				if(neighbor.flows.cost + curNode.flows.prior < neighbor.flows.prior)
				{
					neighbor.flows.prior = (ushort)(neighbor.flows.cost + curNode.flows.prior);
					nodeToCheck.Enqueue(neighbor);
				}
			}
		}
	}

	void OnDrawGizmos()
	{
		if(!debug) return;
		if(m != null) foreach (Node node in m.nodes)
		{
			if(node == null) break;
			Handles.Label(node.pos, node.flows.prior.ToString());
		}
	}
}
