using UnityEngine;

public class BackgroundSwitcher : MonoBehaviour
{
    public GameObject dayBackground; // GameObject for daytime background
    public GameObject nightBackground; // GameObject for nighttime background

    void Start()
    {
        // if (dayBackground == null || nightBackground == null)
        // {
        //     Debug.LogError("Background GameObjects are not assigned.");
        //     return;
        // }
        dayBackground.SetActive(true);
        nightBackground.SetActive(false);
        Invoke("ChangeToNight", 39f);
    }

    void ChangeToNight()
    {
        dayBackground.SetActive(false);
        nightBackground.SetActive(true);
        Invoke("ChangeToDay", 33f); // 56 - 39 = 17 seconds later
    }

    void ChangeToDay()
    {
        dayBackground.SetActive(true);
        nightBackground.SetActive(false);
        Invoke("ChangeToNight", 39f); // Restart cycle
    }
}