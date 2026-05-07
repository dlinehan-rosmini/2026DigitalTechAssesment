using UnityEngine;

public class SoundEffectScript : MonoBehaviour
{
    public AudioClip sound;
    public float volume;
    public float pitch;

    AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.volume= volume;
        source.pitch= pitch;
        source.clip = sound;
        source.Play();
    }

}
