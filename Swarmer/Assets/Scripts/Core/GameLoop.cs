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
	public float calmDuration; [SerializeField] float calmTimer;
	public int killReq; [SerializeField] int killCount;
	public delegate void OnLevelBegin(int level); public static OnLevelBegin onLevelBegin;
	public delegate void OnLevelComplete(int level); public static OnLevelComplete onLevelComplete;
	[Header("UI")]
	public TextMeshProUGUI levelCounterTxt;
	public GameObject builderUIPanel;
	public GameObject raidUIPanel;
	public TextMeshProUGUI killProgressTxt;
	public TextMeshProUGUI calmTimerTxt;
	[SerializeField] GameObject raidEffect, calmEffect;
	[SerializeField] UnityEngine.UI.Image calmTimerImg;
	[SerializeField] AudioClip raidAu, calmAu;

	void OnEnable()
	{
		calmTimer = calmDuration;
	}

	void Update()
	{
		levelCounterTxt.text = "LEVEL " + level;
		//Skip calm when use hot key
		if(Input.GetKeyDown(SessionOperator.i.config.SkipCalm))
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
				calmTimer = calmDuration;
				calmTimerImg.color = Color.white;
				calmTimerTxt.color = Color.white;
			}
		}
		else
		{
			//Decrease and display the calm timer
			calmTimer -= Time.deltaTime;
			calmTimerTxt.text = Mathf.RoundToInt(calmTimer) + "<size=18>." + (System.Math.Round(calmTimer - Mathf.Floor(calmTimer), 1)*10) + "</size>";
			//Play timer warning animation when under 10s
			if(calmTimer < 10) {calmTimerImg.color = Color.red; calmTimerTxt.color = Color.red;}
			//Start level when out of timer
			if(calmTimer <= 0)
			{
				BeginLevel();
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
		SessionOperator.i.audios.soundSource.PlayOneShot(raidAu);
		raidEffect.SetActive(true);
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
		SessionOperator.i.audios.soundSource.PlayOneShot(calmAu);
		calmEffect.SetActive(true);
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
