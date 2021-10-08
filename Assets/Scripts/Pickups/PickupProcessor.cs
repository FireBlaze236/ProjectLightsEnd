using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Have this as a component of a child which has a trigger
/// </summary>
[RequireComponent(typeof(Collider))]
public class PickupProcessor : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Pickup"))
        {
            if(other.TryGetComponent<IPickup>(out IPickup pickup))
            {
                pickup.Pickup(transform.parent.gameObject);
            }
            GameManager.Log("Pickup Hit!");
        }
    }
}
