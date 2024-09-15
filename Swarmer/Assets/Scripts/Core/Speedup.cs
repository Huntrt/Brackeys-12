using UnityEngine;

public class Speedup : MonoBehaviour
{
	[SerializeField] TMPro.TextMeshProUGUI speedCounter;
	[SerializeField] int speedStage = 1;

	void Update()
	{
		if(Input.GetKeyDown(SessionOperator.i.config.Speedup))
		{
			SpeedToogle();
		}
	}


    public void SpeedToogle()
	{
		speedStage++;
		if(speedStage == 1) {Time.timeScale = 1; speedCounter.text = ">";}
		if(speedStage == 2) {Time.timeScale = 2; speedCounter.text = ">>";}
		if(speedStage == 3) {Time.timeScale = 4; speedCounter.text = ">>>>"; speedStage = 0;}
	}
}
