using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : Creature
{
    void Start()
    {
        base.Start();
    }

    protected override void Die() {
        Destroy(gameObject);
    }
}
