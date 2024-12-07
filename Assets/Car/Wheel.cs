using UnityEngine;

public class Wheel : MonoBehaviour
{
    public Rigidbody2D carRigidbody; // Arac�n Rigidbody2D bile�eni
    public float rotationMultiplier = 10f; // D�n�� h�z�n� kontrol eden �arpan

    public float suspensionStrength = 1000f; // S�spansiyonun yay kuvveti
    public float suspensionDamping = 50f; // Amortis�r kuvveti
    public float maxSuspensionHeight = 1f; // S�spansiyonun maksimum s�k��ma mesafesi
    public float wheelRadius = 0.5f; // Tekerlek yar��ap�
    public LayerMask groundLayer; // Zemin katman�

    private float suspensionCompression = 0f; // S�spansiyonun s�k��ma miktar�
    private float previousHeight = 0f; // �nceki yerden y�kseklik de�eri

    private void LateUpdate()
    {
        // Arac�n h�z�n� al
        float carSpeed = carRigidbody.velocity.x;

        // D�n�� h�z�n� hesapla
        float rotationSpeed = carSpeed * rotationMultiplier;

        // Tekerle�i d�nd�r
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);

        // S�spansiyon sistemini hesapla
        HandleSuspension();
    }

    void HandleSuspension()
    {
        // Zeminle olan mesafeyi �l�mek i�in raycast kullan
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, wheelRadius + 0.5f, groundLayer);
        if (hit.collider != null)
        {
            float currentHeight = hit.distance; // Zeminle aras�ndaki mesafe

            // S�spansiyonun s�k��ma miktar�n� hesapla
            float compression = Mathf.Clamp01((currentHeight - wheelRadius) / maxSuspensionHeight);
            suspensionCompression = Mathf.Lerp(suspensionCompression, compression, Time.deltaTime * suspensionDamping);

            // S�spansiyon kuvveti uygula (yay kuvv.eti)
            float springForce = suspensionStrength * suspensionCompression;
            Vector2 suspensionForce = new Vector2(0, -springForce);

            // Amortis�r kuvvetini uygula (s�k��ma ile h�z�n �arp�m�)
            Vector2 dampingForce = new Vector2(0, -suspensionDamping * (carRigidbody.velocity.y));

            // Uygulanan toplam kuvveti arac�n Rigidbody2D'sine ekle
            carRigidbody.AddForce(suspensionForce + dampingForce);
        }
    }
}
