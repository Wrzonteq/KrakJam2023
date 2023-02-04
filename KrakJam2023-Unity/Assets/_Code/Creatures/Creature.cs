using UnityEngine;

public abstract class Creature : MonoBehaviour {
    [SerializeField] int maxHealth;

    public int MaxHealth => maxHealth;

    public int CurrentHealth { get; protected set; }

    public void DealDamage(int dmg) {
        CurrentHealth -= dmg;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
        if (CurrentHealth <= 0)
            Die();
    }

    protected virtual void AddHealth(int value) {
        CurrentHealth += value;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
    }

    protected void Start() {
        CurrentHealth = MaxHealth;
    }
    protected abstract void Die();
}
