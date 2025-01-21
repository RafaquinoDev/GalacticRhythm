using System;
using UnityEngine;

public class Life : MonoBehaviour
{
    //Life settings
    public int maxLife;
    [HideInInspector] public int currentLife;

    //Eventos
    public event EventHandler OnLiveLoss;


    void Start()
    {
        currentLife = maxLife;
    }
    public void TakeDamage(int damage)
    {
        int temporaryLife = currentLife - damage;
        
        if(temporaryLife < 0)
        {
            currentLife = 0;
        }
        else
        {
            currentLife = temporaryLife;
        }

        if(currentLife <= 0)
        {
            OnLiveLoss?.Invoke(this, EventArgs.Empty);
            this.gameObject.SetActive(false);
        }
    }

    public void Heal(int cure)
    {
        int temporaryLife = currentLife + cure;
        
        if(temporaryLife > maxLife)
        {
            currentLife = maxLife;
        }
        else
        {
            currentLife = temporaryLife;
        }
    }
}