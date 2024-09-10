using UnityEngine.UI;
using UnityEngine;

namespace Settings
{
public class KeyBinder : MonoBehaviour
{
	[SerializeField] Config.Actions bindTo; 
	public string BindedAction { get => bindTo.ToString();}
	[SerializeField] Button bindingButton;
	[SerializeField] TMPro.TMP_Text keyLabel;
	[Header("Auto Setup (Optional)")]
	[Tooltip("Rename this object to action it will be bind to")]
	[SerializeField] GameObject autoNaming;
	[Tooltip("Make this label text match the action it will bind to")]
	[SerializeField] TMPro.TMP_Text actionLabel;
	KeyManager manager;

	void OnValidate() 
	{
		if(autoNaming != null) gameObject.name = BindedAction;
		if(actionLabel != null) actionLabel.text = BindedAction;
	}

	void OnEnable()
	{
		//Get the key manager
		manager = KeyManager.i;
		//Upon clicking bind button it will send this to be bind in manager
		bindingButton.onClick.AddListener(delegate {manager.BeginBind(this);});
		//Refresh key label when manager need to
		manager.RefreshBinderLabel += RefreshKeyLabel;
		RefreshKeyLabel();
	}

	public void RefreshKeyLabel()
	{
		//Display the keycode of this binder in manager
		keyLabel.text = manager.GetActionField(BindedAction).GetValue(SessionOperator.i.config).ToString();
	}

	//Display given text to key label
	public void DisplayKeyLabel(string text) {keyLabel.text = text;}

	void OnDisable()
	{
		bindingButton.onClick.RemoveListener(delegate {manager.BeginBind(this);});
		manager.RefreshBinderLabel -= RefreshKeyLabel;
	}
}
}