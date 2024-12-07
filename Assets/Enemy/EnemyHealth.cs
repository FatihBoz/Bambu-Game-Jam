using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public float maxHealth = 100;

    private float currentHealth;

    void Start()
    {
        // Ba�lang��ta d��man�n can�n� maksimum de�erine ayarla.
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        // Hasar al�nd���nda can� d���r.
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} has {currentHealth} health remaining.");


        // Can s�f�r veya daha azsa d��man� �ld�r.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log($"{gameObject.name} has died!");
        // D��man� sahneden kald�r.
        Destroy(gameObject);
    }
}
