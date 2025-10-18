using UnityEngine;

public class UIShows : MonoBehaviour
{
    [SerializeField] private GameObject uiObject; // Przypisz obiekt UI w Inspectorze
    [SerializeField] private KeyCode toggleKey = KeyCode.Tab; // Klawisz do przełączania

    // Jeśli ustawione — będzie użyte jako kontener wszystkich paneli (sąsiadów).
    // Jeśli null — użyty zostanie parent przypisanego uiObject.
    [SerializeField] private Transform groupParent;

    void Update()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        if (uiObject == null) return;

        bool willActivate = !uiObject.activeSelf;

        if (willActivate)
        {
            // wyłącz wszystkie inne panele w tej samej grupie
            Transform parent = groupParent != null ? groupParent : uiObject.transform.parent;
            if (parent != null)
            {
                foreach (Transform child in parent)
                {
                    if (child.gameObject != uiObject)
                        child.gameObject.SetActive(false);
                }
            }
        }

        uiObject.SetActive(willActivate);
    }

    // metody pomocnicze — otwórz panel (i zamknij inne) / zamknij
    public void Show()
    {
        if (uiObject == null) return;
        Transform parent = groupParent != null ? groupParent : uiObject.transform.parent;
        if (parent != null)
        {
            foreach (Transform child in parent)
            {
                if (child.gameObject != uiObject)
                    child.gameObject.SetActive(false);
            }
        }
        uiObject.SetActive(true);
    }

    public void Hide()
    {
        if (uiObject == null) return;
        uiObject.SetActive(false);
    }
}