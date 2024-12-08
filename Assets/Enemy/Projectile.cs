using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f;           // Verilecek hasar miktar�
    public float lifetime = 5f;         // Merminin ya�am s�resi

    void Start()
    {
        // Belirli bir s�re sonra mermiyi yok et
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // E�er mermi "Player" layer'�na sahip bir nesneye �arparsa
        if (collision.CompareTag("Player"))
        {
            // Oyuncuya hasar ver
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            // Mermiyi yok et
            Destroy(gameObject);
        }
        if (collision.TryGetComponent<TheCar>(out var car))
        {
            car.CartakingDamage(damage);
            
            Destroy(gameObject);
        }
    }
}
