using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    private bool collectable = false;
    private GameObject playerObject;

    [SerializeField] private Sprite itemSprite; // �temin sprite'� (G�rseli)

    private void Start()
    {
        itemSprite = GetComponent<Sprite>();    
    }

    private void Update()
    {
        if (collectable && Input.GetKeyDown(KeyCode.E) && !PlayerInventory.instance.Inventory)
        {
            if (playerObject != null)
            {
                PlayerInventory.instance.Inventory = true;
                PlayerInventory.instance.InventoryUIImage.sprite = itemSprite; // G�rseli UI'ye ata
                Debug.Log("�tem al�nd� ve UI g�ncellendi");
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player")) // Oyuncu layer'�n� kontrol et
        {
            Debug.Log("Oyuncu yak�nla�t�");
            playerObject = other.gameObject;
            collectable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            playerObject = null;
            collectable = false;
        }
    }
}
