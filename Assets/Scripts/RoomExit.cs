using UnityEngine;

public class RoomExit : MonoBehaviour
{
    private bool exitUnlocked = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Something entered the exit beam: " + other.name); // Debugging

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected in exit beam.");

            if (exitUnlocked)
            {
                Debug.Log("Exit is unlocked, loading next room...");
                FindObjectOfType<RoomManager>().LoadNextRoom();
            }
            else
            {
                Debug.Log("Exit is still locked!");
            }
        }
    }

    // Unlock the exit beam
    public void UnlockExit()
    {
        exitUnlocked = true;
        gameObject.SetActive(true);
        Debug.Log("Exit unlocked!");
    }

    // Hide exit beam initially for combat/tetris rooms
    public void HideExit()
    {
        exitUnlocked = false;
        gameObject.SetActive(false);
    }
}
