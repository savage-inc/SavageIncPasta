using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayBackgroundMusic : MonoBehaviour
{
    public int Track;

    private SoundManager _sound;

    private void Awake()
    {
        _sound = FindObjectOfType<SoundManager>();
    }

    // Use this for initialization
    void Start()
    {
        _sound.PlayBackgroundMusic(_sound.BackgroundTracks[Track]);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
