using UnityEngine;

public class PanelButtonOpener : MonoBehaviour
{
    [Tooltip("GameObject panelu, który ma być otwarty po kliknięciu")]
    [SerializeField] private GameObject targetPanelObject;

    [Tooltip("Opcjonalny kontener (parent) zawierający wszystkie panele. Jeśli null użyty zostanie parent targetPanelObject.")]
    [SerializeField] private Transform groupParent;

    // Jeśli chcesz użyć UIShows.Show() zamiast bezpośredniego SetActive, możesz przypisać UIShows tutaj.
    [SerializeField] private UIShows targetUIShows;

    public void OpenTarget()
    {
        if (targetPanelObject == null && targetUIShows == null) return;

        // preferuj UIShows jeśli jest przypisany
        GameObject target = (targetUIShows != null) ? targetUIShows.gameObject : targetPanelObject;
        if (target == null) return;

        // ustal parent grupy paneli
        Transform parent = groupParent != null ? groupParent : (target.transform.parent);

        // wyłącz wszystkie inne panele w grupie
        if (parent != null)
        {
            foreach (Transform child in parent)
            {
                if (child.gameObject == target) continue;
                if (child.gameObject.activeSelf)
                    child.gameObject.SetActive(false);
            }
        }

        // włącz docelowy panel (zawsze włączamy — przycisk ma aktywować dany obiekt)
        if (targetUIShows != null)
            targetUIShows.Show();
        else
            target.SetActive(true);
    }
}