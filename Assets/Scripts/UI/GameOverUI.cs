using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GameOverUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI recipesDeliveredText;

        private void Start()
        {
            KitchenGameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
            Hide();
        }

        private void GameManager_OnGameStateChanged(object sender, EventArgs e)
        {
            if (KitchenGameManager.Instance.IsGameOver())
            {
                Show();
                recipesDeliveredText.text = DeliveryManager.Instance.GetSuccessfulRecipesAmount().ToString();
            }
            else
            {
                Hide();
            }
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