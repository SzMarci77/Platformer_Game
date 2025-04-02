using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    public int healthAmount = 20;
    public Vector3 spinRotationSpeed = new Vector3(0, 100, 0);

    private void Update()
    {
        transform.eulerAngles += spinRotationSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Damageable damageable = collision.GetComponent<Damageable>();

        if (damageable)
        {
            bool waxHealed = damageable.Heal(healthAmount);

            if (waxHealed)
            {
                Destroy(gameObject);
            }
            
        }
    }


}
