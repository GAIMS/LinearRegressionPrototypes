using UnityEngine;
using System.Collections;
 
public class FlyCamera : MonoBehaviour {
	
	private static FlyCamera _Instance;
	public static FlyCamera Instance {
		get {
			if (_Instance == null) {
				_Instance = FindObjectOfType<FlyCamera>();
			}
			return _Instance;
		}
	}
	
	public Transform racerToFollow;
	
	private int racer = 0;
	
	private void Update() {
		if (this.racerToFollow == null) {
			return;
		}
		this.transform.position = this.racerToFollow.position + new Vector3(0f, 9.5f, -35f);
		
		
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			this.racer++;
			if (this.racer >= GameManager.Instance.racers.Count) {
				this.racer = 0;
			}
			this.racerToFollow = GameManager.Instance.racers[this.racer].transform;
		}
		
		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			this.racer--;
			if (this.racer < 0) {
				this.racer = GameManager.Instance.racers.Count - 1;
			}
			this.racerToFollow = GameManager.Instance.racers[this.racer].transform;
		}
	} 
 
	public void SetRacer(int index) {
		this.racer = index;
	}
 
	/*
    Writen by Windexglow 11-13-10.  Use it, edit it, steal it I don't care.  
    Converted to C# 27-02-13 - no credit wanted.
    Simple flycam I made, since I couldn't find any others made public.  
    Made simple to use (drag and drop, done) for regular keyboard layout  
    wasd : basic movement
    shift : Makes camera accelerate
    space : Moves camera on X and Z axis only.  So camera doesn't gain any height
     
     
    float mainSpeed = 15.0f; //regular speed
    float shiftAdd = 25.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 50.0f; //Maximum speed when holdin gshift
    float camSens = 0f; //How sensitive it with mouse
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    private float totalRun= 1.0f;
	
	private Vector3 initialPos;
	
	void Start() {
		this.initialPos = this.transform.position;
	}
     
    void Update () {
        //Mouse  camera angle done.  
       
        //Keyboard commands
        float f = 0.0f;
        Vector3 p = GetBaseInput();
        if (p.sqrMagnitude > 0){ // only move while a direction key is pressed
          if (Input.GetKey (KeyCode.LeftShift)){
              totalRun += Time.deltaTime;
              p  = p * totalRun * shiftAdd;
              p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
              p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
              p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
          } else {
              totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
              p = p * mainSpeed;
          }
         
          p = p * Time.deltaTime;
          Vector3 newPosition = transform.position;
          if (Input.GetKey(KeyCode.Space)){ //If player wants to move on X and Z axis only
              transform.Translate(p);
              newPosition.x = transform.position.x;
              newPosition.z = transform.position.z;
              transform.position = newPosition;
          } else {
              transform.Translate(p);
          }
        }
    }
     
    private Vector3 GetBaseInput() { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey (KeyCode.W)){
            p_Velocity += new Vector3(0, 1 , 0);
        }
        if (Input.GetKey (KeyCode.S)){
            p_Velocity += new Vector3(0, -1, 0);
        }
        if (Input.GetKey (KeyCode.A)){
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey (KeyCode.D)){
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }
	
	public void ResetPosition() {
		this.transform.position = this.initialPos;
	}
	*/
}