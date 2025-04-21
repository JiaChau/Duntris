using UnityEngine;
using TMPro;

public class HealthGainPopupUI : MonoBehaviour
{
    public TMP_Text popupText;
    public float duration = 2f;

    private void Awake()
    {
        popupText.gameObject.SetActive(false);
    }

    public void ShowHealAmount(int amount)
    {
        popupText.text = $"+{amount} HP";
        popupText.gameObject.SetActive(true);

        CancelInvoke(nameof(Hide));
        Invoke(nameof(Hide), duration);
    }

    private void Hide()
    {
        popupText.gameObject.SetActive(false);
    }
}
