using System;
using UnityEngine;

public class SuitorMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How likely they are to move towards the player on a given beat. 1 = they always move towards the player, 0 = they always move randomly.")]
    private float aggressiveness = 0.8f;

    private Transform player;
    private System.Random rand = new System.Random();
    
    void Start()
    {
        BeatManager.Instance.OnBeat += OnBeat;

        player = FindObjectsByType<PlayerMovement>()[0].transform;
    }

    private void OnDestroy()
    {
        BeatManager.Instance.OnBeat -= OnBeat;
    }

    private void OnBeat()
    {
        if (rand.NextDouble() <= aggressiveness)
        {
            MoveTowardsPlayer();
        }
        else
        {
            MoveInRandomDirection();
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2Int gridDirection = new Vector2Int(0, 0);

        if (player.position.x > transform.position.x)
        {
            gridDirection.x = 1;
        }
        else if (player.position.x < transform.position.x)
        {
            gridDirection.x = -1;
        }

        if (player.position.z > transform.position.z)
        {
            gridDirection.y = 1;
        }
        else if (player.position.z < transform.position.z)
        {
            gridDirection.y = -1;
        }

        if (Mathf.Abs(gridDirection.x) > 0 && Mathf.Abs(gridDirection.y) > 0)
        { 
            //Cannot move diagonally - choose randomly whether to move horizontally or vertically
            if (rand.NextDouble() >= 0.5)
            {
                gridDirection.x = 0;
            }
            else
            {
                gridDirection.y = 0;
            }
        }

        Move(gridDirection);
    }

    private void MoveInRandomDirection()
    {
        Vector2Int gridDirection = new Vector2Int(0, 0);

        if (rand.NextDouble() >= 0.5)
        {
            if (rand.NextDouble() >= 0.5)
            {
                gridDirection.x = 1;
            }
            else
            {
                gridDirection.x = -1;
            }
        }
        else
        {
            if (rand.NextDouble() >= 0.5)
            {
                gridDirection.y = 1;
            }
            else
            {
                gridDirection.y = -1;
            }
        }

        Move(gridDirection);
    }

    private void Move(Vector2Int gridDirection)
    {
        transform.Translate(gridDirection.x, 0, gridDirection.y);
    }
}
