using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public int HP_max;
    public int HP_current;

    public void GetDmg(int dmg) {
        if ((HP_current -= dmg) <= 0)
            Die();
    }
    private void Die() {
        
    }
}
