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

	public void Buy(GameObject structure, Structure data)
	{
		Buyable buyable = structure.GetComponent<Buyable>();
		bool haveBought = Economy.i.Spend(buyable.Cost);
		if(haveBought)
		{
			Player.i.PlaceStructure(structure);
		}
		else
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
		}
		else
		{
			if(sellLayer == -1) {Debug.LogWarning("Nothing to sell"); return;}
			else {Debug.LogWarning("Cant sell [" + hoverNode.occupations[sellLayer].obj.name + "] structure");}
		}
	}
}