using System;
using UnityEngine;

public class Door : MonoBehaviour
{
    public event Action OnDoorReached;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            OnDoorReached?.Invoke();
        }
    }
}
