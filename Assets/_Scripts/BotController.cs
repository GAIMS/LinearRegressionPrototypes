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
    [SerializeField] public float distanceDelta;
    [HideInInspector] public Vector3 lastPos;
    [HideInInspector] public bool _canAim;
    
    [SerializeField] private GameObject slopeGuide;
    [SerializeField] private GameObject slopeIndicator;
    [SerializeField] private int slopeResolution;
    public bool usingNoise;
    public bool useDistance;
    public bool useDistanceForPow;

    [HideInInspector] public Rigidbody2D _rb;
    private GameObject _hole;
    private Vector2 _direction;
    
    private NoiseGenerator _noiseGen;
    private float x, y, z;
    private float minX, minY, minAngle;
    private bool checkSlope = true;

    private int shotNum = -1;
    private bool firstShot = true;
    public bool inHole = false;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _hole = GameObject.FindWithTag("Finish");
        _canAim = true;
        previousPow = closestPow = GetDistance();
        
        if (usingNoise)
        {
            _noiseGen = FindObjectOfType<NoiseGenerator>();
            transform.position = _noiseGen.highestCoord;
            slopeIndicator.SetActive(true);
            //aimReticle.SetActive(false);
            lastPos = transform.position;
        }
    }

    bool _canMove()
    {
        //float lerp = 0;
        if (_rb.velocity.magnitude <= stopSpeed)
        {
            _rb.velocity = Vector2.zero;
            aimGuide.SetActive(true);
            slopeGuide.SetActive(true);
            return true;
        }

        //lerp = 0;
        return false;
    }
    
    void Update()
    {
        if (_canMove() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Move();
        }
        else if(_canMove() && _canAim && !inHole)
        {
            Aim();
        }
    }

    void Aim()
    {
        distanceDelta = Vector3.Distance(lastPos, transform.position);
        lastPos = transform.position;
        
        
        shotNum++;
        if(useDistanceForPow)
        {
            currentPow = GetDistance();
            aimReticle.transform.localScale = new Vector3(.25f, (GetDistance() / 1.5f) / 2, 1);
            aimReticle.transform.localPosition = Vector3.right * (((GetDistance() / 1.5f) / 4) + .5f);
        }
        else
        {
            currentPow = GetSlope();
            aimReticle.transform.localScale = new Vector3(.25f, (GetSlope() / 1.5f) * 4, 1);
            aimReticle.transform.localPosition = Vector3.right * (((GetSlope() / 1.5f) * 2) + .5f);
        }
        if (_noiseGen && checkSlope)
        {
            slopeIndicator.transform.localScale = new Vector3(.25f, (GetSlope() / 1.5f) * 4, 1);
            slopeIndicator.transform.localPosition = Vector3.right * (((GetSlope() / 1.5f) * 2) + .5f);

            slopeGuide.transform.rotation = Quaternion.Euler(new Vector3(0, 0, minAngle));
            checkSlope = false;
        }
        //Debug.Log("previousPow " + previousPow);
        //Debug.Log("currentPow " + currentPow);
        if(useDistanceForPow)
        {
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
            else if (_canAim)
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
        }
        else if(_canAim && !firstShot)
        {
            //previousDir = angle;
            float noiseAngle = Random.Range(-aiRandomness + 60, aiRandomness - 60);
            aimGuide.transform.rotation = Quaternion.Euler(new Vector3(0, 0, minAngle + noiseAngle));
            
            _canAim = false;
            previousPow = currentPow;
            _direction = aimGuide.transform.right;
        }
        if (firstShot && _canAim)
        {

            float work = Random.Range(0, 360);
            //Debug.Log(work);
            aimGuide.transform.rotation = Quaternion.Euler(new Vector3(0, 0, work));
                
            _direction = aimGuide.transform.right;
            _canAim = false;
            firstShot = false;
        }
        Invoke("Move", 1f);
    }
    void Move()
    {
        aimGuide.SetActive(false);
        slopeGuide.SetActive(false);
        if (useDistanceForPow)
        {
            _rb.velocity = _direction.normalized * GetDistance() * speedMult;
        }
        else
        {
            _rb.velocity = _direction.normalized * GetSlope() * speedMult * 4;
        }
        _canAim = true;
        checkSlope = true;
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

    public void Hole()
    {
        inHole = true;
        aimGuide.SetActive(false);
        slopeGuide.SetActive(false);
    }
    
    public void ResetGen()
    {
        if(!inHole)
        {
            /*
            _noiseGen.noiseTex.SetPixel((int) ConvertWorldToTex(transform.position.x * 10, transform.position.y * 10).x,
                (int) ConvertWorldToTex(transform.position.x * 10, transform.position.y * 10).y,
                _noiseGen.noiseTex.GetPixel(
                    (int) ConvertWorldToTex(transform.position.x * 10, transform.position.y * 10).x,
                    (int) ConvertWorldToTex(transform.position.x * 10, transform.position.y * 10).y) +
                new Color(.5f, 0, 0, 1));
                */
            _noiseGen.noiseTex.SetPixel((int)ConvertWorldToTex(minX,minY).x,(int)ConvertWorldToTex(minX,minY).y,
                _noiseGen.noiseTex.GetPixel((int)ConvertWorldToTex(minX,minY).x,(int)ConvertWorldToTex(minX,minY).y) + new Color(.5f, 0, 0, 1));
            _noiseGen.noiseTex.Apply();
        }
    }
    
}
