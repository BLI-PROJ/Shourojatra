using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 100;
    [SerializeField] int playerDamage = 10;


    public int GetDamage()
    {
        return damage;
    }

    public int GetPlayerDamage()
    {
        return playerDamage;
    }

    public void Hit()
    {
        Destroy(gameObject);
    }
}
