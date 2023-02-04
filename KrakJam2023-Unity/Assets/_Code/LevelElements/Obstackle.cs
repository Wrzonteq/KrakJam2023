using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstackle : MonoBehaviour
{
    [SerializeField]
    public int physicalDamage;
    public int magicallDamage;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")){
            Debug.Log(collision.gameObject.tag);
        }
    }
}
