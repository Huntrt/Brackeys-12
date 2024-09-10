using System.Collections;
using System.Reflection;
using UnityEngine;
using System;

namespace Settings
{
public class KeyManager : MonoBehaviour
{
	[Tooltip("Will replace key label text when binding it")]
	[SerializeField] string waitingMessage;
	public bool areBinding;
	[SerializeField] KeyBinder selectedBinder;
	public Action RefreshBinderLabel;
	
	#region Set this class to singleton
	static KeyManager _i; public static KeyManager i 
	{
		get
		{
			//If this class is not static
			if(_i == null)
			{
				//Find object with this class to make it static
				_i = GameObject.FindObjectOfType<KeyManager>();
			}
			return _i;
		}
	}
	#endregion
	
	public void BeginBind(KeyBinder binder)
	{
		//Stop if currently binding
		if(areBinding) return;
		//Get the binder given
		selectedBinder = binder;
		//Begining key binding
		areBinding = true; StartCoroutine("Binding");
	}

	public FieldInfo GetActionField(string actionName)
	{
		//? Relied on reflection to find keycode variable of given action
		return SessionOperator.i.config.GetType().GetField(actionName);
	}
	
	IEnumerator Binding()
	{
		//If currently binding
		while(areBinding)
		{
			//Display the binder key label to waiting message
			selectedBinder.DisplayKeyLabel(waitingMessage);
            //! Go though ALL the key to check if there is currently any input (PERFORMANCE HEAVY)
			foreach(KeyCode pressedKey in System.Enum.GetValues(typeof(KeyCode)))
			{
				//If an key has been press
				if(Input.GetKey(pressedKey))
				{
					//? Box an new instance of config to edit
					object boxedConfig = (object)SessionOperator.i.config;
					//Set keycode of action currently binded to key got press
					GetActionField(selectedBinder.BindedAction).SetValue(boxedConfig, pressedKey);
					//Unbox the edited config to overwrite on session
					SessionOperator.i.OverwriteConfig((Config)boxedConfig);
					//Refresh current label's key label
					selectedBinder.RefreshKeyLabel();
					//? Delay to prevent re-bind after binding left mouse
					if(pressedKey == KeyCode.Mouse0) yield return new WaitForSeconds(0.3f);
					//Stop assigning
					areBinding = false;
				}
			}
			//Clear the current binder if no longer binding
			if(!areBinding) {selectedBinder = null;}
			yield return null;
		}
	}
}
}