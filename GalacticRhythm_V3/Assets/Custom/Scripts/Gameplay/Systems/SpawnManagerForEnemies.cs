using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManagerForEnemies : MonoBehaviour
{
    [Header("Configuración de Spawn Enemigo")]
    [SerializeField] Transform borderLeft;  // Límite izquierdo de la pantalla para el spawn
    [SerializeField] Transform borderRight; // Límite derecho de la pantalla para el spawn
    [SerializeField] private float spawnInterval = 1f; // Intervalo base de spawn
    private int difficultyLevel = 1;  // Nivel de dificultad que afecta las probabilidades de spawn
    
    [Header("Configuración Musical")]
    [SerializeField] private float minToSpawn = .05f;

    [Header("Referencias")]
    [SerializeField] private GameObject prefabEnemy;  // Prefab base para los elementos
    [SerializeField] private List<EnemyElement> enemyElementsPool;  //Lista de posibles enemigos que aparecen
    
    private ObjectPool<GameObject> gameObjectPool; // Pool de objetos
    private MusicManager musicManager;
    private float nextSpawnTime; // Tiempo siguiente de spawn
    private bool availableSpawn = true;

    private void Start()
    {
        InitializeObjectPool(); // Inicializa el pool de objetos
        musicManager = FindObjectOfType<MusicManager>();
        nextSpawnTime = Time.time + spawnInterval; // Calcula el primer spawn
    
        if(musicManager != null) musicManager.OnHighDetected += SpawnRandomElement;
    }
    
    private void Update()
    {
        if (Time.time >= nextSpawnTime) // Verifica si es tiempo de spawn
        {
            CalculateNextSpawnTime(); // Recalcula el tiempo para el próximo spawn
        }
    }
    
    private void InitializeObjectPool()
    {
        // Inicializa el pool de objetos con un tamaño ajustado al número de elementos
        int poolMaxSize = enemyElementsPool.Count * 3;

        gameObjectPool = new ObjectPool<GameObject>(() => 
        {
            return Instantiate(prefabEnemy); // Crea un nuevo objeto de pool
        }, poolObject => 
        {
            poolObject.gameObject.SetActive(true); // Activa el objeto al obtenerlo del pool
        }, poolObject => 
        {
            poolObject.gameObject.SetActive(false); // Desactiva el objeto al devolverlo al pool
        }, poolObject => 
        {
            Destroy(poolObject.gameObject); // Destruye el objeto si se excede el tamaño del pool
        }, true, 10, poolMaxSize); // Configuración inicial del pool
    }
    
    private void SpawnRandomElement(float musicHigh)
    {
        if(!availableSpawn) return;

        FallingElement elementToSpawn = null;

        // Selecciona aleatoriamente un enemigo
        elementToSpawn = enemyElementsPool[Random.Range(0, enemyElementsPool.Count)];
        
        if (elementToSpawn != null && Random.value <= elementToSpawn.spawnProbability)
        {
            Debug.Log(minToSpawn + " < " + musicHigh);
            if (minToSpawn < musicHigh) 
            {
                SpawnElement(elementToSpawn); // Genera el elemento con la configuración del scriptable
                availableSpawn = false;
            }
        }
    }
    
    private void SpawnElement(FallingElement data)
    {
        // Obtiene un objeto del pool y lo configura
        GameObject pooledObject = gameObjectPool.Get();

        FallingElementBehavior behavior = pooledObject.GetComponent<FallingElementBehavior>();
        
        if (pooledObject != null)
        {
            behavior.Initialize(data, gameObjectPool); // Inicializa el elemento con sus datos específicos
            ConfigurePosition(pooledObject); // Configura la posición inicial del spawn
        }
    }
    
    private void ConfigurePosition(GameObject obj)
    {
        // Configura una posición aleatoria entre los límites establecidos
        float randomX = Random.Range(borderLeft.position.x, borderRight.position.x);
        obj.transform.position = new Vector3(randomX, transform.position.y, 0);
    }
    
    private void CalculateNextSpawnTime()
    {
        // Calcula el próximo tiempo de spawn según el nivel de dificultad
        float spawnRate = spawnInterval * (1f - (difficultyLevel * 0.115f));
        nextSpawnTime = Time.time + spawnRate;
        availableSpawn = true;
    }
}