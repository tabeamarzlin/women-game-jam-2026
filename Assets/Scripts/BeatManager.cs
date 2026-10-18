using System;
using UnityEngine;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance { get; private set; }

    [SerializeField] private int bpm;
    [SerializeField] private AudioSource startSong;
    [SerializeField] private AudioSource loopSong;
    [SerializeField] private float offset;
    public int BeatIndex { get; private set; } = -1;

    private float songPosition;
    private double loopStartTime;
    private bool loopStarted;

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
    }

    private void Start()
    {
        if (bpm <= 0 || startSong == null || loopSong == null ||
            startSong.clip == null || loopSong.clip == null)
        {
            Debug.LogError("BeatManager: Check bpm and AudioSources");
            enabled = false;
            return;
        }

        BeatLength = 60f / bpm;

        startSong.loop = false;
        loopSong.loop = true;

        startSong.Stop();
        loopSong.Stop();

        // Schedule the loop for the exact end of the intro.
        loopStartTime = AudioSettings.dspTime + (double)startSong.clip.samples /
                        startSong.clip.frequency;

        startSong.PlayScheduled(AudioSettings.dspTime);
        loopSong.PlayScheduled(loopStartTime);

        loopStarted = false;
        BeatIndex = -1;
        LastBeat = 0f;
    }

    private void Update()
    {
        double dspTime = AudioSettings.dspTime;

        if (!loopStarted && dspTime >= loopStartTime)
        {
            loopStarted = true;
            BeatIndex = -1;
            LastBeat = 0f;
        }

        AudioSource currentSong = loopStarted ? loopSong : startSong;

        if (currentSong == null || currentSong.clip == null)
            return;

        songPosition =
            (float)currentSong.timeSamples / currentSong.clip.frequency
            - offset;

        while (songPosition >= LastBeat + BeatLength)
        {
            LastBeat += BeatLength;
            BeatIndex++;
            OnBeat?.Invoke();
        }
    }
}
