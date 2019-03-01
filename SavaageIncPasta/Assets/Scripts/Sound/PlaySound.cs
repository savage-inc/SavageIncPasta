using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour {
	public int Clip;

    private SoundManager _sound;

    private void Awake()
    {
        _sound = FindObjectOfType<SoundManager>();    
    }

    // Use this for initialization
    void Start () {
        _sound.PlaySFX(_sound.SfxClips[Clip], GetComponent<AudioSource>());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
