using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ShootManager : MonoBehaviour
{
    [Header("Configuración de Proyectil")]
    [SerializeField] Transform firePoint;
    [SerializeField] private GameObject prefabProjectile;  // Prefab base para los elementos
    [SerializeField] private ProjectileElement projectileData; // Configuraci[on] del scriptableobject
    
    private ObjectPool<GameObject> gameObjectPool; // Pool de objetos
    private float nextSpawnTime; // Tiempo siguiente de spawn

    public event EventHandler OnShooting;
    
    private void Start()
    {
        InitializeObjectPool(); // Inicializa el pool de objetos
        nextSpawnTime = Time.time + projectileData.fireRate; // Calcula el primer spawn
    }
    
    private void Update()
    {
        if (Time.time >= nextSpawnTime) // Verifica si es tiempo de spawn
        {
            SpawnProjectile(projectileData);
            OnShooting?.Invoke(this, EventArgs.Empty);
            nextSpawnTime = Time.time + NextShotCalculate(); // Recalcula el tiempo para el próximo spawn
        }
    }

    public float NextShotCalculate(float bonus = 1f)
    {
        return projectileData.fireRate * bonus;
    }

    private void InitializeObjectPool()
    {
        // Inicializa el pool de objetos
        gameObjectPool = new ObjectPool<GameObject>(() => 
        {
            return Instantiate(prefabProjectile); // Crea un nuevo objeto de pool
        }, poolObject => 
        {
            poolObject.gameObject.SetActive(true); // Activa el objeto al obtenerlo del pool
        }, poolObject => 
        {
            poolObject.gameObject.SetActive(false); // Desactiva el objeto al devolverlo al pool
        }, poolObject => 
        {
            Destroy(poolObject.gameObject); // Destruye el objeto si se excede el tamaño del pool
        }, true, 8, 15); // Configuración inicial del pool
    }
    
    private void SpawnProjectile(ProjectileElement data)
    {
        // Obtiene un objeto del pool y lo configura
        GameObject pooledObject = gameObjectPool.Get();

        ProjectileBehavior behavior = pooledObject.GetComponent<ProjectileBehavior>();
        
        if (pooledObject != null)
        {
            behavior.Initialize(data, gameObjectPool); // Inicializa el elemento con sus datos específicos
            pooledObject.transform.position = firePoint.transform.position; // Configura la posición inicial del spawn
        }
    }
}