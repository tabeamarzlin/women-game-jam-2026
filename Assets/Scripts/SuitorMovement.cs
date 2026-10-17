using System;
using UnityEngine;

public class SuitorMovement : MonoBehaviour
{
    private Transform player;
    private System.Random rand = new System.Random();
    
    void Start()
    {
        BeatManager.Instance.OnBeat += OnBeat;

        player = FindObjectsByType<PlayerMovement>()[0].transform;
    }

    private void OnBeat()
    {
        MoveTowardsPlayer();
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

    private void Move(Vector2Int gridDirection)
    {
        transform.Translate(gridDirection.x, 0, gridDirection.y);
    }
}
