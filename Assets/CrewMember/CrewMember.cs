using System.Collections.Generic;
using UnityEngine;

public class CrewMember : MonoBehaviour
{
    [SerializeField] private List<Transform> movePoints;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject uiPrefab;
    [SerializeField] private Canvas worldSpaceCanvas;

    private GameObject instantiatedUI;
    private Transform target;
    private int targetCount =0;

    void Start()
    {
        instantiatedUI = Instantiate(uiPrefab,worldSpaceCanvas.transform);
        instantiatedUI.gameObject.SetActive(false);
        SetUIPosition();
    }



    private void Update()
    {
        if (target == null)
        {
            return;
        }

        Vector3 dir = (target.position - transform.position).normalized;

        transform.position += moveSpeed * Time.deltaTime * dir;
        if (Vector2.Distance(transform.position, target.position) <= 0.1f)
        {
            
            ++targetCount;
            if (targetCount < movePoints.Count)
            {
                target = movePoints[targetCount];
            }
            else
            {
                AddToCrew();
            }
        }



        if (instantiatedUI.gameObject.activeInHierarchy)
        {
            SetUIPosition();
        }
    }


    void SetUIPosition()
    {
        instantiatedUI.transform.position = transform.position + new Vector3(0, 2f,0);
    }

    private void OnCrewMemberRecruited()
    {
        target = movePoints[targetCount];

    }

    private void AddToCrew()
    {
        print(gameObject.name + " added to crew");
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IPlayerRecruit>(out var player))
        {
            player.CanRecruit = true;
            instantiatedUI.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<IPlayerRecruit>(out var player))
        {
            player.CanRecruit = false;
            instantiatedUI.gameObject.SetActive(false);
        }
    }


    private void OnEnable()
    {
        PlayerRecruit.OnCrewMemberRecruited += OnCrewMemberRecruited;
    }

    private void OnDisable()
    {
        PlayerRecruit.OnCrewMemberRecruited -= OnCrewMemberRecruited;
    }


}