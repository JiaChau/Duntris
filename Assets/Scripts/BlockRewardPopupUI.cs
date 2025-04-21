using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasGroup))]
public class BlockRewardPopupUI : MonoBehaviour
{
    public TMP_Text popupText;
    public float displayDuration = 2f;

    private Queue<string> rewardQueue = new Queue<string>();
    private bool isDisplaying = false;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        if (popupText != null)
        {
            popupText.raycastTarget = false;
            popupText.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Add multiple rewards to the queue and start showing them one by one.
    /// </summary>
    public void ShowRewards(List<(string blockName, int amount)> rewards)
    {
        foreach (var reward in rewards)
        {
            rewardQueue.Enqueue($"+{reward.amount} {reward.blockName}");
        }

        if (!isDisplaying)
        {
            StartCoroutine(DisplayRewards());
        }
    }

    private IEnumerator DisplayRewards()
    {
        isDisplaying = true;

        while (rewardQueue.Count > 0)
        {
            string message = rewardQueue.Dequeue();
            popupText.text = message;
            popupText.gameObject.SetActive(true);

            yield return new WaitForSeconds(displayDuration);

            popupText.gameObject.SetActive(false);
        }

        isDisplaying = false;
    }
}
