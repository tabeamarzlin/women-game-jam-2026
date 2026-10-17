using UnityEngine;
using UnityEngine.UI;

public class SocialScoreView : MonoBehaviour
{
    [SerializeField]
    private Image socialBar;

    private void Start()
    {
        SocialScoreManager.Instance.OnSocialScoreChanged += OnSocialScoreChanged;
    }

    private void OnSocialScoreChanged()
    {
        socialBar.fillAmount = SocialScoreManager.Instance.SocialScore;
    }
}
