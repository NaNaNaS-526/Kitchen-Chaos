using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GamePlayingClockUI : MonoBehaviour
    {
        [SerializeField] private Image timerImage;

        private void Update()
        {
            timerImage.fillAmount = KitchenGameManager.Instance.GetPlayingTimerNormalized();
        }
    }
}