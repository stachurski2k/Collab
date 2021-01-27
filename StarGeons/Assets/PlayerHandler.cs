using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{

    Health PlayerHealth;

    void Start()
    {
        PlayerHealth = GetComponent<Health>();
    }


    void Update()
    {
        lifeCheck();
    }

    private void lifeCheck()
    {
        if (PlayerHealth.GetHealPoints() <= 0)
        {
            // TODO Death effects (Vfx, SfX, etc...)
            // TODO end Level
            Destroy(gameObject);
        }
    }

    public void playerTakeHit(int damageTaken)
    {
        // TODO Death effects (Vfx, SfX, etc...)
        print(1);
        PlayerHealth.takeHeal(damageTaken);
    }
}
