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
    [SerializeField]
    private float offset;

    private float songPosition;

    public float LastBeat;
    public float BeatLength;
    public event Action OnBeat;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        BeatLength = 60 / bpm;
        song.Play();
    }

    private void Update()
    {
        songPosition = (float)song.timeSamples / song.clip.frequency - offset;

        if (songPosition > LastBeat + BeatLength)
        {
            OnBeat?.Invoke();
            LastBeat += BeatLength;
        }
    }
}
