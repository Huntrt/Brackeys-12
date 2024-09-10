using UnityEngine;

namespace Settings
{
[System.Serializable] public struct Config
{
	//? Binder
	/// Add the action you want below
	//! keycode and enum NEED TO BE MATCH
	public enum Actions {Up, Down, Left, Right}
	public KeyCode Up, Down, Left, Right;
	
	//? Audio
	[Range(0,100)] public float masterVolume, soundVolume, musicVolume;

	//? Display
	public bool fullScreen, vSync;
	public int resolution, frameCap;
}
}