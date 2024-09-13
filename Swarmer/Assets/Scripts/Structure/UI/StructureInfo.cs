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
		infoTxt.text = "<b>" + structure.DisplayName + "</b>\n<size=7>" + structure.Description + "</size>";
		infoPanel.SetActive(true);
	}

	public void CloseInfo()
	{
		infoPanel.SetActive(false);
	}
}