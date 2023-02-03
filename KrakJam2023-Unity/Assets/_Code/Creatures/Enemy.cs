using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Creature
{
    public int meleeDmg = 0;
    public float meleeRng = 0f;
    public float sightRng = 10f;

    private bool busy;

    private void Die() {
        Destroy(this.gameObject);
    }

    private void TryHitPlayer() {

    }

    private void DecideAction() {
        
    }

    private void GoTo(Vector2 position) {

    }
}
