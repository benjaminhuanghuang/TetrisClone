using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour 
{
	public bool musicEnabled = false;
	[Range(0, 1)]
	public float musicVolume = 1.0f;

	public bool fxEnabled = true;
	[Range(0, 1)]
	public float fxVolume = 1.0f;

	public AudioClip clearRowSound;
	public AudioClip moveSound;
	public AudioClip dropSound;
	public AudioClip gameOverSound;
	public AudioClip errorSound;
	
	// background music clips
	public AudioClip[] musicClips;
	private AudioClip randomMusicClip;
	// Play background music
	public AudioSource musicSource;

	public AudioClip[] vocalClips;
	
	// Use this for initialization
	void Start () {
		randomMusicClip = GetRandomClip(musicClips);
		PlayBackgroundMusic(randomMusicClip);
	}
	
	public AudioClip GetRandomClip(AudioClip[] clips)
	{
		return clips[Random.Range(0, clips.Length)];
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void PlayBackgroundMusic(AudioClip musicClip)
	{
		if(!musicEnabled || !musicClip || !musicSource)
		{
			return;
		}

		musicSource.Stop();
		musicSource.clip = musicClip;
		musicSource.volume = musicVolume;
		musicSource.loop = true;
		musicSource.Play();
	}

	public void UpdateMusic()
	{
		if(musicSource.isPlaying != musicEnabled)
		{
			if(musicEnabled)
			{
				PlayBackgroundMusic(GetRandomClip(musicClips));
			}
			else
			{
				musicSource.Stop();
			}
		}
	}

	public void ToggleMusic()
	{
		musicEnabled = !musicEnabled;
		UpdateMusic();
	}

	public void ToggleFX()
	{
		fxEnabled = !fxEnabled;
	}
}
