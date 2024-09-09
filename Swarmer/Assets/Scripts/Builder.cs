using System.IO.Compression;
using UnityEngine;

public class Builder : MonoBehaviour
{
	//test: Test variable
	public GameObject previewer;
	Node hoverNode; public Node HoverNode {get => hoverNode;}
	public Vector2Int mouseCoord; public Vector2Int MouseCoord {get => mouseCoord;}
	General g;


	void Start()
	{
		g = General.i;
	}

	void Update()
	{
		//? Convert mouse position to node coordinates
		Vector2 spacedMousePos = g.MousePos() / Map.i.Spacing;
		mouseCoord = new Vector2Int
		(
			Mathf.RoundToInt(spacedMousePos.x),
			Mathf.RoundToInt(spacedMousePos.y)
		);
		//If there an node exist at mouse coordinates
		if(Map.i.nodeIndexs.ContainsKey(mouseCoord))
		{
			//Move the preview and get node got hover
			previewer.transform.position = g.MouseSnap();
			hoverNode = Map.i.nodeIndexs[mouseCoord];
		}
	}
}
