using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float parallaxSpeed = 0.5f;  // Katman h�z�n� ayarlamak i�in
    private float length, startPos;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (Camera.main.transform.position.x * (1 - parallaxSpeed));
        float distance = (Camera.main.transform.position.x * parallaxSpeed);
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}
