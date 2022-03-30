using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speedMult;
    [SerializeField] private float stopSpeed;
    [SerializeField] private GameObject aimGuide;
    [SerializeField] private GameObject aimReticle;
    public bool usingNoise;

    private Rigidbody2D _rb;
    private GameObject _hole;
    private Vector2 _direction;
    private NoiseGenerator _noiseGen;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hole = GameObject.FindWithTag("Finish");
        if (usingNoise)
        {
            _noiseGen = FindObjectOfType<NoiseGenerator>();
            transform.position = _noiseGen.highestCoord;
        }
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
            Move();
        }
        else if(_canMove())
        {
            Aim();
        }
    }

    void Aim()
    {
        aimReticle.transform.localScale = new Vector3(.25f,(GetDistance()/1.5f)/2,1);
        aimReticle.transform.localPosition = Vector3.right * (((GetDistance() / 1.5f) / 4) + .5f);

        Vector3 mousePos = Input.mousePosition;
        
        Vector3 objectPos = Camera.main.WorldToScreenPoint (aimGuide.transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
 
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        aimGuide.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _direction = aimGuide.transform.right;
    }
    void Move()
    {
        aimGuide.SetActive(false);
        _rb.velocity = _direction.normalized * GetDistance() * speedMult;
    }

    float GetDistance()
    {
        return Vector2.Distance(transform.position, _hole.transform.position);
    }
}
