using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    public Rigidbody2D carRigidbody; // Arac�n Rigidbody2D bile�eni
    public float rotationMultiplier = 10f; // D�n�� h�z�n� kontrol eden �arpan

    private void LateUpdate()
    {
        // Arac�n h�z�n� al
        float carSpeed = carRigidbody.velocity.x;

        // D�n�� h�z�n� hesapla
        float rotationSpeed = carSpeed * rotationMultiplier;



        // Tekerle�i d�nd�r
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
}
