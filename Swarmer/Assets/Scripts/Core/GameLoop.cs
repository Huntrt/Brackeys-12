using UnityEngine;
using TMPro;

public class GameLoop : MonoBehaviour
{
    #region Set this class to singleton
	static GameLoop _i; public static GameLoop i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindObjectOfType<GameLoop>();
			}
			return _i;
		}
	}

	#endregion

	public int level;
	public bool raidPhase;
	public int killReq;
	[SerializeField] int killCount;
	public delegate void OnLevelBegin(int level); public static OnLevelBegin onLevelBegin;
	public delegate void OnLevelComplete(int level); public static OnLevelComplete onLevelComplete;
	[Header("UI")]
	public TextMeshProUGUI levelCounterTxt;
	public GameObject builderUIPanel;
	public GameObject raidUIPanel;
	public TextMeshProUGUI killProgressTxt;

	void Update()
	{
		levelCounterTxt.text = "LEVEL " + level;
		if(Input.GetKeyDown(KeyCode.T))
		{
			BeginLevel();
		}
		//When in raid
		if(raidPhase)
		{
			//Disaply kill count as %
			killProgressTxt.text = (int)(((float)killCount / (float)killReq) * 100) + "%";
			//Complete level when get enough kill count
			if(killCount >= killReq)
			{
				CompleteLevel();
			}
		}
	}

	public void KillCouting()
	{
		if(!raidPhase) return;
		killCount++;
	}

	public void BeginLevel()
	{
		if(raidPhase) return;
		level++;
		//Start raid phase
		raidPhase = true;
		//Set kill requierment
		killReq = level * 5;
		//Switch to raider UI
		builderUIPanel.SetActive(false);
		raidUIPanel.SetActive(true);
		//Cal event
		onLevelBegin?.Invoke(level);
	}

	public void CompleteLevel()
	{
		//Call event
		onLevelComplete?.Invoke(level);
		//Switch to builder UI
		builderUIPanel.SetActive(true);
		raidUIPanel.SetActive(false);
		//No longer raid
		raidPhase = false;
		//Reset kill count
		killCount -= killCount;
	}
}
