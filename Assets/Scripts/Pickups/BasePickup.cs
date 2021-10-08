using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePickup : MonoBehaviour, IPickup
{
    [SerializeField] protected float _amount = 50f;
    [SerializeField] float _rotateSpeed = 20f;
    [SerializeField] float _delta = 10f;
    [SerializeField] protected AudioClip _sound;

    private void Start()
    {
        _rotateSpeed = Random.Range(_rotateSpeed - _delta, _rotateSpeed + _delta);
    }
    public virtual void Pickup(GameObject who)
    {
        SFXPlayer.Instance.PlaySoundEffect(transform.position, _sound);
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
    }
}
