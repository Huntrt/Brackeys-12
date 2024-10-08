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

	[SerializeField] AudioClip sellAudio, buyAudio;
	[SerializeField] GameObject sellEffect;

	void Update()
	{
		if(Input.GetKeyDown(SessionOperator.i.config.SellTower))
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
				SessionOperator.i.audios.soundSource.PlayOneShot(buyAudio);
				///Then spend the money
				Economy.i.Spend(buyable.Cost);
				Player.i.HideHoverPanel();
			}
		}
		if(!couldBought)
		{
			Popup.i.Pop("No money to buy " + data.name);
			SessionOperator.i.audios.soundSource.PlayOneShot(Player.i.failAudio);
		}
	}

	public void Sell()
	{
		if(GameLoop.i.raidPhase) return;
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
			SessionOperator.i.audios.soundSource.PlayOneShot(sellAudio);
			Instantiate(sellEffect, hoverNode.pos, sellEffect.transform.rotation);
			Economy.i.Earn(buyableNode.sellAmount);
			BuilderManager.DemolishAtNode(hoverNode, sellLayer);
			Player.i.HideHoverPanel();
		}
		else
		{
			if(sellLayer == -1) {Debug.LogWarning("Nothing to sell"); return;}
			else {Debug.LogWarning("Cant sell [" + hoverNode.occupations[sellLayer].obj.name + "] structure");}
		}
	}
}