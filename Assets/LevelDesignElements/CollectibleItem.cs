using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public bool inventoryFull = false; // Envanterin dolu olup olmad���n� kontrol eder
    public int playerLayer; // Oyuncunun layer'�n� buraya tan�mla

    private void Start()
    {
        // Oyuncu layer'�n� bir kez belirle (�rne�in "Player" layer'� 8. s�radaysa bunu Unity'den ayarlamal�s�n)
        playerLayer = LayerMask.NameToLayer("Items");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
