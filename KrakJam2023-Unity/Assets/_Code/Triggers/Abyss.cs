using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abyss : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        collision.GetComponent<Creature>().DealDamage(1000);
    }
}
