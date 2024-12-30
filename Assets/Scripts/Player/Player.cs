using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHelth = 100;
    public int currentHelth;

    public HelthBar helthBar;

    void Start()
    {
        if (helthBar == null)
        {
            helthBar = GetComponentInChildren<HelthBar>();
        }

        currentHelth = maxHelth;
        helthBar.SetMaxHelth(maxHelth);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //     TakeDamage(20);
        //}
    }

    public void TakeDamage(int damage)
    {
        currentHelth -= damage;
        Debug.Log("Otrzymałeś obrażenia: " + damage);

        helthBar.SetHelth(currentHelth);
    }
}