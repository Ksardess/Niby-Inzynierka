using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Obiekt, za którym kamera ma podążać
    public Vector3 offset;   // Opcjonalne przesunięcie kamery względem obiektu

    void Start()
    {
        // Ustaw wartość Z w offset na -10
        offset.z = -10;
    }

    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}