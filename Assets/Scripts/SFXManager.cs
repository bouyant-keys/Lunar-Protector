using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public AudioClip _jump;
    public AudioClip _asteroidHit;
    public AudioClip _dishInDanger;
    public AudioClip _baseHit;
    public AudioClip _typing;
    public AudioClip _firingLaser;

    AudioSource _audioSource;
    [SerializeField] AudioSource _playerAudioSource;

    private void Awake()
    {
        _audioSource = this.GetComponent<AudioSource>();
    }

    public void PlayClip(AudioClip audio)
    {
        _audioSource.PlayOneShot(audio);
    }

    //For laser effect
    public void PlayLongClip(AudioClip audio)
    {
        _audioSource.clip = audio;
        _audioSource.Play();
        _audioSource.loop = true;
    }

    public void Stop()
    {
        _audioSource.Stop();
        _audioSource.loop = false;
    }

    //For typing effect
    public void PlayPlayerClip()
    {
        _playerAudioSource.Play();
        _playerAudioSource.loop = true;
    }

    public void StopPlayerClip()
    {
        _playerAudioSource.Stop();
        _playerAudioSource.loop = false;
    }
}
