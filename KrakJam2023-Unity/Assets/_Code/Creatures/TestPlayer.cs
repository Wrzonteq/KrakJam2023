using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : Creature
{
    [SerializeField] private Color startColor;
    [SerializeField] private Color endColor;
    private SpriteRenderer rend;
    
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        //Debug.Log(gameObject.TryGetComponent<SpriteRenderer>(out rend));
        //rend.color = Color.Lerp(startColor, endColor, 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        //rend.color = Color.Lerp(startColor, endColor, 0.5f);
        //Debug.Log(((float)CurrentHealth - 1f) / ((float)MaxHealth - 1));
    }

    protected override void Die() {
        Destroy(gameObject);
    }
}
