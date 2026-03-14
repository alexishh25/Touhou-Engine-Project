using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A pool of inactive bullets that can be requested to be activated and used.
/// Improves performance by avoiding the need to instantiate new bullets every time they are needed, reusing existing ones instead.
/// </summary>
public class BulletPool : MonoBehaviour
{

    private static BulletPool _instance;
    public static BulletPool Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("BulletPool instance is null. Make sure there is a BulletPool in the scene.");

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
        foreach (var bullet in pool)
        {
            if (!bullet.gameObject.activeInHierarchy)
                return bullet;
        }

        BulletController newBullet = Instantiate(bulletprefab);
        newBullet.gameObject.SetActive(true);
        newBullet.transform.parent = this.transform;
        pool.Add(newBullet);
        return newBullet;
    }
}