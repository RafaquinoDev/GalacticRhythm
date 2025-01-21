using UnityEngine;
using System;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource; // La fuente de audio que contiene la canción
    public float[] spectrumData = new float[256]; // Tamaño del array para el análisis FFT
    public event Action<float> OnHighDetected; // Evento para bajos pronunciados
    public event Action<float> OnLowDetected; // Evento para altos pronunciados

    [Range(0.01f, 0.5f)] public float sensitivity = 0.1f; // Sensibilidad para detectar picos
    [Range(0.01f, 0.1f)] [SerializeField] private float lowThreshold; // Umbral para detectar bajos
    [Range(0.15f, 0.25f)] [SerializeField] private float highThreshold; // Umbral para detectar altos
    
    void Update()
    {
        // Análisis FFT
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);
        
        float lowSum = 0f;
        float highSum = 0f;
        
        // Sumamos las frecuencias bajas y altas
        for (int i = 0; i < spectrumData.Length / 2; i++) 
        {
            lowSum += spectrumData[i]; // Frecuencias bajas
        }
        
        for (int i = spectrumData.Length / 2; i < spectrumData.Length; i++) 
        {
            highSum += spectrumData[i]; // Frecuencias altas
        }

        // Si las frecuencias bajas y altas sobrepasan ciertos umbrales, emitimos eventos
        if (lowSum > lowThreshold) 
        {
            OnLowDetected?.Invoke(lowSum);
        }

        if (highSum > highThreshold) 
        {
            OnHighDetected?.Invoke(highSum);
        }
    }
}
