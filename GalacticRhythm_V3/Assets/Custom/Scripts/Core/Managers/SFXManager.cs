using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        if(SFXManager.Instance == null)
        {
            SFXManager.Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        sfxSource = GetComponent<AudioSource>();
    }

    public void PlayEffect(AudioClip soundEffect){ sfxSource.PlayOneShot(soundEffect); }
}
