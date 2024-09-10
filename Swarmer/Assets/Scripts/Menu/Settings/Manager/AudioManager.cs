using UnityEngine;
using System;

namespace Settings 
{
public class AudioManager : MonoBehaviour
{
	public AudioSource soundSource, musicSource;
	public Action RefreshControlGUI;

	void Start()
	{	
		//@ Updated the master, sound and music volume value upon scene open
		AdjustVolume(SessionOperator.i.config.masterVolume, AudioControl.VolumeType.master); 
		AdjustVolume(SessionOperator.i.config.soundVolume, AudioControl.VolumeType.sound);
		AdjustVolume(SessionOperator.i.config.musicVolume, AudioControl.VolumeType.music);
	}

	public void AdjustVolume(float adjust, AudioControl.VolumeType type)
	{
		SessionOperator s = SessionOperator.i;
		//If this control are master type
		if(type == AudioControl.VolumeType.master) 
		{
			s.config.masterVolume = adjust;
			AudioListener.volume = adjust/100;
		}
		//If this control are sound type
		else if(type == AudioControl.VolumeType.sound) 
		{
			s.config.soundVolume = adjust;
			soundSource.volume = adjust/100;
		}
		//If this control are music type
		else if(type == AudioControl.VolumeType.music) 
		{
			s.config.musicVolume = adjust;
			musicSource.volume = adjust/100;
		}
	}
}
}