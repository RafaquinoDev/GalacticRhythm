using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "NewElement", menuName = "GalacticRhythm/SpawnElements/Base")]
public class ElementBase : ScriptableObject
{
    [Header("Configuraci�n B�sica")]
    //Initial
    public string elementName;
    public float lifetime;
    //Art
    public Sprite elementSprite;
    public AudioClip elementAudio;
    public AnimatorController elementAnimatorController;
    public GameObject elementVisualEffect;
}