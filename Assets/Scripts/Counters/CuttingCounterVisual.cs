using System;
using UnityEngine;

namespace Counters
{
    public class CuttingCounterVisual : MonoBehaviour
    {
        [SerializeField] private CuttingCounter cuttingCounter;
        private Animator _animator;
        private static readonly int Cut = Animator.StringToHash("Cut");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            cuttingCounter.OnCut += CuttingCounter_OnCut;
        }

        private void CuttingCounter_OnCut(object sender, EventArgs e)
        {
            _animator.SetTrigger(Cut);
        }
    }
}