using UnityEngine.UI;
using UnityEngine;

public class Heart : MonoBehaviour
{
	public GameObject obj;
	Animation heartAnim;
	public int curHeart, maxHeart;
	[SerializeField] TMPro.TextMeshProUGUI heartCounterTxt;
	[SerializeField] Image heartBar;
	[SerializeField] GameObject gameOverPanel;

	void OnEnable()
	{
		curHeart = maxHeart;
		Map.i.onMapCreated += CreateHeart;
	}

	void CreateHeart(Vector2Int chunk)
	{
		//Create the heart in the center node of chunk 0
		if(chunk == Vector2Int.zero)
		{
			obj = BuilderManager.BuildAtNode(Map.i.FindNode(Vector2Int.zero), obj);
			heartAnim = obj.GetComponent<Animation>();
			heartCounterTxt.text = "HP " + curHeart;
			heartBar.fillAmount = maxHeart/curHeart;
			Map.i.onMapCreated -= CreateHeart;
		}
	}
	
	public void DamageHeart(int taken)
	{
		curHeart -= taken;
		curHeart = Mathf.Clamp(curHeart, 0, int.MaxValue);
		heartAnim.Stop();
		heartAnim.Play();
		heartCounterTxt.text = "HP " + curHeart;
		heartBar.fillAmount = (float)((float)curHeart/(float)maxHeart);
		if(curHeart <= 0)
		{
			gameOverPanel.SetActive(true);
		}
	}
}
