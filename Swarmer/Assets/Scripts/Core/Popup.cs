using UnityEngine;

public class Popup : MonoBehaviour
{
	#region Set this class to singleton
	static Popup _i; public static Popup i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindObjectOfType<Popup>();
			}
			return _i;
		}
	}
	#endregion

	[SerializeField] GameObject popupObj;
	[SerializeField] TMPro.TextMeshProUGUI popupText;
	[SerializeField] float popupDuration; float popupTimer;

	public void Pop(string info)
	{
		popupTimer = popupDuration;
		popupText.text = info;
		popupObj.SetActive(true);
	}

	void Update()
	{
		if(popupTimer <= 0)
		{
			popupObj.SetActive(false);
		}
		else
		{
			popupTimer -=Time.deltaTime;
		}
	}
}