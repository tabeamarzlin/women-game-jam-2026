using UnityEngine;

public class Table : MonoBehaviour
{
    [SerializeField]
    private float refillPerBeat = 0.1f;

    private bool isPlayerInRange = false;

    private void Start()
    {
        BeatManager.Instance.OnBeat += OnBeat;
    }

    private void OnDestroy()
    {
        BeatManager.Instance.OnBeat -= OnBeat;
    }

    private void OnBeat()
    {
        if (!isPlayerInRange) return;

        SocialScoreManager.Instance.ChangeSocialScore(refillPerBeat);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
