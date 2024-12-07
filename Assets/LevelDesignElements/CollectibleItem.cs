using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public bool inventoryFull = false; // Envanterin dolu olup olmad���n� kontrol eder


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �arp��an nesne "Player" layer'�ndaysa ve envanter bo�sa
        if (collision.gameObject.layer == 15 && !inventoryFull)
        {
            inventoryFull = true; // Envanteri dolu olarak i�aretle
            Debug.Log("Item topland�. Envanter dolu!");

            // Bu itemi sahneden kald�r
            Destroy(gameObject);
        }
    }
}
