using UnityEngine;

public class Builder : MonoBehaviour
{
	//test: Test variable
	public GameObject previewer;
	Node hoverNode; public Node HoverNode {get => hoverNode;}
	public Vector2Int mouseCoord; public Vector2Int MouseCoord {get => mouseCoord;}
	public GameObject buildPanel;
	General g;


	void Start()
	{
		g = General.i;
	}

	void Update()
	{
		//? Convert mouse position to node coordinates
		mouseCoord = Map.WorldToCoordinates(g.MousePos());
		//If there an node exist at mouse coordinates
		if(Map.i.nodeIndexs.ContainsKey(mouseCoord))
		{
			//Move the preview and get node got hover
			previewer.transform.position = Map.SnapPosition(g.MousePos());
			hoverNode = Map.i.nodeIndexs[mouseCoord];
			if(Input.GetKeyDown(KeyCode.Mouse0))
			{
				ShowBuildPanel();
			}
		}
		//test: Hide the build ui when right click
		if(Input.GetKeyDown(KeyCode.Mouse1) && buildPanel.activeInHierarchy)
		{
			buildPanel.SetActive(false);
			return;
		}
	}

	void ShowBuildPanel()
	{
		buildPanel.transform.position = g.cam.WorldToScreenPoint(hoverNode.pos);
		buildPanel.SetActive(true);
	}
}
