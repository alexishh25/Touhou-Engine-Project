using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// A pool of inactive bullets that can be requested to be activated and used.
/// Improves performance by avoiding the need to instantiate new bullets every time they are needed, reusing existing ones instead.
/// </summary>
public class BulletPoolManager : MonoBehaviour
{

    private static BulletPoolManager _instance;
    public static BulletPoolManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("BulletPool instance is null. Make sure there is a BulletPool in the scene.");

            return _instance;
        }
    }

    private Dictionary<BulletController, List<BulletController>> pool 
        = new Dictionary<BulletController, List<BulletController>>();

    [SerializeField] private BulletController playerBulletPrefab;
    [SerializeField] private BulletController enemyBulletPrefab;
    [SerializeField] private int playerPoolSize = 20;
    [SerializeField] private int enemyPoolSize = 50;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
    }

    private void Start()
    {
        if (playerBulletPrefab != null) WarmPool(playerBulletPrefab, playerPoolSize);
        if (enemyBulletPrefab != null) WarmPool(enemyBulletPrefab, enemyPoolSize);
    }

    public void WarmPool(BulletController prefab, int size)
    {
        if (!pool.ContainsKey(prefab))
            pool[prefab] = new List<BulletController>();

        for (int i = 0; i < size; i++)
        {
            BulletController bullet = Instantiate(prefab); // use the parameter, not the serialized field
            bullet.gameObject.SetActive(false);
            bullet.transform.SetParent(transform, worldPositionStays: true);
            pool[prefab].Add(bullet);
        }
    }

    public BulletController RequestBullet(BulletController prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("BulletPoolManager.RequestBullet: prefab is null. Assign it in the inspector.");
            return null;
        }

        if (!pool.ContainsKey(prefab))
            pool[prefab] = new List<BulletController>();

        foreach (var bullet in pool[prefab])
        {
            if (!bullet.gameObject.activeInHierarchy)
                return bullet;
        }

        // Instantiate without parent to preserve original prefab scale
        BulletController newBullet = Instantiate(prefab);
        newBullet.gameObject.SetActive(false);
        // Set parent after instantiation so scale is not inherited
        newBullet.transform.SetParent(transform, worldPositionStays: true);
        pool[prefab].Add(newBullet);
        return newBullet;
    }
}