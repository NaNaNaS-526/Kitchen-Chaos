using UnityEngine;

namespace Counters
{
    public class SelectedCounterVisual : MonoBehaviour
    {
        [SerializeField] private BaseCounter baseCounter;
        [SerializeField] private GameObject[] visualGameObjectArray;

        private void Start()
        {
            Player.Instance.OnSelectedCounterChanged += PlayerOnSelectedCounterChanged;
        }

        private void PlayerOnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
        {
            if (e.SelectedCounter == baseCounter)
            {
                ShowVisual();
            }
            else
            {
                HideVisual();
            }
        }

        private void ShowVisual()
        {
            foreach (var visual in visualGameObjectArray)
            {
                visual.SetActive(true);
            }
        }

        private void HideVisual()
        {
            foreach (var visual in visualGameObjectArray)
            {
                visual.SetActive(false);
            }
        }
    }
}