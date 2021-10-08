using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectTimer : MonoBehaviour
{
    [SerializeField] float _timeToDisable = 5f;

    private void OnEnable()
    {
        StartCoroutine(TimeToDisable());
    }


    IEnumerator TimeToDisable()
    {
        yield return new WaitForSeconds(_timeToDisable);
        gameObject.SetActive(false);
    }
}
