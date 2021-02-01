using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;

    int healPoints;

    void Start()
    {
        healPoints = maxHealth;
    }
    
    public int GetHealPoints()
    {
        return healPoints;
    }

    public void giveheal(int healToGive)
    {
        healPoints += healToGive;
    }

    public void takeHeal(int healToTake)
    {
        healPoints -= healToTake;
    }
}
