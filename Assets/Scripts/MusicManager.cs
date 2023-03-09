using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string PlayerPrefsMusicVolume = "MusicVolume";
    public static MusicManager Instance { get; private set; }
    private AudioSource _audioSource;
    private float _volume = 0.3f;

    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
        _volume = PlayerPrefs.GetFloat(PlayerPrefsMusicVolume, 0.3f);
        _audioSource.volume = _volume;
    }

    public void ChangeVolume()
    {
        _volume += 0.1f;
        if (_volume > 1.0f)
        {
            _volume = 0.0f;
        }

        _audioSource.volume = _volume;
        PlayerPrefs.SetFloat(PlayerPrefsMusicVolume, _volume);
    }

    public float GetVolume()
    {
        return _volume;
    }
}