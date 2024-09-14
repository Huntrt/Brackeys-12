using UnityEngine;

public class StructureInfo : MonoBehaviour
{
    
	#region Set this class to singleton
	static StructureInfo _i; public static StructureInfo i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindObjectOfType<StructureInfo>();
			}
			return _i;
		}
	}
	#endregion

	[SerializeField] GameObject infoPanel;
	[SerializeField] TMPro.TextMeshProUGUI infoTxt;

	public void ShowInfo(Structure structure)
	{
		//Try to get tower from given structure
		Tower tower = structure.GetComponent<Tower>();
		//Show the structure name
		string title = "<b>" + structure.DisplayName;
		//Show the structure level if it an tower
		if(tower != null) {title += " <color=#4287f5>LV." + tower.upgrader.Level + "</color></b>";}
		//Show the structure description
		infoTxt.text =  title + "\n<size=7>" + structure.Description + "</size>";
		infoPanel.SetActive(true);
	}

	public void CloseInfo()
	{
		infoPanel.SetActive(false);
	}
}