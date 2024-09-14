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

	public Heart heart;
	public GameObject previewer;
	[SerializeField] Node hoverNode; public Node HoverNode {get => hoverNode;}
	Tower hoverTower;
	public Vector2Int mouseCoord; public Vector2Int MouseCoord {get => mouseCoord;}
	[Header("UI")]
	public GameObject buildPanel;
	[SerializeField] GameObject layer1Panel, layer2Panel;
	[SerializeField] GameObject sellPanel;
	[SerializeField] TMPro.TextMeshProUGUI sellAmountTxt;
	[SerializeField] GameObject upgradePanel;
	[SerializeField] TMPro.TextMeshProUGUI upgradeCostTxt;
	General g;

	void Start()
	{
		g = General.i;
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
			///Show build panel when right click
			if(Input.GetKeyDown(KeyCode.Mouse0))
			{
				ShowHoverPanel();
			}
		}
		//test: Hide the build ui when right click
		if(Input.GetKeyDown(KeyCode.Mouse1) && buildPanel.activeInHierarchy)
		{
			HideHoverPanel();
			return;
		}
	}

	void ShowHoverPanel()
	{
		ShowTowerInfo();
		ShowBuildPanel();
		ShowSellAndInfoPanel();
	}

	void ShowTowerInfo()
	{
		GameObject hoverObj = hoverNode.occupations[2].obj;
		if(hoverObj == null) return;
		hoverTower = hoverObj.GetComponent<Tower>();
		//Show tower info
		hoverTower.ShowInfo("", -1);
		//Show tower range
		hoverTower.rangeDetector.RangeDisplay(true);
		//Update the upgrade cost display
		upgradeCostTxt.text = "UP " + hoverTower.upgrader.CurCost + "$";
		upgradePanel.SetActive(true);
	}

	void ShowSellAndInfoPanel()
	{
		//Go through to check if hover node have occupation
		for (int i = 2; i >= 1 ; i--)if(hoverNode.HaveOccupation(i))
		{
			//Show info of the occupation structure
			StructureInfo.i.ShowInfo(hoverNode.occupations[i].component);
			//Show the sell info if the structure allow to sell
			GameObject structure = hoverNode.occupations[i].obj;
			Buyable buyable = structure.GetComponent<Buyable>();
			if(buyable != null)
			{
				sellPanel.SetActive(true);
				sellAmountTxt.text = "Sell +" + buyable.sellAmount + "$";
				break;
			}
		}
	}

	void ShowBuildPanel()
	{
		//Move build panel to cursour
		buildPanel.transform.position = g.cam.WorldToScreenPoint(hoverNode.pos);
		//If there still occupation spot left
		if(!hoverNode.HaveOccupation(2))
		{
			//Show layer 2 panel if layer 1 occupy and allow to tower
			if(hoverNode.HaveOccupation(1))
			{
				if(HoverNode.occupations[1].component.towerable)
				{
					layer2Panel.SetActive(true);
				}
			}
			//Show layer 1 panel when there no occupation at that layer
			else
			{
				layer1Panel.SetActive(true);
			}
		}
		buildPanel.SetActive(true);
	}

	public void HideHoverPanel()
	{
		buildPanel.SetActive(false);
		layer1Panel.SetActive(false);
		layer2Panel.SetActive(false);
		sellPanel.SetActive(false);
		upgradePanel.SetActive(false);
		StructureInfo.i.CloseInfo();
		TowerInfoManager.i.ShowInfo(false);
		if(hoverTower != null) hoverTower.rangeDetector.RangeDisplay(false);
	}

	public void UpgradeHoverTower()
	{
		if(Economy.i.SpendCheck(hoverTower.upgrader.CurCost))
		{
			//Spending money to upgrade
			Economy.i.Spend(hoverTower.upgrader.CurCost);
			//Upgrade the htower hovering
			hoverTower.Upgrading();
			//Update the info of the tower just upgrade 
			StructureInfo.i.ShowInfo(hoverNode.occupations[2].component);
			//Update the upgrade cost display
			upgradeCostTxt.text = "UP " + hoverTower.upgrader.CurCost + "$";
		}
		else
		{
			print("Not enough money to upgrade");
		}
	}

	public bool PlaceStructure(GameObject structure)
	{
		string buildStatus;
		bool placed = BuilderManager.BuildAtNode(hoverNode, structure, out buildStatus);
		HideHoverPanel();
		return placed;
	}
}
