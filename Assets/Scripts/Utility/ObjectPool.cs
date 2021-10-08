using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [SerializeField] ObjectPooler _playerBulletPool;
    [SerializeField] ObjectPooler _bulletHitFXPool;
    [SerializeField] ObjectPooler _enemyBulletPool;
    [SerializeField] ObjectPooler _bigExplosionPool;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    public GameObject GetEnemyBullet()
    {
        return _enemyBulletPool.GetObject();
    }

    public GameObject GetPlayerBullet()
    {
        return _playerBulletPool.GetObject();
    }

    public GameObject GetBulletHitEffect()
    {
        return _bulletHitFXPool.GetObject();
    }

    public GameObject GetBigExplosion()
    {
        return _bigExplosionPool.GetObject();
    }
        
}
