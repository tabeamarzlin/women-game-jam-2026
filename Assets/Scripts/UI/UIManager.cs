using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject victoryScreen;
    [SerializeField]
    private GameObject defeatScreen;
    [SerializeField]
    private Door door;

    void Start()
    {
        victoryScreen.SetActive(false);
        defeatScreen.SetActive(false);

        SocialScoreManager.Instance.OnSocialScoreDepleted += OnLose;
        door.OnDoorReached += OnWin;

        Time.timeScale = 1;
    }

    private void OnLose()
    {
        defeatScreen.SetActive(true);

        Time.timeScale = 0;
    }

    private void OnWin()
    {
        victoryScreen.SetActive(true);

        Time.timeScale = 0;
    }
}
