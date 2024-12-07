using UnityEngine;

public class TriggerSystem : MonoBehaviour
{
    public GameObject uiElement; // UI panelini buraya ba�lay�n
    //public GameObject playerCar; // Oyuncu arabas�n� buraya ba�lay�n


    bool isPlayerInTrigger = false; // Oyuncunun trigger i�inde olup olmad���n� takip eder

    void Start()
    {
        // UI ba�lang��ta kapal�
        if (uiElement != null)
            uiElement.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 8) // Oyuncuyu tespit etmek i�in Tag kontrol�
        {
            isPlayerInTrigger = true;
            if (uiElement != null)
                uiElement.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 8)
        {
            isPlayerInTrigger = false;
            if (uiElement != null)
                uiElement.SetActive(false);
        }
    }


}
