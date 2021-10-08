using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [SerializeField] GameObject _muzzleFlash;

    [SerializeField] float _bulletForce = 50f;
    [SerializeField] Transform _shootPoint;
    [SerializeField] float _fireRate = 1f;
    [SerializeField] float _bulletDamage = 10f;

    [SerializeField] bool _canShoot = true;

    [SerializeField] AudioClip _shootSound;
   
    void Update()
    {
        if(Input.GetButton("Fire1") && _canShoot)
        {
            ShootBullet();
        }
    }

    public void MultiplyFireRate(float mul, float duration)
    {
        StartCoroutine(BoostCoroutine(mul, duration));
    }

    IEnumerator BoostCoroutine(float mul, float duration)
    {
        float prev = _fireRate;
        _fireRate *= mul;
        yield return new WaitForSeconds(duration);
        _fireRate = prev;
    }

    IEnumerator FireRateCoroutine()
    {
        _canShoot = false;
        yield return new WaitForSeconds(1f / _fireRate);
        _canShoot = true;
    }

    private void ShootBullet()
    {
        GameObject newBullet = ObjectPool.Instance.GetPlayerBullet();

        newBullet.transform.position = _shootPoint.position;
        newBullet.transform.forward = _shootPoint.forward;

        Rigidbody bulletRb = newBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = Vector3.zero;

        newBullet.GetComponent<Bullet>().SetDamage(_bulletDamage);

        bulletRb.AddForce(_bulletForce * _shootPoint.forward, ForceMode.Impulse);

        //Play flash fx
        foreach(var p in _muzzleFlash.GetComponentsInChildren<ParticleSystem>())
        {
            p.Play();
        }

        SFXPlayer.Instance.PlaySoundEffect(_shootPoint.position, _shootSound);

        StartCoroutine(FireRateCoroutine());

    }
}
