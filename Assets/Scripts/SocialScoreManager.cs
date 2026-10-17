using System;
using UnityEngine;

public class SocialScoreManager : MonoBehaviour
{
    public float SocialScore = 1;

    public event Action OnSocialScoreChanged;

    public event Action OnSocialScoreDepleted;

    public static SocialScoreManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ChangeSocialScore(float changeAmount)
    {
        SocialScore += changeAmount;
        SocialScore = Mathf.Clamp(SocialScore, 0, 1);

        OnSocialScoreChanged?.Invoke();

        if (SocialScore == 0)
        {
            OnSocialScoreDepleted?.Invoke();
        }
    }
}
