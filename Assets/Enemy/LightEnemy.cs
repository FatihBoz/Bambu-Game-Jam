using System.Collections;
using UnityEngine;

public class LightEnemy : MonoBehaviour
{
    public float detectionRange = 10f;     // G�r�� mesafesi
    public float attackRange = 2f;         // Sald�r� mesafesi
    public float moveSpeed = 3f;           // D��man h�z
    public float attackDelay = 1f;         // Sald�r� gecikmesi

    private bool isAttacking = false;      // Sald�r� yapma durumu

    void Update()
    {
        // D��man g�r�� alan�nda oyuncu var m�?
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, LayerMask.GetMask("Player"));

        if (playerCollider != null && !isAttacking)
        {
            // Oyuncu alg�land�, ona do�ru git
            MoveTowardsPlayer(playerCollider.transform);

            // Oyuncu sald�r� alan�nda m�?
            if (Vector2.Distance(transform.position, playerCollider.transform.position) <= attackRange)
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

        // Sald�r� animasyonu veya etkile�imi buraya ekleyebilirsiniz
        Debug.Log("Attack!");

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
    }
}
