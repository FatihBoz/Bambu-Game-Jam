using UnityEngine;

public class EnemyHealth : MonoBehaviour
{

    public int maxHealth = 100;

    private int currentHealth;

    void Start()
    {
        // Ba�lang��ta d��man�n can�n� maksimum de�erine ayarla.
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
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
