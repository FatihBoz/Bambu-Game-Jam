using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class TheCar : MonoBehaviour
{
    [Header(" GameObjects ")]
    [SerializeField] private GameObject taret;
    [SerializeField] private GameObject missle;
    [SerializeField] private Transform misslePlace;

    [Space(10)]
    [SerializeField] private float carHealt;
    [SerializeField] private float aracYurur;
    [SerializeField] private float mazot;
    [SerializeField] private float mazotUsage;

    bool mazotOk;
    bool carHealthOk;
    bool yururOk;



    [Space(10)]

    [Header("Taret İnfos")]
    [SerializeField] private float fireStandBy = .1f;
    [SerializeField] private float missleDamage;
    [SerializeField] private float taretCoolDown;
    [SerializeField] private float taretHeatDown;
    [SerializeField] private float taretHeatUp;
    Color orjSpriteColor;

    bool isTaretHeatedUp = false;
    float taretCurrentHeat = 0;
    float fireTimer;
    SpriteRenderer taretSprite;
    [SerializeField] private float gear1Speed;
    [SerializeField] private float gear2Speed;


    int gearLevel = 0;
    float speed;
    Vector3 mousePos;
    bool controllingTaret;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        taretSprite = taret.GetComponent<SpriteRenderer>();
        orjSpriteColor = taretSprite.color;
    }

    // Update is called once per frame
    void Update()
    {
        fireTimer -= Time.deltaTime;
        Movement();

        if (Input.GetKeyDown(KeyCode.W))
            gearUp();

        if (Input.GetKeyDown(KeyCode.S))
            gearDown();


        if (Input.GetKeyDown(KeyCode.E))
            TaretOnOff();

        ControlTheTaret();
        FiringTheMissle();
        taretHeatStatus();
    }

    private void taretHeatStatus()
    {
        if (taretCurrentHeat > 0)
            taretCurrentHeat -= taretHeatDown * Time.deltaTime;
        Debug.Log(taretCurrentHeat);
        taretSprite.color = Color.Lerp(orjSpriteColor, Color.red, taretCurrentHeat);
    }

    private void FiringTheMissle()
    {
        if (controllingTaret && !isTaretHeatedUp)
        {
            if (Input.GetMouseButton(0) && fireTimer < 0)
            {
                Missle newMissle = Instantiate(missle, misslePlace.position, Quaternion.identity).GetComponent<Missle>();
                newMissle.AdjustTheMissle(mousePos, missleDamage);
                fireTimer = fireStandBy;
                TaretHeatUp();
            }
        }
    }



    // Controlling the taret with mouse
    public void ControlTheTaret()
    {
        if (controllingTaret)
        {
            Vector3 mousePos = takeMousePos();
            // Objeye doğru bir vektör oluştur 
            Vector3 direction = mousePos - transform.position;
            // Rotasyonu hesapla 
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // Objeyi döndür
            taret.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }


    private void TaretOnOff()
    {
        if (controllingTaret == false)
            controllingTaret = true;
        else if (controllingTaret == true)
            controllingTaret = false;
    }

    private void TaretHeatUp()
    {
        taretCurrentHeat += taretHeatUp;
        if (taretCurrentHeat >= 1f)
        {
            isTaretHeatedUp = true;
            StartCoroutine(taretHeated());
        }
    }

    IEnumerator taretHeated()
    {
        float time = 0f;
        while (time < taretCoolDown)
        {
            taretSprite.color = Color.Lerp(Color.red, orjSpriteColor, time / taretCoolDown);
            time += Time.deltaTime;
            yield return null;
        }
        isTaretHeatedUp = false;
        Debug.Log("taret sogudu");
        taretCurrentHeat = 0;
    }



    private Vector3 takeMousePos()
    {
        mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;
        return mousePos;
    }

    // Can car go ?
    bool canGo()
    {

        if (mazotOk && yururOk && carHealthOk)
            return true;
        else
            return false;
    }



    void CartakingDamage(float _damage)
    {
        carHealt -= _damage;
        if (carHealt < 0)
            carHealthOk = false;
    }


    void CarTakeHealth(float _health)
    {
        if (carHealt + _health > 100)
            carHealt = 100f;
        else
            carHealt += _health;
        carHealthOk = true;
    }


    void DamagedYurur(float _damage)
    {
        aracYurur -= _damage;
        if (aracYurur < 0)
            yururOk = false;
    }

    public void HealYurur(float _health)
    {
        if (aracYurur + _health > 100)
            aracYurur = 100f;
        else
            aracYurur += _health;
        yururOk = true;
    }


    void mazotRunsOut()
    {
        mazot -= mazotUsage;
        if (mazot <= 0)
            mazotOk = false;
    }

    public void mazotAdd(float _mazotAmount)
    {
        mazot += _mazotAmount;
        mazotOk = true;
    }


    // Velocity of the Car
    private void Movement()
    {
        if (gearLevel > 0 && canGo())
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }



    #region  Gear
    // Aracın fitesine göre hızını değiştirildiği kısım
    private void gearLevelDetection()
    {
        Debug.Log($"vites {gearLevel}");
        if (gearLevel == 0)
            speed = 0f;
        else if (gearLevel == 1)
            speed = gear1Speed;
        else if (gearLevel == 2)
            speed = gear2Speed;
    }

    public void gearUp()
    {
        if (gearLevel < 2)
            gearLevel++;
        gearLevelDetection();
    }

    public void gearDown()
    {
        if (gearLevel > 0)
            gearLevel--;
        gearLevelDetection();
    }

    #endregion
}
