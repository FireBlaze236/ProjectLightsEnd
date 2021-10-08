using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] GameObject _objectToPool;
    [SerializeField] int _initialPoolAmount = 5;

    [SerializeField] List<GameObject> _pool = new List<GameObject>();
    private void Start()
    {
        for(int i =0;  i< _initialPoolAmount; i++)
        {
            AddObject();
        }
    }

    private void AddObject()
    {
        GameObject go = Instantiate(_objectToPool, this.transform);
        go.SetActive(false);
        _pool.Add(go);
    }


    public GameObject GetObject()
    {
        foreach(GameObject go in _pool)
        {
            if(!go.activeInHierarchy)
            {
                go.SetActive(true);
                return go;
            }
        }

        AddObject();
        return GetObject();
    }

    private void OnDestroy()
    {
        this.enabled = false;
    }
}
