using UnityEngine;

public class Economy : MonoBehaviour
{
	#region Set this class to singleton
	static Economy _i; public static Economy i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindObjectOfType<Economy>();
			}
			return _i;
		}
	}
	#endregion

    [SerializeField] int money;
	[SerializeField] TMPro.TextMeshProUGUI moneyCounterTxt;

	public void Earn(int amount)
	{
		money += amount;
		moneyCounterTxt.text = "$" + money;
	}

	public bool SpendCheck(int amount)
	{
		if(money >= amount)
		{
			money -= amount;
			return true;
		}
		Debug.Log("No money");
		return false;
	}
	
	public void Spend(int amount)
	{
		money -= amount;
		moneyCounterTxt.text = "$" + money;
	}
}
