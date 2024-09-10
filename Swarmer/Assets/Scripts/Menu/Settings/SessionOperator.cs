using UnityEngine;
using Settings;

public class SessionOperator : MonoBehaviour
{
	public static SessionOperator i;
	public Config config;
	public AudioManager audios;
	public DisplayManager displays;
	public KeyManager keys;
	
	void Awake()
	{
		//Only set to "don't destroy on load" if haven't then destroy any duplicate
		if(i == null) {i = this; DontDestroyOnLoad(this);} else {Destroy(gameObject);}
	}

	public void OverwriteConfig(Config overwriter)
	{
		//Overwrite session config with given config
		config = overwriter;
		//Refresh all audio key display and key binder after apply
		audios.RefreshControlGUI?.Invoke();
		displays.RefreshControlGUI?.Invoke();
		keys.RefreshBinderLabel?.Invoke();
	}
}