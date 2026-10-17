using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public float graceWindow = 0.15f;

    private float lastMovedOnBeat;

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
        float distanceFromBeat = Mathf.Abs(lastBeat - closestWholeBeat);

        //Cannot move twice on the same beat
        if (closestWholeBeat == lastMovedOnBeat) return;

        if (distanceFromBeat <= graceWindow)
        {
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
        //TODO
    }
}
