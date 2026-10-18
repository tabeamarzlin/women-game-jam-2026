using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public enum Direction
{
    Up = 0, Down = 1, Left = 2, Right = 3
}

public class PlayerQTEController : MonoBehaviour
{
    [SerializeField]
    private float distanceFromSuitorToTrigger = 1f;
    [SerializeField]
    private float graceWindow = 0.15f;
    [SerializeField]
    private float socialScoreChangePerMiss = -0.1f;
    [SerializeField]
    private int correctHitsRequired = 4;

    private List<Transform> suitors = new();

    private Direction currentDirection;
    private System.Random rand = new System.Random();
    private bool isInQTE = false;
    private int correctHits = 0;
    private float timeSinceLastBeat;

    public event Action<Direction> OnArrowDirectionChanged;

    void Start()
    {
        BeatManager.Instance.OnBeat += OnBeat;

        var suitorsObjs = FindObjectsByType<SuitorMovement>();
        foreach (var suitor in suitorsObjs)
        {
            suitors.Add(suitor.transform);
        }
    }

    private void OnDestroy()
    {
        BeatManager.Instance.OnBeat -= OnBeat;
    }

    private void OnBeat()
    {
        if (!isInQTE) return;

        SwitchToNextDirection();
    }

    private void SwitchToNextDirection()
    {
        int randomIndex = rand.Next(4);
        Direction direction = (Direction)randomIndex;

        OnArrowDirectionChanged?.Invoke(direction);
    }

    void Update()
    {
        if (isInQTE) return;

        foreach (var suitor in suitors)
        {
            if (Vector3.Distance(transform.position, suitor.position) <= distanceFromSuitorToTrigger)
            {
                StartQTE();
            }
        }
    }

    private void StartQTE()
    {
        isInQTE = true;
        correctHits = 0;
    }

    private void EndQTE()
    {
        isInQTE = false;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        if (!isInQTE) return;

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
            if (IsCorrectDirection(gridDirection))
            {
                correctHits++;
                if (correctHits == correctHitsRequired) EndQTE();
            }
            else
            {
                Fail();
            }
        }
        else
        {
            Fail();
        }

    }

    private bool IsCorrectDirection(Vector2Int input)
    {
        switch (currentDirection)
        {
            case Direction.Up:
                if (input.y == 1) return true;
                break;
            case Direction.Down:
                if (input.y == -1) return true;
                break;
            case Direction.Right:
                if (input.x == 1) return true;
                break;
            case Direction.Left:
                if (input.x == -1) return true;
                break;
        }

        return false;
    }

    private void Fail()
    {
        SocialScoreManager.Instance.ChangeSocialScore(socialScoreChangePerMiss);
    }
}
