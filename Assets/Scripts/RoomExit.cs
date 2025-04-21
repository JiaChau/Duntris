using UnityEngine;

public class RoomExit : MonoBehaviour
{
    private bool exitUnlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && exitUnlocked)
        {
            RoomManager.floorIndex++; // Always increment floorIndex first

            if (RoomManager.floorIndex >= 30)
            {
                var finishUI = FindObjectOfType<FinishGameUI>();
                if (finishUI != null)
                {
                    finishUI.ShowFinishScreen();
                    Debug.Log("[ROOMEXIT] Game completed. Showing finish screen.");
                    return;
                }
                else
                {
                    Debug.LogWarning("[ROOMEXIT] FinishGameUI not found.");
                }
            }

            FindObjectOfType<RoomManager>()?.LoadNextRoom();
        }
    }

    public void UnlockExit()
    {
        exitUnlocked = true;
        gameObject.SetActive(true);
        Debug.Log("Exit unlocked!");
    }

    public void HideExit()
    {
        exitUnlocked = false;
        gameObject.SetActive(false);
    }
}
