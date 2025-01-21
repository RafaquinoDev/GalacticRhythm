using System;
using UnityEngine;

public class Die : MonoBehaviour
{
    [SerializeField] private GameObject dieVisualEffect;
    private Life life;

    private void Start() 
    {
        life = GetComponent<Life>();
        if(life != null) life.OnLiveLoss += DieEffect;
    }

    private void DieEffect(object sender, EventArgs e)
    {
        Instantiate(dieVisualEffect, transform.position, Quaternion.identity);
        life.OnLiveLoss -= DieEffect;
    }
}