using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class Flowfield : MonoBehaviour
{
	Map m;
	[SerializeField] bool debug;
	[SerializeField] GameObject flowArrow;
	List<GameObject> flowArrows = new List<GameObject>();

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
		foreach (Node node in m.nodes) {node.ResetFlow();}
		//Convert goal to coordinates
		Vector2Int goalCoord = Map.WorldToCoordinates(goalPos);
		//Stop if goal coord not exist
		if(m.FindNode(goalCoord) == null) return;
		///Getting the prior of all the node
		GetPrior(goalCoord);
		GetFlow();
		DrawDebug();
	}

	void GetPrior(Vector2Int goalCoord)
	{
		//Convert coordinates to goal node
		Node goalNode = m.FindNode(goalCoord);
		//Make the goal node highest priority
		goalNode.flows.cost = 0; 
		goalNode.flows.prior = 0;
		Queue<Node> nodeToCheck = new Queue<Node>();
		//Starting queue with the goal node
		nodeToCheck.Enqueue(goalNode);
		//If there still node to check
		while (nodeToCheck.Count > 0)
		{
			Node curNode = nodeToCheck.Dequeue();

			//Go through this node up, down, left and right neighbor
			List<Node> neighbors = m.GetNeighbor(curNode, true, false, false);
			foreach (Node neighbor in neighbors)
			{
				//Skip if flow cost is max
				if(neighbor.flows.cost >= ushort.MaxValue) {continue;}
				//If the neighbor have higher prior than it cost combine with this node prior
				if(neighbor.flows.cost + curNode.flows.prior < neighbor.flows.prior)
				{
					//Update the prior of neighbor
					neighbor.flows.prior = (ushort)(neighbor.flows.cost + curNode.flows.prior);
					nodeToCheck.Enqueue(neighbor);
				}
			}
		}
	}

	void GetFlow()
	{
		//Go through all the node have created
		foreach (Node node in m.nodes)
		{
			//Get all of it neighbor
			List<Node> neighbors = m.GetNeighbor(node, true, true, true);
			int priority = node.flows.prior;
			foreach (Node neighbor in neighbors)
			{
				///Update the direction if neighbor have lower prior to this node
				if(neighbor.flows.prior < priority)
				{
					priority = neighbor.flows.prior;
					node.flows.direction = neighbor.coord - node.coord;
					node.flows.nextFlow = neighbor.coord;
				}
			}
		}
	}

	void DrawDebug()
	{
		foreach (GameObject a in flowArrows) {Destroy(a);} flowArrows.Clear();
		if(!debug) return;
		foreach (Node n in m.nodes) 
		{
			GameObject flowedArrow = Instantiate(flowArrow, n.pos, Quaternion.identity);
			flowedArrow.transform.rotation = Quaternion.FromToRotation(Vector3.up, n.flows.direction);
			flowArrows.Add(flowedArrow);
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
