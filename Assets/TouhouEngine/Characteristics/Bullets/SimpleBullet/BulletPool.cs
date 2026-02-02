using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    private static BulletPool _instance;
    public static BulletPool Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("La instancia de BulletPool es nula. Asegúrese de que haya un BulletPool en la escena.");

            return _instance;
        }
    }

    [SerializeField] private BulletController bulletprefab;
    [SerializeField] private int initialPoolSize = 10;

    private List<BulletController> pool = new List<BulletController>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;

        AddBulletsToPool(initialPoolSize);
    }

    private void AddBulletsToPool(int cant)
    {
        for (int i = 0; i < cant; i++)
        {
            BulletController bullet = Instantiate(bulletprefab);
            bullet.gameObject.SetActive(false);
            pool.Add(bullet);
            bullet.transform.parent = this.transform;
        }
    }

    public BulletController RequestBullet()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].gameObject.activeSelf)
            {
                pool[i].gameObject.SetActive(true);
                return pool[i];
            }
        }

        BulletController newBullet = Instantiate(bulletprefab);
        newBullet.gameObject.SetActive(false);
        newBullet.transform.parent = this.transform;
        pool.Add(newBullet);
        return newBullet;
    }
}