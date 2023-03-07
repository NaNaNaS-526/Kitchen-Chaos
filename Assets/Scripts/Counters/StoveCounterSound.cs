using System;
using UnityEngine;

namespace Counters
{
    public class StoveCounterSound : MonoBehaviour
    {
        [SerializeField] private StoveCounter stoveCounter;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        }

        private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
        {
            bool playSound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
            if (playSound)
            {
                _audioSource.Play();
            }
            else
            {
                _audioSource.Pause();
            }
        }
    }
}