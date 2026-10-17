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
    private float songPosition;

    public float LastBeat;
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
        beatLength = 60 / bpm;
        song.Play();
    }

    private void Update()
    {
        songPosition = (float)song.timeSamples / song.clip.frequency;

        if (songPosition > LastBeat + beatLength)
        {
            OnBeat?.Invoke();
            LastBeat += beatLength;
            Debug.Log("Beat!");
        }
    }
}
