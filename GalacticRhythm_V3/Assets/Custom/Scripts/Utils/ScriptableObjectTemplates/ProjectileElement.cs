using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectileBase", menuName = "GalacticRhythm/SpawnElements/ProjectileElement")]
public class ProjectileElement : ElementBase
{
    [Header("Configuraci�n b�sica del proyectil")]
    public int damage = 1;
    [Range(0.1f, 5f)] public float fireRate = 2f;
    [Range(0.1f, 5f)] public float moveSpeed = 2f;
}