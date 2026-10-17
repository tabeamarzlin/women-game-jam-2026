using System;
using UnityEngine;
using UnityEngine.Audio;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance { get; private set; }

    [SerializeField]
    private int bpm;
    [SerializeField]
    private AudioSource song;

    private float beatLength;
    private float lastBeat;
    private float songPosition;

    public static event Action OnBeat;

    private void Start()
    {
        beatLength = 60 / bpm;
        song.Play();
    }

    private void Update()
    {
        songPosition = (float)song.timeSamples / song.clip.frequency;

        if (songPosition > lastBeat + beatLength)
        {
            OnBeat?.Invoke();
            lastBeat += beatLength;
        }
    }
}
