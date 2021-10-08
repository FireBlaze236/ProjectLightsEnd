using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Bullet : MonoBehaviour
{
    [SerializeField] float _maxLifeTime = 15f;
    [SerializeField] float _damage = 10f;

    [SerializeField] TrailRenderer _trail;
    


    void OnEnable()
    {
        StartCoroutine(LifeTimer());
    }

    public void SetDamage(float amount)
    {
        _damage = amount;
        // CaP damage later
    }

    IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(_maxLifeTime);
        gameObject.SetActive(false);
        _trail.Clear();
    }


    private void OnCollisionEnter(Collision collision)
    {
        gameObject.SetActive(false);
        _trail.Clear();

        GetComponent<Rigidbody>().velocity = Vector3.zero;

        ContactPoint contactPoint = collision.contacts[0];
        Quaternion contactRotation = Quaternion.FromToRotation(Vector3.up, contactPoint.normal);

        GameObject fx = ObjectPool.Instance.GetBulletHitEffect();
        fx.transform.position = contactPoint.point;
        fx.transform.rotation = contactRotation;


        if(collision.collider.TryGetComponent<IDamageable>(out IDamageable damageableObject))
        {
            damageableObject.Damage(_damage);
        }
    }


}
