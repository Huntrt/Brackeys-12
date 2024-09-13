using UnityEngine;

public class Buyable : MonoBehaviour
{
    [SerializeField] int cost; public int Cost {get => cost;}
	[SerializeField] int sellPenalty;
	public int sellAmount{get => cost/sellPenalty;}

	void OnValidate()
	{
		
	}
}
