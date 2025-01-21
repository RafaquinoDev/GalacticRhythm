using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SpawnManagerForObjects : MonoBehaviour
{
    [Header("Configuración de Spawn de Objetos")]
    [SerializeField] Transform borderLeft;  // Límite izquierdo de la pantalla para el spawn
    [SerializeField] Transform borderRight; // Límite derecho de la pantalla para el spawn
    [SerializeField] private float spawnInterval = 1f; // Intervalo base de spawn
    private int difficultyLevel = 1;  // Nivel de dificultad que afecta las probabilidades de spawn
    
    [Header("Configuración Musical")]
    [SerializeField] private float minToSpawn = .01f;


    [Header("Referencias")]
    [SerializeField] private GameObject prefabObject;  // Prefab base para los elementos
    [SerializeField] private List<FallingElement> defaultElementsPool;  //Lista de posibles elementos que caen
    [SerializeField] private List<PowerUpElement> powerupsElementsPool;  //Lista de posibles powerups que caen
    
    private ObjectPool<GameObject> gameObjectPool; // Pool de objetos
    private MusicManager musicManager;
    private float nextSpawnTime; // Tiempo siguiente de spawn
    private bool availableSpawn;
    
    private void Start()
    {
        InitializeObjectPool(); // Inicializa el pool de objetos
        musicManager = FindObjectOfType<MusicManager>();
        nextSpawnTime = Time.time + spawnInterval; // Calcula el primer spawn

        if(musicManager != null) musicManager.OnLowDetected += SpawnRandomElement;
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
        int poolMaxSize = (defaultElementsPool.Count + powerupsElementsPool.Count) * 3;

        gameObjectPool = new ObjectPool<GameObject>(() => 
        {
            return Instantiate(prefabObject); // Crea un nuevo objeto de pool
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
    
    private void SpawnRandomElement(float musicLow)
    {
        if(!availableSpawn) return;

        // Selecciona aleatoriamente un elemento
        float random = Random.value;
        FallingElement elementToSpawn = null;
        float powerUpProb = 0.05f - (difficultyLevel * 0.01f);
        
        elementToSpawn = defaultElementsPool[Random.Range(0, defaultElementsPool.Count)];
        
        if (random < powerUpProb)
        {
            elementToSpawn = powerupsElementsPool[Random.Range(0, powerupsElementsPool.Count)];
        }
        
        if (elementToSpawn != null && Random.value <= elementToSpawn.spawnProbability)
        {
            //Debug.Log(minToSpawn + " < " + musicLow);
            if(minToSpawn < musicLow) 
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
        float spawnRate = spawnInterval * (1f - (difficultyLevel * 0.1f));
        nextSpawnTime = Time.time + spawnRate;
        availableSpawn = true;
    }
}