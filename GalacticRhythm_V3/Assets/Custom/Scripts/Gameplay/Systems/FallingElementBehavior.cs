using System.Collections;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Pool;


public class FallingElementBehavior : MonoBehaviour
{
    //Initial
    private FallingElement elementData; // Almacena el scriptable asociado
    private ObjectPool<GameObject> pool;
    private IEnumerator effectCoroutine; //Corutina para el manejo de powerups
    
    //Physics
    private Rigidbody2D rb; // Componente de física
    
    //Art
    private SpriteRenderer imageRender; // Render para el sprite del elemento
    private AudioClip soundEffect; // Efecto de sonido
    private Animator animator; // Animacion
    private GameObject visualEffect; // Efecto Visual

    //PowerUp
    private PowerUpManager powerUpManager;
    
    private void OnEnable() 
    {
        // Inicializa el elemento cuando se activa
        if (elementData != null) { Initialize(elementData, pool); }
    }

    public void Initialize(FallingElement data, ObjectPool<GameObject> objectPool)
    {
        rb = GetComponent<Rigidbody2D>();
        imageRender = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        AnimatorController animController = new AnimatorController();


        pool = objectPool; // Almacena la referencia de la pool
        elementData = data; // Guarda la referencia al scriptable
        
        imageRender.sprite = data.elementSprite; // Asigna el sprite del scriptable
        soundEffect = data.elementAudio; // Almacena el efecto de sonido del scriptable
        animController = data.elementAnimatorController;
        animator.runtimeAnimatorController = animController;
        visualEffect = data.elementVisualEffect; // Almacena el efecto visual del scriptable

        effectCoroutine = null;
    }

    private void Start() 
    {
        powerUpManager = GameObject.Find("Player").GetComponent<PowerUpManager>();
        StartCoroutine(DeactivateAfterTime(elementData.lifetime));
    }
    
    private void FixedUpdate() 
    {
        // Aplica la velocidad de caída definida en el scriptable
        rb.velocity = Vector2.down * elementData.fallSpeed;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Realiza las acciones necesarias según el tipo de elemento
        switch (elementData)
        {
            case PowerUpElement powerUp:

                if (other.gameObject.CompareTag("Player"))
                {
                    if(effectCoroutine == null)
                    {
                        effectCoroutine = ApplyEffectsCoroutine(powerUp.effectDuration,
                                                                powerUp.effectMultiplier,
                                                                powerUp.lifeAmount,
                                                                true,
                                                                powerUp.powerUpType);
                        StartCoroutine(effectCoroutine);
                    }
                    else
                    {
                        StopCoroutine(effectCoroutine);
                        effectCoroutine = ApplyEffectsCoroutine(powerUp.effectDuration,
                                                                powerUp.effectMultiplier,
                                                                powerUp.lifeAmount,
                                                                true,
                                                                powerUp.powerUpType);
                        StartCoroutine(effectCoroutine);
                    }

                    //AudioManager.Instance.PlayEffect(soundEffect);
                    Instantiate(visualEffect, transform.position, Quaternion.identity);
                    ReturnToPool();
                }
                break;

            case EnemyElement enemy:

                Life enemyLife = enemy.enemyLife;
                
                if (other.gameObject.CompareTag("Player"))
                {
                    if (other.TryGetComponent(out Life playerLife))
                    {
                        playerLife.TakeDamage(elementData.contactDamage);
                    }
                    //AudioManager.Instance.PlayEffect(soundEffect);
                    Instantiate(visualEffect, transform.position, Quaternion.identity);
                    ReturnToPool();
                }

                break;

            default:
                if (other.gameObject.CompareTag("Player"))
                {
                    if (other.TryGetComponent(out Life playerLife))
                    {
                        playerLife.TakeDamage(elementData.contactDamage);
                        Instantiate(visualEffect, transform.position, Quaternion.identity);
                        ReturnToPool();
                    }
                }
                if (!other.gameObject.CompareTag("Enemy/Minion") || !other.gameObject.CompareTag("Projectile/Enemy"))
                {
                    //AudioManager.Instance.PlayEffect(soundEffect);
                    Instantiate(visualEffect, transform.position, Quaternion.identity);
                    ReturnToPool();
                }
                break;
        }
    }

    private IEnumerator ApplyEffectsCoroutine(float duration,
                                              float multiplier,
                                              int lifeRecover,
                                              bool activeEffect,
                                              PowerUpElement.PowerUpType powerUpType)
    {
        if(powerUpManager != null)
        {
            powerUpManager.ApplyPowerUp(activeEffect, multiplier, lifeRecover,  powerUpType);
        }
        yield return new WaitForSeconds(duration);
        powerUpManager.ApplyPowerUp(false);
    }

    IEnumerator DeactivateAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        ReturnToPool();
    }

    // Función para devolver el objeto a la pool
    private void ReturnToPool()
    {
        if (pool != null)
        {
            pool.Release(gameObject);
        }
        else
        {
            Debug.LogWarning("Pool no asignada para este objeto");
        }
    }
}