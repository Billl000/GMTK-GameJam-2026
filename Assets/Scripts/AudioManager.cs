using UnityEngine;
using UnityEngine.Audio;
public class AudioManager : MonoBehaviour
{
    public Sound[] audioClips;
    private void Awake()
    {
        foreach(Sound s in audioClips)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string soundName)
    {
        Sound s = System.Array.Find(audioClips, sound => sound.name == soundName);
        if (s != null)
        {
            s.source.Play();
        }
    }
}
