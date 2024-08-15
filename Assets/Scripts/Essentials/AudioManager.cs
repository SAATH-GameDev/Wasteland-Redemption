using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public float stopDuration = 3.0f;
    [Space]
    public AudioClip start;
    public AudioClip loop;
    [Space]
    public bool played = false;
    public bool stopping = false;

    private AudioSource audioSource;

    static public AudioManager Instance;

    void Awake()
    {
        Instance = this;

        audioSource = GetComponent<AudioSource>();
    }

    void Play(AudioClip start, AudioClip loop)
    {
        if(!start || !loop)
            return;

        if(!played && !stopping)
        {
            audioSource.PlayOneShot(start);

            audioSource.clip = loop;
            audioSource.loop = true;
            audioSource.PlayDelayed(start.length - 0.02f);

            played = true;
        }
    }

    void Update()
    {
        Play(start, loop);

        if(stopping)
        {
            audioSource.volume -= Time.unscaledDeltaTime / stopDuration;

            if(audioSource.volume <= 0.0f)
            {
                played = false;
                audioSource.Stop();
                audioSource.volume = 1.0f;

                audioSource.clip = start = loop = null;

                stopping = false;
            }
        }
    }
}
