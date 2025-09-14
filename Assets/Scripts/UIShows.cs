using UnityEngine;

public class UIShows : MonoBehaviour
{
    [SerializeField] private GameObject uiObject; // Przypisz obiekt UI w Inspectorze
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab; // Klawisz do przełączania

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            if (uiObject != null)
                uiObject.SetActive(!uiObject.activeSelf);
        }
    }
}