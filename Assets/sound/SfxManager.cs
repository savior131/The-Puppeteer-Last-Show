using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance;
    [SerializeField] private AudioSource _audioSource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void playSound(AudioClip audio, Transform transform,float volume)
    {
        AudioSource audioSource = Instantiate(_audioSource,transform.position,Quaternion.identity);

        audioSource.clip = audio;

        audioSource.volume = volume;

        audioSource.Play();

        float length=audioSource.clip.length;

        Destroy(audioSource.gameObject,length);


    }

    public void playSoundRandom(AudioClip[] audio, Transform transform, float volume)
    {
        AudioSource audioSource = Instantiate(_audioSource, transform.position, Quaternion.identity);
        int rand=Random.Range(0, audio.Length); 

        audioSource.clip = audio[rand];

        audioSource.volume = volume;

        audioSource.Play();

        float length = audioSource.clip.length;

        Destroy(audioSource.gameObject, length);


    }
}
