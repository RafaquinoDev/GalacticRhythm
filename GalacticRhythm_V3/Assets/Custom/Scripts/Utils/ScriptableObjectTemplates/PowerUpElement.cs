using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerUpElement", menuName = "GalacticRhythm/SpawnElements/FallingElement/PowerUp")]
public class PowerUpElement : FallingElement
{
    [Header("Configuraci�n de PowerUps")]
    public PowerUpType powerUpType;
    public float effectDuration;
    public float effectMultiplier;
    public int lifeAmount;

    public enum PowerUpType
    {
        moreLife, moreFireRate
    }
}