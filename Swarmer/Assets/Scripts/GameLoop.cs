using UnityEngine;

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
	public bool combatPhase;
	public int killReq;
	[SerializeField] int killCount;
	public delegate void OnLevelBegin(int level); public static OnLevelBegin onLevelBegin;
	public delegate void OnLevelComplete(int level); public static OnLevelComplete onLevelComplete;

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.T))
		{
			BeginLevel();
		}
		//When in combat
		if(combatPhase)
		{
			//Complete level when get enough kill count
			if(killCount >= killReq)
			{
				CompleteLevel();
			}
		}
	}

	public void KillCouting()
	{
		killCount++;
	}

	public void BeginLevel()
	{
		if(combatPhase) return;
		combatPhase = true;
		level++;
		killReq = level * 10;
		onLevelBegin?.Invoke(level);
	}

	public void CompleteLevel()
	{
		onLevelComplete?.Invoke(level);
		combatPhase = false;
		killCount -= killCount;
	}
}
