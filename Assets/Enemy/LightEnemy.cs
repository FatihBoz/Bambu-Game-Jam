using System.Collections;
using UnityEngine;

public class LightEnemy : MonoBehaviour
{
    public float detectionRange = 10f;     // G�r�� mesafesi
    public float attackRange = 2f;        // Sald�r� mesafesi
    public float moveSpeed = 3f;          // D��man hareket h�z�
    public float attackDelay = 1f;        // Sald�r� gecikmesi
    public float attackDamage = 10f;      // Sald�r� hasar�
    public Vector2 attackAreaSize; // Sald�r� b�lgesinin boyutu

    private bool isAttacking = false;     // Sald�r� yapma durumu

    void Update()
    {
        // D��man g�r�� alan�nda oyuncu var m�?
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, LayerMask.GetMask("Player"));

        if (playerCollider != null && !isAttacking)
        {
            // Oyuncu alg�land�, ona do�ru hareket et
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

        // Sald�r� b�lgesindeki oyuncuyu kontrol et
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, attackAreaSize, 0f, LayerMask.GetMask("Player"));

        foreach (var hitCollider in hitColliders)
        {
            // Oyuncuya hasar ver
            PlayerHealth playerHealth = hitCollider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(attackDamage);
            }
        }

        Debug.Log("Attack!");

        // Sald�r�dan sonra bir s�re bekle
        yield return new WaitForSeconds(attackDelay);

        // Sald�r� durumu s�f�rlan�r
        isAttacking = false;
    }

    // G�r�� alan�n� ve sald�r� b�lgesini g�rselle�tirmek i�in yard�mc� fonksiyon
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, attackAreaSize);
    }
}
