using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(SphereCollider))]
public class DetectionRadius : MonoBehaviour
{
    public List<string> detectionTags = new List<string>();

    public Action<GameObject> OnDetectObject;

    private void OnTriggerEnter(Collider other)
    {
        if(detectionTags.Contains(other.tag))
        {
            OnDetectObject?.Invoke(other.gameObject);
        }
    }
    public void EnableRadius()
    {
        this.enabled = true;
    }
    public void DisableRadius()
    {
        this.enabled = false;
    }

    public void SetRadius(float rad)
    {

    }
}
