using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private Life playerLife;
    private ShootManager shootManager;

    void Start()
    {
        playerLife = GetComponent<Life>();
        shootManager = GetComponent<ShootManager>();
    }

    public void ApplyPowerUp(bool isActive = true,
                             float multiplier = 1,
                             int lifeAmount = 0,
                             PowerUpElement.PowerUpType powerUpType = PowerUpElement.PowerUpType.moreFireRate)
    {
        if(powerUpType == PowerUpElement.PowerUpType.moreLife)
        {
            playerLife.Heal(lifeAmount);
            Debug.Log("Life +1");
        }

        if(powerUpType == PowerUpElement.PowerUpType.moreFireRate)
        {
            if(isActive){ shootManager.NextShotCalculate(multiplier); Debug.Log("Rate++");}
            else { shootManager.NextShotCalculate(); Debug.Log("Rate: normal"); }
        }
    }
}