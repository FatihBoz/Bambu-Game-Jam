using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    //public float desiredDistance = 5f;     // Oyuncuya korunacak mesafe
    public float moveSpeed = 3f;           // D��man hareket h�z�
    public float maxDistance = 7f;         // Oyuncuya yakla�mas� gereken en uzak mesafe
    public float minDistance = 3f;         // Oyuncudan uzakla�mas� gereken en yak�n mesafe
    public float fireDistance = 4f;        // Ate� etme mesafesi
    public float detectionRange = 10f;     // D��man�n oyuncuyu alg�layaca�� mesafe
    public float attackDelay = 1f;         // Sald�r� gecikmesi

    private bool isAttacking = false;      // Sald�r� durumu

    void Update()
    {
        // Oyuncuyu alg�la
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, LayerMask.GetMask("Player"));

        if (playerCollider != null)
        {
            Transform player = playerCollider.transform;
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Oyuncuya olan mesafeye g�re hareket et
            if (distanceToPlayer > fireDistance)
            {
                // Oyuncudan uzaksa yakla�
                MoveTowardsPlayer(player);
            }

            // E�er ate� mesafesindeyse ate� et
            if (distanceToPlayer <= fireDistance && !isAttacking)
            {
                AttackPlayer();
            }
        }
    }

    void MoveTowardsPlayer(Transform player)
    {
        // Oyuncuya do�ru hareket et
        Vector2 direction = (player.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }


    void AttackPlayer()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;

        // Ate� etme animasyonu veya etkile�imi buraya ekleyebilirsiniz
        Debug.Log("Firing!");

        // Sald�r�dan sonra bir s�re bekle
        yield return new WaitForSeconds(attackDelay);

        // Sald�r� durumu s�f�rlan�r
        isAttacking = false;
    }

    // G�r�� alan�n� g�rsel olarak g�stermek i�in yard�mc� bir fonksiyon (editor'de g�rsel g�sterim i�in)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, fireDistance);
    }
}
