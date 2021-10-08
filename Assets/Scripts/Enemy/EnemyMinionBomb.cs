using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class EnemyMinionBomb : MonoBehaviour, IDamageable
{
    [SerializeField] Transform _targetTransform;
    [SerializeField] Rigidbody _rb;

    [SerializeField] DetectionRadius _detectionRadius;
    

    [SerializeField] float _health = 100f;
    [SerializeField] float _maxHealth = 100f;
    


    [SerializeField] float _damage = 10f;
    [SerializeField] float _explosionRange = 10f;
    [SerializeField] float _decreasedRange = 5f;

    [SerializeField] bool _targetAcquired = false;
    [SerializeField] float _bombTime = 10f;
    [SerializeField] float _moveForce = 5f;

    [SerializeField] bool _isDead = false;


    public Action<GameObject> OnEnemyBombExplode;
    public Action OnEnemyDetectTarget;

    [SerializeField] AudioClip _explosionSound;

    private void OnEnable()
    {
        _detectionRadius.OnDetectObject += DetectTarget;
        _rb.isKinematic = true;
    }
    private void OnDisable()
    {
        _detectionRadius.OnDetectObject -= DetectTarget;
        _rb.isKinematic = true;
    }

    public void Damage(float amount)
    {
        _health -= amount;
        _health = Mathf.Clamp(_health, 0f, _maxHealth);

        if(_health == 0f)
        {
            Explode(_decreasedRange);
            
            OnEnemyBombExplode?.Invoke(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if(_targetAcquired)
        {
            Vector3 targetZX = new Vector3(_targetTransform.position.x, 0f, _targetTransform.position.z);
            Vector3 dir = targetZX - transform.position;
            _rb.AddForce(dir * _moveForce, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_targetTransform != null && collision.collider.transform == _targetTransform)
        {
            Explode(_explosionRange);
        }
    }

    private void DetectTarget (GameObject target)
    {
        _targetTransform = target.transform;
        _targetAcquired = true;
        _detectionRadius.DisableRadius();
        _rb.isKinematic = false;

        StartCoroutine(BombTimerCoroutine());
    }
    
    private void Explode(float range)
    {
        if (_isDead) return;

        OnEnemyBombExplode?.Invoke(this.gameObject);
        int _ignoreLayer = ~(1 << gameObject.layer);

        Collider[] hits = Physics.OverlapSphere(transform.position, range, _ignoreLayer);

        if (hits.Length == 0)
            return;

        foreach (Collider col in hits)
        {
            if(col.TryGetComponent<IDamageable>(out IDamageable damageableObject))
            {
                damageableObject.Damage(_damage);
            }
        }

        GameObject explosionEffect = ObjectPool.Instance.GetBigExplosion();
        explosionEffect.transform.position = transform.position;


        gameObject.SetActive(false);

        SFXPlayer.Instance.PlaySoundEffect(transform.position, _explosionSound);
        _isDead = true;
    }

    IEnumerator BombTimerCoroutine()
    {
        yield return new WaitForSeconds(_bombTime);
        Explode(_explosionRange);
    }

}
