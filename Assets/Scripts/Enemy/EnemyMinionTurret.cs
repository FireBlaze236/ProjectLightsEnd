using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMinionTurret : MonoBehaviour, IDamageable
{
    [SerializeField] DetectionRadius _detectionRadius;
    [SerializeField] Transform _targetTransform;

    [SerializeField] Transform _gunTransform;
    [SerializeField] Transform _shootPoint;
    [SerializeField] GameObject _muzzleFlash;

    [SerializeField] bool _targetAcquired = false;
    [SerializeField] float _fireRate = 10f;
    
    [SerializeField] bool _canShoot = true;



    [SerializeField] float _range = 15f;
    [SerializeField] float _damage = 5f;
    [SerializeField] float _bulletForce = 10f;

    [Header("Stats")]
    [SerializeField] float _health = 100f;
    [SerializeField] float _maxHealth = 100f;
    [SerializeField] AudioClip _shootSound;

    public Action<GameObject> OnEnemyTurretDead;
    [SerializeField] AudioClip _deadSound;

    private void OnEnable()
    {
        _detectionRadius.OnDetectObject += DetectTarget;

    }
    private void OnDisable()
    {
        _detectionRadius.OnDetectObject -= DetectTarget;
    }

    private void Update()
    {
        if (_targetAcquired)
        {
            if ((_targetTransform.position - transform.position).magnitude <= _range)
            {
                TrackTarget();
                ShootBullet();
            }
        }
    }


    IEnumerator FireRateLoop()
    {
        _canShoot = false;
        yield return new WaitForSeconds(1f / _fireRate);
        _canShoot = true;
    }
    private void DetectTarget(GameObject target)
    {
        
        _targetTransform = target.transform;
        _targetAcquired = true;
    }

    public void Damage(float amount)
    {
        _health -= amount;
        _health = Mathf.Clamp(_health, 0f, _maxHealth);

        if (_health == 0f)
        {
            OnEnemyTurretDead?.Invoke(this.gameObject);

            GameObject explosionEffect = ObjectPool.Instance.GetBigExplosion();
            explosionEffect.transform.position = transform.position;

            gameObject.SetActive(false);

            SFXPlayer.Instance.PlaySoundEffect(transform.position, _deadSound);
        }

        
    }

    private void TrackTarget()
    {
        _gunTransform.LookAt(_targetTransform.position);
    }

    private void ShootBullet()
    {
        if(!_canShoot)
        {
            return;
        }

        GameObject go = ObjectPool.Instance.GetEnemyBullet();
        Bullet bullet = go.GetComponent<Bullet>();

        bullet.transform.position = _shootPoint.position;
        bullet.transform.rotation = _shootPoint.rotation;

        bullet.SetDamage(_damage);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        bulletRb.AddForce(_gunTransform.forward * _bulletForce, ForceMode.Impulse);

        StartCoroutine(FireRateLoop());

        foreach(ParticleSystem p in _muzzleFlash.GetComponentsInChildren<ParticleSystem>())
        {
            p.Play();
        }

        SFXPlayer.Instance.PlaySoundEffect(_shootPoint.position, _shootSound);
    }



}
