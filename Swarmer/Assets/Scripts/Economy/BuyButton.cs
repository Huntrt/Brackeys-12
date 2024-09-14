using UnityEngine;

public class BuyButton : MonoBehaviour
{
	public GameObject buyingStructure;
	public Structure structureData;
	public Buyable structureBuyable;
	[SerializeField] UnityEngine.UI.Image icon;

	void OnValidate()
	{
		structureData = buyingStructure.GetComponent<Structure>();
		structureBuyable = buyingStructure.GetComponent<Buyable>();
		SpriteRenderer structureSr = buyingStructure.GetComponent<SpriteRenderer>();
		icon.sprite = structureSr.sprite;
		icon.color = structureSr.color;
		gameObject.name = "Buy - " + structureData.DisplayName;
	}

	public void RequestBuy()
	{
		BuyManager.i.Buy(buyingStructure, structureData, structureBuyable);
	}
}