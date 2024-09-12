using UnityEngine;

public class Player : MonoBehaviour
{
	#region Set this class to singleton
	static Player _i; public static Player i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindObjectOfType<Player>();
			}
			return _i;
		}
	}
	#endregion
    
	public GameObject heartObj;
	public int curHeart, maxHeart;
	//test: Test variable
	public GameObject previewer;
	[SerializeField] Node hoverNode; public Node HoverNode {get => hoverNode;}
	public Vector2Int mouseCoord; public Vector2Int MouseCoord {get => mouseCoord;}
	public GameObject buildPanel;
	General g;

	void OnEnable()
	{
		Map.i.onMapCreated += CreateHeart;
	}

	void Start()
	{
		g = General.i;
		curHeart = maxHeart;
	}
	
	void Update()
	{
		//? Convert mouse position to node coordinates
		mouseCoord = Map.WorldToCoordinates(g.MousePos());
		//If there an node exist at mouse coordinates and build panel is closed
		if(Map.i.nodeIndexs.ContainsKey(mouseCoord) && !buildPanel.activeInHierarchy)
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

	public void BuildStructure(GameObject testStructure)
	{
		string buildStatus;
		Builder.BuildAtNode(hoverNode, testStructure, out buildStatus); 
		print(buildStatus);
		buildPanel.SetActive(false);
	}

	void CreateHeart(Vector2Int chunk)
	{
		//Create the heart in the center node of chunk 0
		if(chunk == Vector2Int.zero)
		{
			heartObj = Builder.BuildAtNode(Map.i.FindNode(Vector2Int.zero), heartObj);
			Map.i.onMapCreated -= CreateHeart;
		}
	}

	public void DamageHeart(int taken)
	{
		curHeart -= taken;
		if(curHeart <= 0)
		{
			print("Game Over");
		}
	}
}
