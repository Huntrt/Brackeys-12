using UnityEngine;
using System;

namespace Settings 
{
public class DisplayManager : MonoBehaviour
{
	public Action RefreshControlGUI;

	void OnEnable()
	{
		SessionOperator.i.config.resolution = -1;
	}
}
}