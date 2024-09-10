using UnityEngine.UI;
using UnityEngine;
using TMPro;

namespace Settings 
{
public class AudioControl : MonoBehaviour
{
	public TextMeshProUGUI display;
	public Slider slider;
	public enum VolumeType {master, sound, music} [SerializeField] VolumeType type;
	AudioManager manager;

	void OnEnable() 
	{
		//Get the audio manager
		manager = SessionOperator.i.audios;
		//Update display to show current manager volume
		UpdateDisplay();
		//Slide the volume when slider value changed
		slider.onValueChanged.AddListener(SlideVolume);
		//Update display when manager need to refresh
		manager.RefreshControlGUI += UpdateDisplay;
	}

	public void SlideVolume(float value)
	{
		//Don't modify if there no manager
		if(manager == null) return;
		//Time 100 slide value then round it off
		value *= 100; value = Mathf.Round(value);
		//Adjust this slider type volume with value
		manager.AdjustVolume(value, type);
		//Update both display and slider
		UpdateDisplay(); UpdateSlider();
	}

	void UpdateDisplay()
	{
		//Don't update if there no manager
		if(manager == null) return;
		//@ Set the display text as manager volume then update slider
		if(type == VolumeType.master) {display.text = "Master: "+SessionOperator.i.config.masterVolume;}
		if(type == VolumeType.sound) {display.text = "Sound: "+SessionOperator.i.config.soundVolume;}
		if(type == VolumeType.music) {display.text = "Music: "+SessionOperator.i.config.musicVolume;}
		UpdateSlider();
	}

	void UpdateSlider()
	{
		//Don't update if there no manager
		if(manager == null) return;
		//@ Update slider value to be manager volume
		if(type == VolumeType.master) {slider.value = (float)SessionOperator.i.config.masterVolume/100;}
		if(type == VolumeType.sound) {slider.value = (float)SessionOperator.i.config.soundVolume/100;}
		if(type == VolumeType.music) {slider.value = (float)SessionOperator.i.config.musicVolume/100;}
	}

	void OnDisable()
	{
		slider.onValueChanged.RemoveListener(SlideVolume);
		manager.RefreshControlGUI -= UpdateDisplay;
	}
}
}