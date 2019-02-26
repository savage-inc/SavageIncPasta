using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	public static SoundManager instance;

	public AudioClip[] BackgroundTracks;
	public AudioClip[] AmbienceClips;
	public AudioClip[] SfxClips;
	public AudioClip[] LongAmbienceTracks;

	public AudioSource BackgroundMusic;
	private AudioSource Sfx;
	public AudioSource Ambience;
	public AudioSource LongAmbience;


	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(this.gameObject);
		}
		DontDestroyOnLoad(this.gameObject);
	}

	public void PlayBackgroundMusic(AudioClip clip)
	{
		BackgroundMusic.clip = clip;
		BackgroundMusic.loop = true;
		BackgroundMusic.Play();
		Debug.Log("backgroundMusicPlaying");
	}

	public void PlayLongAmbience(AudioClip clip)
	{
		LongAmbience.loop = true;
		StartCoroutine(_fadeClip(clip));
		Debug.Log("Long ambience playing");
	}

	private IEnumerator _fadeClip(AudioClip clip)
	{
		Debug.Log("1st loop");
		if (LongAmbience.isPlaying)
		{
			for (float f = 1f; f >= 0; f -= 0.01f)
			{
				LongAmbience.volume = f;
				yield return new WaitForSeconds(.1f);
			}
			Debug.Log("Ambience Silenced");
		}

		LongAmbience.clip = clip;
		LongAmbience.Play();
		Debug.Log("2nd loop");
		for (float i = 0f; i <= 1f; i += 0.01f)
		{
			LongAmbience.volume = i;
			Debug.Log("ambience volume rising");
			yield return new WaitForSeconds(.1f);
		}

		Debug.Log("test Long Ambience Playing");
	}


    public void PlayAmbience()
    {
        Ambience.clip = AmbienceClips[Random.Range(0, AmbienceClips.Length)];
        Ambience.Play();
        Debug.Log("ambience Playing");
        StartCoroutine("_AmbienceDelay");
    }

    private IEnumerator _AmbienceDelay()
    {
        yield return new WaitForSeconds(Random.Range(Ambience.clip.length, 50));
        print(Time.time);
        PlayAmbience();
    }


	public void PlaySFX(AudioClip clip, AudioSource Source)
    {
		Sfx = Source;
		Sfx.clip = clip;
        Sfx.Play();
	}


    private void Start()
    {
		//PlayAmbience();
		//PlayBackgroundMusic(BackgroundTracks[0]);
		//PlayLongAmbience(LongAmbienceTracks[0]);
		print("leaves rustling");
		PlaySFX(SfxClips[1], GetComponent<AudioSource>());
	}
	private void Update()
	{

    }
}