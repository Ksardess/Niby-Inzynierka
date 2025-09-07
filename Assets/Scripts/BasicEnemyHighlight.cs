using UnityEngine;

public class BasicEnemyHighlight : MonoBehaviour
{
    [SerializeField] private GameObject highlightObject;

    private void OnMouseEnter()
    {
        if (highlightObject != null)
            highlightObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (highlightObject != null)
            highlightObject.SetActive(false);
    }
}