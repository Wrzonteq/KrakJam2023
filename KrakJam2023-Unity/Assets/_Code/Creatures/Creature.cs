using UnityEngine;

public abstract class Creature : MonoBehaviour {
    [SerializeField] int maxHealth;


    public int MaxHealth => maxHealth;
    public int CurrentHealth { get; private set; }


    public void DealDamage(int dmg) {
        if ((CurrentHealth -= dmg) <= 0)
            Die();
    }

    protected virtual void AddHealth(int value) {
        CurrentHealth += value;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
    }

    protected abstract void Die();
}
