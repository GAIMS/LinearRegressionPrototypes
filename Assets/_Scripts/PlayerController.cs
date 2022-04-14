using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speedMult;
    [SerializeField] private float stopSpeed;
    [SerializeField] private GameObject aimGuide;
    [SerializeField] private GameObject aimReticle;
    [SerializeField] private GameObject slopeGuide;
    [SerializeField] private GameObject slopeIndicator;
    [SerializeField] private int slopeResolution;
    [SerializeField] private Text shotNumText;
    public bool usingNoise;
    public bool useDistance;
    public bool useDistanceForPow;
    public bool twoPlayer;
    public bool playerTwo;
    public bool myTurn;

    private Rigidbody2D _rb;
    private GameObject _hole;
    private Vector2 _direction;
    private NoiseGenerator _noiseGen;
    private float x, y, z;
    private float minX, minY, minAngle;
    private bool checkSlope = true;
    private GameManager gm;
    private int shotNum;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hole = GameObject.FindWithTag("Finish");
        shotNum = 0;
        shotNumText = FindObjectOfType<Text>();
        if (usingNoise)
        {
            _noiseGen = FindObjectOfType<NoiseGenerator>();
            transform.position = _noiseGen.highestCoord;
            slopeIndicator.SetActive(true);
        }

        if (twoPlayer)
        {
            gm = FindObjectOfType<GameManager>();
        }
    }

    bool _canMove()
    {
        //float lerp = 0;
        if (_rb.velocity.magnitude <= stopSpeed)
        {
            if (twoPlayer && myTurn)
            {
                _rb.velocity = Vector2.zero;
                aimGuide.SetActive(true);
                slopeGuide.SetActive(true);
                return true;
            }
            else if(!twoPlayer)
            {
                _rb.velocity = Vector2.zero;
                aimGuide.SetActive(true);
                slopeGuide.SetActive(true);
                return true;
            }

        }

        //lerp = 0;
        return false;
    }
    
    void Update()
    {
        if (_canMove() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            shotNum++;
            if (twoPlayer && myTurn)
            {
                gm.ChangeTurn(this);

                Move();
            }
            else if(!twoPlayer)
            {
                Move();
                shotNumText.text = "Shot Number: " + shotNum;
            }
        }
        else if(_canMove())
        {
            if (twoPlayer && myTurn)
            {
                Aim();
            }
            else if(!twoPlayer)
            {
                Aim();
            }
        }
    }

    void Aim()
    {
        if (!useDistanceForPow)
        {
            aimReticle.transform.localScale = new Vector3(.25f,(GetDistance()/1.5f)/2,1);
            aimReticle.transform.localPosition = Vector3.right * (((GetDistance() / 1.5f) / 4) + .5f);
        }
        else
        {
            aimReticle.transform.localScale = new Vector3(.25f, (GetSlope() / 1.5f) * 4, 1);
            aimReticle.transform.localPosition = Vector3.right * (((GetSlope() / 1.5f) * 2) + .5f);
        }


        Vector3 mousePos = Input.mousePosition;
        
        Vector3 objectPos = Camera.main.WorldToScreenPoint (aimGuide.transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;
 
        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        aimGuide.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        _direction = aimGuide.transform.right;

        if (_noiseGen && checkSlope)
        {
            slopeIndicator.transform.localScale = new Vector3(.25f, (GetSlope() / 1.5f) * 4, 1);
            slopeIndicator.transform.localPosition = Vector3.right * (((GetSlope() / 1.5f) * 2) + .5f);

            slopeGuide.transform.rotation = Quaternion.Euler(new Vector3(0, 0, minAngle));
            checkSlope = false;
        }
    }
    void Move()
    {
        aimGuide.SetActive(false);
        slopeGuide.SetActive(false);
        checkSlope = true;
        if(!useDistanceForPow)
            _rb.velocity = _direction.normalized * GetDistance() * speedMult;
        else
        {
            _rb.velocity = _direction.normalized * GetSlope() * speedMult * 4;
        }
    }

    float GetDistance()
    {
        return Vector2.Distance(transform.position, _hole.transform.position);
    }

    float GetSlope()
    {
        float min = 1f;
        for (int i = 0; i < 360 * slopeResolution; i++)
        {
            if (useDistance)
            {
                x = ((transform.position.x * 10) + (GetDistance() / 1.5f) * Mathf.Cos(i * Mathf.PI / 180));
                y = ((transform.position.y * 10) + (GetDistance() / 1.5f) * Mathf.Sin(i * Mathf.PI / 180));   
            }
            if (!useDistance)
            {
                x = ((transform.position.x * 10) + 10 * Mathf.Cos(i * Mathf.PI / 180));
                y = ((transform.position.y * 10) + 10 * Mathf.Sin(i * Mathf.PI / 180));
            }
            
            z = _noiseGen.noiseTex.GetPixel((int)ConvertWorldToTex(x,y).x,(int)ConvertWorldToTex(x,y).y).r;
            
            //_noiseGen.noiseTex.SetPixel((int)ConvertWorldToTex(x,y).x,(int)ConvertWorldToTex(x,y).y, Color.blue);
            //_noiseGen.noiseTex.Apply();
            //Debug.Log(x +","+ y);
            //Debug.Log(ConvertWorldToTex(x,y));
            //Debug.Log("point at angle: " + i + " is " + z);
            
            if (z < min)
            {
                min = z;
                minX = x;
                minY = y;
                minAngle = i;
            }
        }

        _noiseGen.noiseTex.SetPixel((int)ConvertWorldToTex(minX,minY).x,(int)ConvertWorldToTex(minX,minY).y,
            _noiseGen.noiseTex.GetPixel((int)ConvertWorldToTex(minX,minY).x,(int)ConvertWorldToTex(minX,minY).y) + Color.green);
        _noiseGen.noiseTex.Apply();
        //Debug.Log("Min = " + min);
        
        return min;
    }

    Vector2 ConvertWorldToTex(float x, float y)
    {
        return new Vector2(((int) -x + (_noiseGen.pxlWidth/2)), ((int) -y + (_noiseGen.pxlHeight/2)));
    }
}
