using System.Collections.Generic;
using UnityEngine;

public class TowerInfoManager : MonoBehaviour
{
	#region Set this class to singleton
	static TowerInfoManager _i; public static TowerInfoManager i
	{
		get
		{
			if(_i==null)
			{
				_i = GameObject.FindObjectOfType<TowerInfoManager>();
			}
			return _i;
		}
	}
	#endregion

    [SerializeField] Transform towerStatsPanel, towerStatsLayout;
	[SerializeField] GameObject towerStatsUITemplate;
	[SerializeField] List<GameObject> towerStatsUIs = new List<GameObject>();

	public void UpdateInfo(List<TowerInfoController.Info> infos)
	{
		foreach (GameObject towerStatsUI in towerStatsUIs) {Destroy(towerStatsUI);}
		towerStatsUIs.Clear();
		foreach (TowerInfoController.Info info in infos)
		{
			GameObject createdUI = Instantiate(towerStatsUITemplate, towerStatsLayout.transform);
			createdUI.GetComponent<TMPro.TextMeshProUGUI>().text = info.statsName + ": <b><size=12>" + info.statValue + "</size></b>";
			createdUI.name = "Info stats - " + info.statsName;
			towerStatsUIs.Add(createdUI);
		}
	}

	public void ShowInfo(bool show)
	{
		towerStatsPanel.gameObject.SetActive(show);
	}
}