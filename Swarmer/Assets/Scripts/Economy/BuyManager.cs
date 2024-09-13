using UnityEngine;

public class BuyManager : MonoBehaviour
{
	#region Set this class to singleton
	static BuyManager _i; public static BuyManager i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindObjectOfType<BuyManager>();
			}
			return _i;
		}
	}
	#endregion

	void Update()
	{
		//temp: sell test
		if(Input.GetKeyDown(KeyCode.S))
		{
			Sell();
		}
	}

	public void Buy(GameObject structure, Structure data, Buyable buyable)
	{
		//Check if could spend
		bool couldBought = Economy.i.SpendCheck(buyable.Cost);
		//Waiting to see if could place
		bool couldPlace = false;
		//If able to buy
		if(couldBought)
		{
			//And able to place
			couldPlace = Player.i.PlaceStructure(structure); if(couldPlace)
			{
				///Then spend the money
				Economy.i.Spend(buyable.Cost);
				Player.i.HideBuildPanel();
			}
		}
		if(!couldBought || !couldPlace)
		{
			print("Not enough money to buy " + data.DisplayName);
		}
	}

	public void Sell()
	{
		Node hoverNode = Player.i.HoverNode;
		Buyable buyableNode = null;
		int sellLayer = -1;
		bool couldSell = false;
		for (int i = hoverNode.occupations.Length - 1; i >= 1 ; i--)
		{
			if(hoverNode.HaveOccupation(i))
			{
				sellLayer = i;
				buyableNode = hoverNode.occupations[i].obj.GetComponent<Buyable>();
				if(buyableNode != null) {couldSell = true; break;}
			}
		}
		if(couldSell)
		{
			Economy.i.Earn(buyableNode.sellAmount);
			BuilderManager.DemolishAtNode(hoverNode, sellLayer);
			Player.i.HideBuildPanel();
		}
		else
		{
			if(sellLayer == -1) {Debug.LogWarning("Nothing to sell"); return;}
			else {Debug.LogWarning("Cant sell [" + hoverNode.occupations[sellLayer].obj.name + "] structure");}
		}
	}
}