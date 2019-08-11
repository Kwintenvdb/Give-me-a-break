using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusicController : MonoBehaviour
{
    [SerializeField] private AudioClip chillStart;
    [SerializeField] private AudioClip chillLoop;
    [SerializeField] private AudioClip tenseStart;
    [SerializeField] private AudioClip tenseLoop;
    [SerializeField] private AudioClip riotLoop;
    [SerializeField] private List<AudioSource> audioSourceArray;
    [SerializeField] private Office office;

    private int selectedAudioSourceIndex = 0;
    private OfficeState previousOfficeState;
    private BackgroundMusicState state;
    private double nextStartTime;
    private AudioClip nextClip;

    private void Start()
    {
        state = BackgroundMusicState.Intro;
        nextStartTime = AudioSettings.dspTime + 2;
        previousOfficeState = OfficeState.Chill;
        nextClip = GetNextClip();
        playNext(nextClip);
    }

    void Update()
    {
        if (AudioSettings.dspTime > nextStartTime - 1)
        {
            nextClip = GetNextClip();
            playNext(nextClip);
        }
    }

    private void playNext(AudioClip clipToPlay)
    {
        // Loads the next Clip to play and schedules when it will start
        audioSourceArray[selectedAudioSourceIndex].clip = clipToPlay;
        audioSourceArray[selectedAudioSourceIndex].PlayScheduled(nextStartTime-0.05);

        // Checks how long the Clip will last and updates the Next Start Time with a new value
        double duration = (double) clipToPlay.samples / clipToPlay.frequency;
        nextStartTime = nextStartTime + duration;

        // Switches the selectedAudioSourceIndex to use the other Audio Source next
        selectedAudioSourceIndex = 1 - selectedAudioSourceIndex;
    }

    private AudioClip GetNextClip()
    {
        if (previousOfficeState != office.State)
        {
            state = BackgroundMusicState.Intro;
        }

        previousOfficeState = office.State;

        switch (office.State)
        {
            case OfficeState.Chill:
                if (state == BackgroundMusicState.Intro)
                    nextClip = chillStart;
                else
                    nextClip = chillLoop;
                break;
            case OfficeState.Tense:
                if (state == BackgroundMusicState.Intro)
                    nextClip = tenseStart;
                else
                    nextClip = tenseLoop;
                break;
            case OfficeState.Riot:
                nextClip = riotLoop;
                break;
        }

        if (state == BackgroundMusicState.Intro)
        {
            state = BackgroundMusicState.Loop;
        }

        return nextClip;
    }
}
