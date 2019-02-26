using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSound : MonoBehaviour {
	public SoundManager Sound;
	public int clip;
	// Use this for initialization
	void Start () {
		print("dog barking");
		Sound.PlaySFX(Sound.SfxClips[clip], GetComponent<AudioSource>());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
