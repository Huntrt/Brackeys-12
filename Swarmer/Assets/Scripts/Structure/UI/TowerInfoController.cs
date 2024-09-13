using System.Collections.Generic;
using UnityEngine;

public class TowerInfoController : MonoBehaviour
{
    [System.Serializable] public class Info
	{
		public string statsName;
		public float statValue;

		public Info(string statsName, float statValue)
		{
			this.statsName = statsName;
			this.statValue = statValue;
		}
	}

	public List<Info> infos = new List<Info>();

	public void Inform(string informStat, float informValue)
	{
		foreach (Info info in infos)
		{
			if(informStat == info.statsName)
			{
				info.statValue = informValue;
				return;
			}
		}
		Info newInfo = new Info(informStat, informValue);
		infos.Add(newInfo);
	}
}