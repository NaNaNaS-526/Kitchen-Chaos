using System;
using UnityEngine;

namespace Counters
{
    public class ContainerCounterVisual : MonoBehaviour
    {
        [SerializeField] private ContainerCounter containerCounter;
        private Animator _animator;
        private static readonly int OpenClose = Animator.StringToHash("OpenClose");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            containerCounter.OnPlayerGrabbedObject += ContainerCounter_OnPlayerGrabbedObject;
        }

        private void ContainerCounter_OnPlayerGrabbedObject(object sender, EventArgs e)
        {
            _animator.SetTrigger(OpenClose);
        }
    }
}