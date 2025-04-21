using UnityEngine;
using TMPro;

public class BuffStatsUI : MonoBehaviour
{
    [Header("Stat Display")]
    public GameObject statPanelGroup;       // Parent group that contains background + stats text
    public TMP_Text statsText;              // Reference to the text component showing stats

    [Header("Buff Popup")]
    public TMP_Text buffPopupText;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        // Setup CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        // Make all children ignore raycasts
        if (statsText != null) statsText.raycastTarget = false;
        if (buffPopupText != null) buffPopupText.raycastTarget = false;
    }

    private void Start()
    {
        if (statPanelGroup != null)
            statPanelGroup.SetActive(false);

        if (buffPopupText != null)
            buffPopupText.gameObject.SetActive(false);
    }

    public void ShowStatsPanel(bool show)
    {
        if (statPanelGroup != null)
            statPanelGroup.SetActive(show);
    }

    public void UpdateStats(PlayerStats stats)
    {
        if (statsText == null || stats == null) return;

        statsText.text =
            "HP: " + stats.maxHealth +
            "\nMelee Dmg: " + stats.meleeDamage +
            "\nRanged Dmg: " + stats.rangedDamage +
            "\nMove Speed: " + stats.moveSpeed +
            "\nMelee Range: " + stats.meleeRange +
            "\nProjectile Speed: " + stats.projectileSpeed;
    }

    public void FlashBuff(string buffDescription)
    {
        StopAllCoroutines();
        StartCoroutine(Flash(buffDescription));
    }

    private System.Collections.IEnumerator Flash(string buffText)
    {
        if (buffPopupText != null)
        {
            buffPopupText.text = buffText;
            buffPopupText.gameObject.SetActive(true);

            yield return new WaitForSeconds(1.5f);

            buffPopupText.gameObject.SetActive(false);
        }
    }
}
