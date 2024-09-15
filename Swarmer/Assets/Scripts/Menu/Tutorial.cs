using UnityEngine;

public class Tutorial : MonoBehaviour
{
	public GameObject[] tutorials;
	[SerializeField] GameObject tutorialPanel;
	[SerializeField] int curTutorial;
	[SerializeField] TMPro.TextMeshProUGUI counter;

	void OnEnable()
	{
		if(!SessionOperator.i.completeTutorial)
		{
			tutorialPanel.SetActive(true);
			SwitchTutorial();
		}
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.LeftArrow))
		{
			curTutorial--;
			if(curTutorial < 0)
			{
				curTutorial = tutorials.Length;
			}
			SwitchTutorial();
		}
		if(Input.GetKeyDown(KeyCode.RightArrow))
		{
			curTutorial++;
			if(curTutorial > tutorials.Length-1)
			{
				curTutorial = 0;
			}
			SwitchTutorial();
			
		}
		if(Input.GetKeyDown(KeyCode.Space))
		{
			tutorialPanel.SetActive(false);
			SessionOperator.i.completeTutorial = true;
			this.enabled = false;
		}
	}

	public void SwitchTutorial()
	{
		counter.text = "Page " + curTutorial + "/" + tutorials.Length;
		for (int i = 0; i < tutorials.Length; i++)
		{
			tutorials[i].SetActive(false);
			if(curTutorial == i) {tutorials[i].SetActive(true);}
		}
	}
}
