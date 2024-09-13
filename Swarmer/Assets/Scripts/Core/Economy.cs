using UnityEngine;

public class Economy : MonoBehaviour
{
    [SerializeField] int money;
	[SerializeField] TMPro.TextMeshProUGUI moneyCounterTxt;

	public void Earn(int amount)
	{
		money += amount;
		moneyCounterTxt.text = "$" + money;
	}
	
	public bool Spend(int amount)
	{
		if(amount > money)
		{
			print("No enough money");
			return false;
		}
		money -= amount;
		moneyCounterTxt.text = "$" + money;
		return true;
	}
}
