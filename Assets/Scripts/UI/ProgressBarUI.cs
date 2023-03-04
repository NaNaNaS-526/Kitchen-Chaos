using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressBarGameObject;
    [SerializeField] private Image barImage;
    private IHasProgressBar _hasProgressBar;

    private void Start()
    {
        _hasProgressBar = hasProgressBarGameObject.GetComponent<IHasProgressBar>();
        if (_hasProgressBar is null)
        {
            Debug.LogError($"GameObject {hasProgressBarGameObject} does not have IHasProgressBar");
        }

        _hasProgressBar.OnProgressChanged += HasProgressBar_OnProgressChanged;
        barImage.fillAmount = 0.0f;
        Hide();
    }

    private void HasProgressBar_OnProgressChanged(object sender, IHasProgressBar.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.ProgressNormalized;
        if (e.ProgressNormalized is 0f or 1)
        {
            Hide();
        }
        else
        {
            Show();
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