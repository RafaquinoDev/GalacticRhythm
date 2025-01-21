using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public Renderer objectRenderer; // Referencia al renderer del objeto (para obstáculos, enemigos, etc.)

    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private Color maincolor;
    private float nextSpawnTime;

    private MusicManager musicManager;

    private bool availableColorChange = true;

    void Start()
    {
        musicManager = FindObjectOfType<MusicManager>();
        musicManager.OnLowDetected += ChangeToLowColor; // Suscribir evento de bajos
        musicManager.OnHighDetected += ChangeToHighColor; // Suscribir evento de altos

        nextSpawnTime = Time.time + spawnInterval;
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime) // Verifica si es tiempo de spawn
        {
            CalculateNextSpawnTime(); // Recalcula el tiempo para el próximo spawn
            //Debug.Log(maincolor.ToString());
        }
    }

    void ChangeToLowColor(float intensity)
    {
        if (!availableColorChange) return;
        Color newColor = new Color(1f * intensity, 1f * intensity, 1f, 1f); // Colores fríos para bajos
        Debug.Log(newColor.ToString());
        objectRenderer.material.color = newColor;
        availableColorChange = false;
    }

    void ChangeToHighColor(float intensity)
    {
        if (!availableColorChange) return;
        Color newColor = new Color(1f, 1f * intensity, 1f * intensity, 1f); // Colores cálidos para altos
        Debug.Log(newColor.ToString());
        objectRenderer.material.color = newColor;
        availableColorChange = false;
    }

    private void CalculateNextSpawnTime()
    {
        nextSpawnTime = Time.time + spawnInterval;
        availableColorChange = true;
    }

    void OnDisable() 
    {
        musicManager.OnLowDetected -= ChangeToLowColor;
        musicManager.OnHighDetected -= ChangeToHighColor;
    }
}
