using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float graceWindow = 0.15f;
    [SerializeField]
    private float socialScoreChangePerMiss = -0.1f;

    private float lastMovedOnBeat;
    private float timeSinceLastBeat;

    private void Start()
    {
        BeatManager.Instance.OnBeat += OnBeat;
    }

    private void OnDestroy()
    {
        BeatManager.Instance.OnBeat -= OnBeat;
    }

    private void OnEnable()
    {
        BeatManager.Instance.OnBeat += OnBeat;
    }

    private void OnDisable()
    {
        BeatManager.Instance.OnBeat -= OnBeat;
    }

    private void OnBeat()
    {
        timeSinceLastBeat = 0;
    }

    private void Update()
    {
        timeSinceLastBeat += Time.deltaTime;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 rawInput = context.ReadValue<Vector2>();

        Vector2Int gridDirection = new Vector2Int(
            Mathf.RoundToInt(rawInput.x),
            Mathf.RoundToInt(rawInput.y)
        );

        if (gridDirection == Vector2Int.zero || (gridDirection.x != 0 && gridDirection.y != 0)) return;

        float lastBeat = BeatManager.Instance.LastBeat;
        float closestWholeBeat = Mathf.Round(lastBeat);

        float distanceFromBeat = timeSinceLastBeat;
        float beatLength = BeatManager.Instance.BeatLength;
        if (timeSinceLastBeat > beatLength / 2)
        {
            distanceFromBeat = beatLength - timeSinceLastBeat;
        }

        Debug.Log("Distance from beat: " + distanceFromBeat);

        if (distanceFromBeat <= graceWindow)
        {
            //Cannot move twice on the same beat
            if (closestWholeBeat == lastMovedOnBeat) return;

            Move(gridDirection);
            lastMovedOnBeat = closestWholeBeat;
        }
        else
        {
            Fail();
        }

    }

    private void Move(Vector2Int gridDirection)
    {
        transform.Translate(-gridDirection.y, 0, gridDirection.x);
    }

    private void Fail()
    {
        SocialScoreManager.Instance.ChangeSocialScore(socialScoreChangePerMiss);
    }
}
