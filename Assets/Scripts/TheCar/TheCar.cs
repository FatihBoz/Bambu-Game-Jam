using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class TheCar : MonoBehaviour
{
    [SerializeField] private GameObject playerUI;
    [SerializeField] private AudioClip engine;

    private AudioSource source;

    [Header("Car Statistics")]
    public GameObject handbrakeUI;
    public GameObject brakeUI;
    public TextMeshProUGUI Gear;
    public TextMeshProUGUI Speed;

    [SerializeField] private PlayerMovement player;
    [SerializeField] private GameObject carUI;
    [SerializeField] private GameObject driver;
    [SerializeField] private Transform getOffPos;

    [Header(" GameObjects ")]
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private MazotBar mazotBar;
    [SerializeField] private GameObject taret;
    [SerializeField] private GameObject missle;
    [SerializeField] private Transform misslePlace;

    [Space(10)]

    [Header("Car Attributes")]
    public float carHealth;
    public static float aracYurur;
    [SerializeField] private float maxMazot;
    private float currentMazot;
    private float mazotUsage;


    bool mazotOk = true;
    bool carHealthOk = true;
    bool yururOk = true;


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

    [Header("Gears Level Speed")]
    [SerializeField] private float gear1Speed;
    [SerializeField] private float gear2Speed;

    private Rigidbody2D carRigidbody;

    private bool isHandbrakeActive;
    float handBreakSpeed = 2f;

    int gearLevel = 0;
    float speed;
    Vector3 mousePos;
    bool controllingTaret;

    List<Transform> wheelsList = new List<Transform>();

    bool isBraking =false;

    private bool playerIsInDriverSeat;
    void Start()
    {
        source = GetComponent<AudioSource>();
        carRigidbody = GetComponent<Rigidbody2D>();
        taretSprite = taret.GetComponent<SpriteRenderer>();
        orjSpriteColor = taretSprite.color;

        healthBar.SetMaxHealth(carHealth);
        mazotBar.SetMaxMazot(maxMazot);

        GetWheels();

        currentMazot = maxMazot;

        Invoke(nameof(ToggleHandbrake),1f);
    }

    public void GetWheels()
    {

        // "Wheels" isminde bir child arıyoruz.
        Transform wheelsParent = transform.Find("Wheels");

        if (wheelsParent != null)
        {
            // "Wheels" parent'ındaki tüm child'ları dolaşarak listeye ekliyoruz.
            foreach (Transform wheel in wheelsParent)
            {
                wheelsList.Add(wheel);
            }
        }
        else
        {
            Debug.LogWarning("Wheels parent not found!");
        }
    }

    public void ApplyTorqueToWheels()
    {
        foreach (Transform wheel in wheelsList)
        {
            // Wheel objesinin Rigidbody2D bileşenini alıyoruz
            Rigidbody2D wheelRigidbody2D = wheel.GetComponent<Rigidbody2D>();

            // Eğer wheel objesinde Rigidbody2D varsa, tork uyguluyoruz
            if (wheelRigidbody2D != null)
            {
                wheelRigidbody2D.AddTorque(-speed * Time.fixedDeltaTime);

            }
            else
            {
                Debug.LogWarning("No Rigidbody2D found on " + wheel.name);
            }
        }
    }

    public void ApplyBrake()
    {
        foreach (Transform wheel in wheelsList)
        {
            Rigidbody2D wheelRigidbody2D = wheel.GetComponent<Rigidbody2D>();

            if (wheelRigidbody2D != null)
            {
                wheelRigidbody2D.AddTorque(speed * Time.fixedDeltaTime);

            }
            else
            {
                Debug.LogWarning("No Rigidbody2D found on " + wheel.name);
            }
        }
    }

    private void ToggleHandbrake()
    {
        if (!isHandbrakeActive)
        {
            // Eğer araç hız sınırının altındaysa el frenini etkinleştir
            if (carRigidbody.velocity.magnitude < handBreakSpeed)
            {
                isHandbrakeActive = true;
                carRigidbody.constraints = RigidbodyConstraints2D.FreezeAll; // Rigidbody'yi dondur
                Debug.Log("El freni çekildi.");
            }
            else
            {
                Debug.Log("Araç çok hızlı, el freni çekilemez!");
            }
        }
        else
        {
            // El frenini devre dışı bırak
            isHandbrakeActive = false;
            carRigidbody.constraints = RigidbodyConstraints2D.None; // Rigidbody'yi serbest bırak
            Debug.Log("El freni bırakıldı.");
        }
    }

    public void StatisticChanger()
    {
        if (isHandbrakeActive)
        {
            handbrakeUI.SetActive(true);
        }
        else
        {
            handbrakeUI.SetActive(false);
        }

        if (isBraking)
        {
            brakeUI.SetActive(true);
        }
        else
        {
            brakeUI.SetActive(false);
        }

        Gear.text = $"Gear : {gearLevel}";

        Speed.text = $"Speed : {(carRigidbody.velocity.x).ToString("F2")}";
    }

    // Update is called once per frame
    void Update()
    {
        StatisticChanger();

        //Debug.Log("Hızımız : " + this.GetComponent<Rigidbody2D>().velocity);

        //Debug.Log("Mazot Durumu : " + mazot);

        fireTimer -= Time.deltaTime;





        if (playerIsInDriverSeat)
        {
            if (Input.GetKey(KeyCode.P))
                isBraking = true;
            else
                isBraking = false;

            if (Input.GetKeyDown(KeyCode.O))
                ToggleHandbrake();

            if (Input.GetKeyDown(KeyCode.UpArrow))
                gearUp();

            if (Input.GetKeyDown(KeyCode.DownArrow))
                gearDown();
        }

        Movement();


        controllingTaret = playerIsInDriverSeat;

        ControlTheTaret();
        FiringTheMissle();
        taretHeatStatus();
        mazotRunsOut();
    }



    #region TaretActions

    private void taretHeatStatus()
    {
        if (taretCurrentHeat > 0)
            taretCurrentHeat -= taretHeatDown * Time.deltaTime;
        //Debug.Log(taretCurrentHeat);
        taretSprite.color = Color.Lerp(orjSpriteColor, Color.red, taretCurrentHeat);
    }

    private void FiringTheMissle()
    {
        if (controllingTaret && !isTaretHeatedUp)
        {
            if (Input.GetMouseButton(0) && fireTimer < 0)
            {
                Missle newMissle = Instantiate(missle, misslePlace.position, Quaternion.identity).GetComponent<Missle>();
                ScreenShake.Instance.Shake(0.1f,0);
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
            Vector3 direction = mousePos - misslePlace.transform.position;
            // Rotasyonu hesapla 
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            // Objeyi döndür
            taret.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
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

    #endregion





    #region Health Actions
    public void CartakingDamage(float _damage)
    {
        if (carHealth > 0)
            carHealth -= _damage;
        healthBar.SetCurrentHealth(carHealth);
        Debug.Log(carHealth);
        if (carHealth < 0)
            carHealthOk = false;
    }


    public void CarTakeHealth(float _health)
    {
        if (carHealth + _health > 100)
            carHealth = 100f;
        else
            carHealth += _health;
        carHealthOk = true;
        healthBar.SetCurrentHealth(carHealth);
    }
    #endregion



    #region Mazot Actions

    void mazotRunsOut()
    {
        if (currentMazot > 0)
            currentMazot -= mazotUsage;
        mazotBar.SetCurrentMazot(currentMazot);
        if (currentMazot <= 0)
            mazotOk = false;
    }

    public void mazotAdd(float _mazotAmount)
    {
        if ((currentMazot + _mazotAmount) > maxMazot)
            currentMazot = maxMazot;
        else
            currentMazot += _mazotAmount;
        mazotOk = true;
    }

    #endregion

    // Velocity of the Car
    private void Movement()
    {
        if (!playerIsInDriverSeat)
        {
            return;
        }

        if(isBraking)
        {
            ApplyBrake();
        }

        if (gearLevel > 0 && canGo() && !isBraking)
        {
            //rb.velocity = new Vector2(speed, rb.velocity.y);
            ApplyTorqueToWheels();

        }
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

    #region  Gear
    private void gearLevelDetection()
    {
        Debug.Log($"Vites: {gearLevel}");

        if (gearLevel == 0)
        {
            speed = 0f;
            mazotUsage = 0f; // Boş viteste mazot tüketimi durabilir veya çok az olabilir.
        }
        else if (gearLevel == 1)
        {
            speed = gear1Speed;
            mazotUsage = 0.1f; // İlk viteste düşük mazot tüketimi.
        }
        else if (gearLevel == 2)
        {
            speed = gear2Speed;
            mazotUsage = 0.2f; // İkinci viteste daha fazla mazot tüketimi.
        }
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



    private void OnEnable()
    {
        CarDriveTriggerSystem.OnPlayerStartedToDrive += Car_OnPlayerStartedToDrive;
        CarDriveTriggerSystem.OnPlayerStoppedToDrive += Car_OnPlayerStoppedToDrive;
        FuelTriggerSystem.OnFuelGained += Car_OnFuelGained;
    }



    private void OnDisable()
    {
        CarDriveTriggerSystem.OnPlayerStartedToDrive -= Car_OnPlayerStartedToDrive;
        CarDriveTriggerSystem.OnPlayerStoppedToDrive -= Car_OnPlayerStoppedToDrive;
        FuelTriggerSystem.OnFuelGained -= Car_OnFuelGained;
    }

    private void Car_OnPlayerStoppedToDrive()
    {
        source.Stop();
        driver.SetActive(false);
        player.gameObject.SetActive(true);
        player.transform.position = getOffPos.position;
        carUI.SetActive(false);
        playerUI.SetActive(true);
        playerIsInDriverSeat = false;

        StartCoroutine(SlowApproach(-6));
        if (Camera.main.TryGetComponent<CameraControl>(out var camera))
        {
            camera.offset = new Vector3(5, camera.offset.y, camera.offset.z);
            camera.SetTarget(player.transform);
        }
    }

    private void Car_OnFuelGained(int amount)
    {
        mazotAdd(amount);
        print("mazot geldi");
        mazotBar.SetCurrentMazot(currentMazot);
    }

    private void Car_OnPlayerStartedToDrive()
    {
        source.PlayOneShot(engine);
        driver.SetActive(true);
        carUI.SetActive(true);
        playerIsInDriverSeat = true;

        StartCoroutine(SlowApproach(6));
        if (Camera.main.TryGetComponent<CameraControl>(out var camera))
        {
            camera.offset = new Vector3(5, camera.offset.y, camera.offset.z);
            camera.SetTarget(driver.transform);
        }
    }

    public bool IsHandBreakActive()
    {
        return isHandbrakeActive;
    }


    private IEnumerator SlowApproach(float targetIncrease)
    {
        Camera mainCam = Camera.main;
        float initialSize = mainCam.orthographicSize; // Kameranın başlangıç boyutu
        float targetSize = initialSize + targetIncrease; // Hedef boyut
        float elapsedTime = 0f;

        Vector3 initialPosition = mainCam.transform.position; // Kameranın başlangıç pozisyonu

        while (elapsedTime < 1f)
        {
            mainCam.orthographicSize = Mathf.Lerp(initialSize, targetSize, elapsedTime / 1f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

    }
}
