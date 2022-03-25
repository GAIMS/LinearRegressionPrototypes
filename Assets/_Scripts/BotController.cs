using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField] private float speedMult;
    [SerializeField] private float stopSpeed;
    [SerializeField] private GameObject aimGuide;
    [SerializeField] private GameObject aimReticle;
    [Header("AI")] 
    [SerializeField] private float aiRandomness;
    [SerializeField] private float previousDir;
    [SerializeField] private float closestDir;
    [SerializeField] private float previousPow;
    [SerializeField] private float currentPow;
    [SerializeField] private float closestPow;
    private bool _canAim;

    private Rigidbody2D _rb;
    private GameObject _hole;
    private Vector2 _direction;

    private int shotNum = -1;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hole = GameObject.FindWithTag("Finish");
        _canAim = true;
        previousPow = closestPow = GetDistance();
    }

    bool _canMove()
    {
        float lerp = 0;
        if (_rb.velocity.magnitude <= stopSpeed)
        {
            _rb.velocity = Vector2.zero;
            aimGuide.SetActive(true);
            return true;
        }

        lerp = 0;
        return false;
    }
    
    void Update()
    {
        if (_canMove() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Move();
        }
        else if(_canMove() && _canAim)
        {
            Aim();
        }
    }

    void Aim()
    {
        currentPow = GetDistance();
        shotNum++;
        aimReticle.transform.localScale = new Vector3(.25f,(GetDistance()/1.5f)/2,1);
        aimReticle.transform.localPosition = Vector3.right * (((GetDistance() / 1.5f) / 4) + .5f);

        Debug.Log("previousPow " + previousPow);
        Debug.Log("currentPow " + currentPow);
        
        if (currentPow < previousPow && _canAim)
        {
            if (currentPow < closestPow)
            {
                closestPow = currentPow;
                closestDir = previousDir;
            }
            
            float newAngle = previousDir + Random.Range(-aiRandomness + 60, aiRandomness - 60);
            previousDir = newAngle;
            aimGuide.transform.rotation = Quaternion.Euler(new Vector3(0, 0, newAngle));
            _canAim = false;
            Debug.Log("closer");
            previousPow = currentPow;
        }
        else if(_canAim)
        {

            Vector3 randPos = Random.insideUnitCircle.normalized;
        
            Vector3 objectPos = aimGuide.transform.position;
            randPos.x = randPos.x - objectPos.x;
            randPos.y = randPos.y - objectPos.y;
 
            //float angle = Mathf.Atan2(randPos.y, randPos.x) * Mathf.Rad2Deg;
            float angle = previousDir + Random.Range(-aiRandomness, aiRandomness) + 180;
            previousDir = angle;
            aimGuide.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));  
            _canAim = false;
            Debug.Log("further");
            previousPow = currentPow;
        }
        _direction = aimGuide.transform.right;
        Invoke("Move", 1f);
    }
    void Move()
    {
        aimGuide.SetActive(false);
        _rb.velocity = _direction.normalized * GetDistance() * speedMult;
        _canAim = true;
    }

    float GetDistance()
    {
        return Vector2.Distance(transform.position, _hole.transform.position);
    }
}
