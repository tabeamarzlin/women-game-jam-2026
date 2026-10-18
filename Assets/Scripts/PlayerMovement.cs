using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float graceWindow = 0.30f;
    [SerializeField]
    private float socialScoreChangePerMiss = -0.1f;

    private float lastMovedOnBeat = -1;
    private float timeSinceLastBeat;
    
    [SerializeField] 
    private GameObject[] posePrefabs;

    private GameObject[] poseInstances;
    private int currentPoseIndex = -1;
    
    [SerializeField] 
    private AudioSource[] gasps;

    private void OnEnable()
    {
        BeatManager.Instance.OnBeat += OnBeat;
        InitializePoses();
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
        if (!this.enabled) return;

        Vector2 rawInput = context.ReadValue<Vector2>();

        Vector2Int gridDirection = new Vector2Int(
            Mathf.RoundToInt(rawInput.x),
            Mathf.RoundToInt(rawInput.y)
        );

        if (gridDirection == Vector2Int.zero || (gridDirection.x != 0 && gridDirection.y != 0)) return;

        int currentBeat = BeatManager.Instance.BeatIndex;

        float distanceFromBeat = timeSinceLastBeat;
        float beatLength = BeatManager.Instance.BeatLength;
        if (timeSinceLastBeat > beatLength / 2)
        {
            distanceFromBeat = beatLength - timeSinceLastBeat;
        }

        Debug.Log("Distance from beat: " + distanceFromBeat);

        if (distanceFromBeat <= graceWindow)
        {
            if (currentBeat == lastMovedOnBeat)
                return;

            Move(gridDirection);
            lastMovedOnBeat = currentBeat;
            CyclePose();
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
        EmitGasp();
        SocialScoreManager.Instance.ChangeSocialScore(socialScoreChangePerMiss);
    }

    public void EmitGasp()
    {
        int randomIndex = UnityEngine.Random.Range(0, gasps.Length);
        Debug.Log($"Random index: {randomIndex}");
        gasps[randomIndex].Play();
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
