using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GamePauseUI : MonoBehaviour
    {
        [SerializeField] private Button menuButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button optionsButton;

        private void Awake()
        {
            menuButton.onClick.AddListener(() => { Loader.Load(Loader.Scene.MainMenuScene); });
            resumeButton.onClick.AddListener(() => { KitchenGameManager.Instance.TogglePauseGame(); });
            optionsButton.onClick.AddListener(() =>
            {
                OptionsUI.Instance.Show();
            });
        }

        private void Start()
        {
            KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
            KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
            Hide();
        }

        private void KitchenGameManager_OnGamePaused(object sender, EventArgs e)
        {
            Show();
        }

        private void KitchenGameManager_OnGameUnpaused(object sender, EventArgs e)
        {
            Hide();
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