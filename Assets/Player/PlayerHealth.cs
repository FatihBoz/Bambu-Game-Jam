using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider slider;
    public float maxHealth = 100f;       // Oyuncunun maksimum sa�l���
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;      // Oyuncunun sa�l��� ba�lang��ta maksimum
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log("Player Health: " + currentHealth);
        slider.value = currentHealth;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        // Oyuncu �l�m� burada i�lenir (�rne�in, oyun sonu ekran�, yeniden ba�latma vb.)
    }
}
