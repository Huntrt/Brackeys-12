using UnityEngine;
using System;

namespace Settings 
{
public class DisplayManager : MonoBehaviour
{
	public Action RefreshControlGUI;

	void Awake()
	{
		//Haven't choose any resolution
		SessionOperator.i.config.resolution = -1;
	}
}
}