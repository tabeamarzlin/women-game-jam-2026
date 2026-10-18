using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuitorMovement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("How likely they are to move towards the player on a given beat. 1 = they always move towards the player, 0 = they always move randomly.")]
    private float aggressiveness = 0.8f;

    private Transform player;
    private System.Random rand = new System.Random();
    private List<Transform> otherSuitors = new();
   
    [SerializeField] private GameObject[] posePrefabs;

    private GameObject[] poseInstances;
    private int currentPoseIndex = -1;
    
    void Start()
    {
        BeatManager.Instance.OnBeat += OnBeat;

        player = FindObjectsByType<PlayerMovement>()[0].transform;

        var suitors = FindObjectsByType<SuitorMovement>();
        foreach (var suitor in suitors)
        {
            if (suitor == this) continue;

            otherSuitors.Add(suitor.transform);
        }
        
        InitializePoses();
    }

    private void OnDestroy()
    {
        BeatManager.Instance.OnBeat -= OnBeat;
    }

    private void OnBeat()
    {
        if (!this.enabled) return;
        
        CyclePose();

        var validMoves = GetValidMovements();
        if (validMoves.Count == 0) return;

        if (rand.NextDouble() <= aggressiveness)
        {
            MoveTowardsPlayer(validMoves);
        }
        else
        {
            MoveInRandomDirection(validMoves);
        }
    }

    private void MoveTowardsPlayer(List<Vector2Int> validMoves)
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

        if (validMoves.Any(move => move.Equals(gridDirection))) {
            Move(gridDirection);
        }
        else
        {
            MoveInRandomDirection(validMoves);
        }
    }

    private void MoveInRandomDirection(List<Vector2Int> validMoves)
    {
        int index = rand.Next(0, validMoves.Count);
        var move = validMoves[index];

        Move(move);
    }

    private void Move(Vector2Int gridDirection)
    {
        transform.Translate(gridDirection.x, 0, gridDirection.y);
    }

    private bool IsPositionOccupied(Vector3 position)
    {
        foreach (var suitor in otherSuitors)
        {
            if (suitor.position.Equals(position)) return true;
        }

        return false;
    }

    private List<Vector2Int> GetValidMovements()
    {
        List<Vector2Int> movements = new List<Vector2Int>();
        movements.Add(new Vector2Int(-1, 0));
        movements.Add(new Vector2Int(1, 0));
        movements.Add(new Vector2Int(0, -1));
        movements.Add(new Vector2Int(0, 1));

        List<Vector2Int> validMovements = new List<Vector2Int>();

        Vector3 pos = transform.position;

        foreach (var movement in movements)
        {
            Vector3 candidatePos = new Vector3(pos.x + movement.x, pos.y, pos.z + movement.y);
            if (!IsPositionOccupied(candidatePos))
            {
                validMovements.Add(movement);
            }
        }

        return validMovements;
    }
    
    private void InitializePoses()
    {
        poseInstances = new GameObject[posePrefabs.Length];

        for (int i = 0; i < posePrefabs.Length; i++)
        {
            poseInstances[i] = Instantiate(
                posePrefabs[i],
                transform
            );

            poseInstances[i].transform.localPosition = Vector3.zero;
            poseInstances[i].transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            poseInstances[i].SetActive(false);
        }

        if (poseInstances.Length > 0)
        {
            currentPoseIndex = 0;
            poseInstances[0].SetActive(true);
        }
    }
    
    private void CyclePose()
    {
        if (poseInstances == null || poseInstances.Length == 0)
            return;

        poseInstances[currentPoseIndex].SetActive(false);

        currentPoseIndex = (currentPoseIndex + 1) % poseInstances.Length;

        poseInstances[currentPoseIndex].SetActive(true);
    }
}
