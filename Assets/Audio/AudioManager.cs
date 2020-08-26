﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum SoundState
{
    Off,
    On
}

public enum MusicState
{
    Off,
    On
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public SoundState CurrentSoundState
    {
        get => currentSoundState;
        private set
        {
            currentSoundState = value;
            var volume = GetIntFromSoundState(currentSoundState);
            mainGroup.audioMixer.SetFloat("SoundVolume", volume);
            PlayerPrefs.SetInt(pp_soundState, volume);
            SoundStateChanged?.Invoke(currentSoundState);
        }
    }

    public MusicState CurrentMusicState
    {
        get => currentMusicState;
        private set
        {
            currentMusicState = value;
            var volume = GetIntFromMusicState(currentMusicState);
            mainGroup.audioMixer.SetFloat("MusicVolume", volume);
            PlayerPrefs.SetInt(pp_musicState, volume);
            MusicStateChanged?.Invoke(currentMusicState);
        }
    }

    public event System.Action<SoundState> SoundStateChanged;
    public event System.Action<MusicState> MusicStateChanged;

    public void SwitchOnSound()
    {
        CurrentSoundState = SoundState.On;
    }

    public void SwitchOffSound()
    {
        CurrentSoundState = SoundState.Off;
    }

    public void SwitchSoundToState(bool state)
    {
        CurrentSoundState = state ? SoundState.On : SoundState.Off;
    }

    public void SwitchOnMusic()
    {
        CurrentMusicState = MusicState.On;
    }

    public void SwitchOffMusic()
    {
        CurrentMusicState = MusicState.Off;
    }

    public void SwitchMusicToState(bool state)
    {
        CurrentMusicState = state ? MusicState.On : MusicState.Off;
    }

    private const string pp_soundState = "SoundState";
    private const string pp_musicState = "MusicState";
    [SerializeField] private AudioMixerGroup mainGroup;
    private SoundState currentSoundState;
    private MusicState currentMusicState;

    private void Awake()
    {
        Instance = this;

        CurrentSoundState = PlayerPrefs.HasKey(pp_soundState) ? GetSoundStateFromInt(PlayerPrefs.GetInt(pp_soundState)) : SoundState.On;
    }

    private int GetIntFromSoundState(SoundState state)
    {
        return state == SoundState.On ? 0 : -80;
    }

    private SoundState GetSoundStateFromInt(int value)
    {
        return value == 0 ? SoundState.On : SoundState.Off;
    }

    private int GetIntFromMusicState(MusicState state)
    {
        return state == MusicState.On ? 0 : -80;
    }

    private MusicState GetMusicStateFromInt(int value)
    {
        return value == 0 ? MusicState.On : MusicState.Off;
    }
}
