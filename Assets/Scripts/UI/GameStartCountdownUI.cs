using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameStartCountdownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI countdownText;

        private void Start()
        {
            KitchenGameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
            Hide();
        }

        private void GameManager_OnGameStateChanged(object sender, EventArgs e)
        {
            if (KitchenGameManager.Instance.IsCountdownToStartActive())
            {
                Show();
            }
            else
            {
                Hide();
            }
        }

        private void Update()
        {
            countdownText.text = Mathf.Ceil(KitchenGameManager.Instance.GetCountdownToStartTimer()).ToString();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}