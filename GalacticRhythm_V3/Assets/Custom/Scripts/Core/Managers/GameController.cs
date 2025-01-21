using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameController : MonoBehaviour
{
    public static GameController Instance;
    private void Awake() 
    {
        if(GameController.Instance == null)
        {
            GameController.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private float gameSpeed; //Creamos la variable gameSpeed que controlara la velocidad del juego.
    [Range(0, 5)] public float speedRate; //Con esta variable podemos regular la velocidad
    public float gameSpeedMax = 5; //Incrementara gameSpeedRegulator por segundo
    void Update()
    {
        gameSpeed = speedRate * Time.deltaTime;
        if (speedRate >= gameSpeedMax) //Verificamos si gameSpeedRegulator es menor o igual a gameSpeedMax
        {
            gameSpeed = gameSpeedMax * Time.deltaTime; //Incrementamos la velocidad del juego en base a speedRate
        }
    }

    public float GetGameSpeed(){ return this.gameSpeed; }
}
