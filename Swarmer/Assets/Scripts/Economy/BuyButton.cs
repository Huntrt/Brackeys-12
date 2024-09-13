using UnityEngine;

public class BuyButton : MonoBehaviour
{
    [SerializeField] GameObject buyingStructure;
	[SerializeField] Structure structureData;

	void OnValidate()
	{
		structureData = buyingStructure.GetComponent<Structure>(); 
	}

	public void RequestBuy()
	{
		BuyManager.i.Buy(buyingStructure, structureData);
	}
}
