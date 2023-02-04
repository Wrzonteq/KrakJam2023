using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : Creature
{
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Die() {
        Destroy(gameObject);
    }
}
