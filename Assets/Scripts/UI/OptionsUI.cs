using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class OptionsUI : MonoBehaviour
    {
        public static OptionsUI Instance { get; private set; }
        [SerializeField] private Button soundEffectsButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button closeButton;
        [SerializeField] private TextMeshProUGUI soundEffectsText;
        [SerializeField] private TextMeshProUGUI musicText;

        [SerializeField] private Button moveUpButton;
        [SerializeField] private Button moveDownButton;
        [SerializeField] private Button moveLeftButton;
        [SerializeField] private Button moveRightButton;
        [SerializeField] private Button interactButton;
        [SerializeField] private Button interactAlternateButton;
        [SerializeField] private Button pauseButton;

        [SerializeField] private TextMeshProUGUI moveUpText;
        [SerializeField] private TextMeshProUGUI moveDownText;
        [SerializeField] private TextMeshProUGUI moveLeftText;
        [SerializeField] private TextMeshProUGUI moveRightText;
        [SerializeField] private TextMeshProUGUI interactText;
        [SerializeField] private TextMeshProUGUI interactAlternateText;
        [SerializeField] private TextMeshProUGUI pauseText;

        [SerializeField] private Transform pressKeyToRebindTransform;

        private void Awake()
        {
            Instance = this;
            soundEffectsButton.onClick.AddListener(() =>
            {
                SoundManager.Instance.ChangeVolume();
                UpdateVisual();
            });
            musicButton.onClick.AddListener(() =>
            {
                MusicManager.Instance.ChangeVolume();
                UpdateVisual();
            });
            closeButton.onClick.AddListener(Hide);

            moveUpButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveUp));
            moveDownButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveDown));
            moveLeftButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveLeft));
            moveRightButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.MoveRight));
            interactButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Interact));
            interactAlternateButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.InteractAlternate));
            pauseButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Pause ));
        }

        private void Start()
        {
            KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;
            UpdateVisual();
            Hide();
            HidePressKeyToRebind();
        }

        private void KitchenGameManager_OnGameUnpaused(object sender, EventArgs e)
        {
            Hide();
        }

        private void UpdateVisual()
        {
            soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.GetVolume() * 10.0f);
            musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.GetVolume() * 10.0f);

            moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveUp);
            moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveDown);
            moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveLeft);
            moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.MoveRight);
            interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
            interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.InteractAlternate);
            pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            gameObject.SetActive(false);
        }

        private void ShowPressKeyToRebind()
        {
            pressKeyToRebindTransform.gameObject.SetActive(true);
        }

        private void HidePressKeyToRebind()
        {
            pressKeyToRebindTransform.gameObject.SetActive(false);
        }

        private void RebindBinding(GameInput.Binding binding)
        {
            ShowPressKeyToRebind();
            GameInput.Instance.RebindBinding(binding, () =>
            {
                HidePressKeyToRebind();
                UpdateVisual();
            });
        }
    }
}