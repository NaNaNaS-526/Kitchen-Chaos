using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    [SerializeField] private float volume = 1.0f;
    private Player _player;
    private float _footStepTimer;
    private float _footStepTimerMax;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        _footStepTimer -= Time.deltaTime;
        if (_footStepTimer < 0.0f)
        {
            _footStepTimer = _footStepTimerMax;
            if (_player.IsWalking())
            {
                SoundManager.Instance.PlayFootstepSound(_player.transform.position, volume);
            }
        }
    }
}