using System.Collections;
using UnityEngine;

public class FreezerEnemy : MonoBehaviour
{
    public float detectionRange = 10f;      // Alg�lama mesafesi
    public float freezeDistance = 1.5f;     // Oyuncu ile durulacak mesafe
    public float moveSpeed = 3f;            // D��man h�z
    public float freezeTime = 2f;           // Oyuncu dondurulacak s�re
    public float attackDistance = 1f;       // Sald�r� mesafesi

    private Transform player;               // Oyuncu referans�
    private Rigidbody2D playerRb;           // Oyuncunun Rigidbody2D'si
    private bool isFreezing = false;        // Oyuncuyu dondurma durumu
    private float freezeTimer = 0f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Oyuncuyu alg�la
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, detectionRange, LayerMask.GetMask("Player"));

        if (playerCollider != null)
        {
            player = playerCollider.transform;
            playerRb = player.GetComponent<Rigidbody2D>();

            // Oyuncuya do�ru gitme yerine belli bir mesafeye yakla�ma
            MoveNearPlayer();

            // Oyuncuya yakla�t���nda durakla
            if (Vector2.Distance(transform.position, player.position) <= freezeDistance)
            {
                StartCoroutine(FreezePlayerIfClose());
            }
        }
    }

    void MoveNearPlayer()
    {

        if (player != null)
        {

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);


            if (distanceToPlayer > freezeDistance)
            {
                animator.SetBool(AnimationKey.Is_Running, true);
                Vector2 direction = (player.position - transform.position).normalized;
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

                if (direction.x != 0)
                {
                    Vector3 scale = transform.localScale;
                    scale.x = direction.x > 0 ? Mathf.Abs(scale.x) : - Mathf.Abs(scale.x);
                    transform.localScale = scale;
                }
            }
        }
    }


    IEnumerator FreezePlayerIfClose()
    {
        // Player'a yak�nsa 2 saniye bekle
        float initialDistance = Vector2.Distance(transform.position, player.position);

        // Duraklamay� ba�lat
        Debug.Log("D��man oyuncuya yak�nla�t�, duraklama ba�l�yor.");

        // S�reyi s�f�rla
        freezeTimer = 0f;

        while (Vector2.Distance(transform.position, player.position) <= freezeDistance)
        {
            freezeTimer += Time.deltaTime;

            // 2 saniye boyunca Player'�n yak�n�nda dur
            if (freezeTimer >= freezeTime)
            {
                if (playerRb != null)
                {
                    // Player'�n Rigidbody2D'sini dondur
                    playerRb.constraints = RigidbodyConstraints2D.FreezeAll;
                    Debug.Log("Oyuncu dondu!");

                    // 2 saniye doldu�unda sald�r� yap
                    AttackPlayer();
                }
                yield break; // 2 saniye dolmu�sa Coroutine'i bitir
            }

            yield return null;
        }

        // E�er Player uzakla��rsa s�resi s�f�rlan�r
        freezeTimer = 0f;
        Debug.Log("Player uzakla�t�, duraklama s�f�rland�.");
    }

    void AttackPlayer()
    {
        // Oyuncu dondurulmu�sa, sald�r�y� ba�lat
        if (playerRb != null)
        {
            Debug.Log("D��man sald�r�yor!");
            // Burada sald�r� i�lemi ger�ekle�ebilir
        }

        // Sald�r� sonras� 2 saniye bekle, sonra dondurmay� kald�r
        StartCoroutine(UnfreezePlayer());
    }

    IEnumerator UnfreezePlayer()
    {
        // 2 saniye sonra oyuncunun dondurulmas�n� kald�r
        yield return new WaitForSeconds(2f);

        if (playerRb != null)
        {
            playerRb.constraints = RigidbodyConstraints2D.None; // Dondurmay� kald�r
            playerRb.constraints = RigidbodyConstraints2D.FreezeRotation; // Yaln�zca rotasyonu sabitle
        }
    }

    // G�rsel yard�m i�in �izim
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange); // Alg�lama alan�
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, freezeDistance); // Donma mesafesi
    }

    private void OnDestroy()
    {
        if (playerRb != null)
        {
            playerRb.constraints = RigidbodyConstraints2D.None; // Dondurmay� kald�r
            playerRb.constraints = RigidbodyConstraints2D.FreezeRotation; // Yaln�zca rotasyonu sabitle
        }
    }
}
