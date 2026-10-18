using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject victoryScreen;
    [SerializeField]
    private GameObject defeatScreen;
    [SerializeField]
    private Door door;
    [SerializeField]
    private PlayerQTEController playerQTEController;
    [SerializeField]
    private GameObject QTEView;
    [SerializeField]
    private AudioSource victorySound;
    [SerializeField]
    private AudioSource defeatSound;

    void Start()
    {
        victoryScreen.SetActive(false);
        defeatScreen.SetActive(false);
        QTEView.SetActive(false);

        SocialScoreManager.Instance.OnSocialScoreDepleted += OnLose;
        door.OnDoorReached += OnWin;
        playerQTEController.OnQTEStarted += OnQTEStarted;
        playerQTEController.OnQTEEnded += OnQTEEnded;

        Time.timeScale = 1;
    }

    private void OnDestroy()
    {
        SocialScoreManager.Instance.OnSocialScoreDepleted -= OnLose;
        door.OnDoorReached -= OnWin;
        playerQTEController.OnQTEStarted -= OnQTEStarted;
        playerQTEController.OnQTEEnded -= OnQTEEnded;
    }

    private void OnLose()
    {
        defeatSound.Play();
        defeatScreen.SetActive(true);
        
        Time.timeScale = 0;
    }

    private void OnWin()
    {
        victorySound.Play();
        victoryScreen.SetActive(true);

        Time.timeScale = 0;
    }

    private void OnQTEStarted()
    {
        QTEView.SetActive(true);
    }

    private void OnQTEEnded()
    {
        QTEView.SetActive(false);
    }
}
