using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button quitButton;

        private void Awake()
        {
            playButton.onClick.AddListener(Play);
            quitButton.onClick.AddListener(Quit);
            Time.timeScale = 1.0f;
        }

        private void Play()
        {
            Loader.Load(Loader.Scene.GameScene);
        }

        private void Quit()
        {
            Application.Quit();
        }
    }
}