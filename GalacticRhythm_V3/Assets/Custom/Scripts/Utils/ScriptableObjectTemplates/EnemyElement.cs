using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyElement", menuName = "GalacticRhythm/SpawnElements/FallingElement/Enemy")]
public class EnemyElement : FallingElement
{
    [Header("Configuraci�n de Enemigos")]
    public Life enemyLife;
}