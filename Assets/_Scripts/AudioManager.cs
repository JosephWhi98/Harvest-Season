using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public AudioMixer mixer;

    public AudioMixerSnapshot open;
    public AudioMixerSnapshot closed;

    public void Start()
    {
        SnapAudioClose(0f); 
        SnapAudioOpen(1f);
    } 

    public void SnapAudioOpen(float time = 1f)
    {
        open.TransitionTo(time);
    }

    public void SnapAudioClose(float time = 1f)
    {
        closed.TransitionTo(time);
    }
}
