using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTrackGenerator : MonoBehaviour {
	
	private static RaceTrackGenerator _Instance;
	public static RaceTrackGenerator Instance {
		get {
			if (_Instance == null) {
				_Instance = FindObjectOfType<RaceTrackGenerator>();
			}
			return _Instance;
		}
	}
	
    public GameObject[] raceChunks;

    [SerializeField] private GameObject start, finish;
    
    [SerializeField] private int raceSegments;
	
	public int RaceSegments {
		get {
			return this.raceSegments;
		}
	}
    
    [Range(.25f,1)]
    [SerializeField] private float runWeight;
    [Range(.25f,1)]
    [SerializeField] private float climbWeight;
    [Range(.25f,1)]
    [SerializeField] private float flyWeight;
    [Range(.25f,1)]
    [SerializeField] private float swimWeight;
	
	public float RunWeight {
		get {
			return this.runWeight;
		} set {
			this.runWeight = value;
		}
	}
	
	public float FlyWeight {
		get {
			return this.flyWeight;
		} set {
			this.flyWeight = value;
		}
	}
	
	public float ClimbWeight {
		get {
			return this.climbWeight;
		} set {
			this.climbWeight = value;
		}
	}
	
	public float SwimWeight {
		get {
			return this.swimWeight;
		} set {
			this.swimWeight = value;
		}
	}

//    private KillBox killbox;
    private GameManager gm;
    
    private RaceChunk lastChunk;
	
	public RaceChunk LastChunk {
		get {
			return this.lastChunk;
		}
	}
	
    private RaceChunk finishChunk;
    private bool firstChunk = true;
    void Awake()
    {
    //    killbox = FindObjectOfType<KillBox>();
        gm = FindObjectOfType<GameManager>();
        for (int i = 0; i < raceSegments; i++)
        {
			if (i == 0) {
				PlaceStart();
				firstChunk = false;
			} else {
			
				GenerateTrack();
				if (i == raceSegments - 1)
				{
					PlaceFinish();
				}
			}
        }
    }

    
    void Update()
    {
    }

    public void GenerateTrack()
    {
        float runVal = Random.Range(.25f, runWeight);
        float climbVal = Random.Range(.25f, climbWeight);
        float flyVal = Random.Range(.25f, flyWeight);
        float swimVal = Random.Range(.25f, swimWeight);

        if (firstChunk) flyVal = climbVal = 0 ;
        
        float max = Mathf.Max(runVal, climbVal, flyVal, swimVal);

        if (runVal == max)
        {
            PlaceSegment(ChunkType.Ground);
        }
        if (climbVal == max)
        {
            PlaceSegment(ChunkType.Climb);
        }
        if (flyVal == max)
        {
            PlaceSegment(ChunkType.Fly);
        }
        if (swimVal == max)
        {
            PlaceSegment(ChunkType.Swim);
        }
    }

    public void PlaceSegment(ChunkType type)
    {
        List<GameObject> useableChunks = new List<GameObject>();
        foreach (var chunk in raceChunks)
        {
            if (chunk.GetComponent<RaceChunk>().chunkType == type)
            {
                useableChunks.Add(chunk);
            }
        }

        GameObject newChunk = gameObject;
        int rand = Random.Range(0, useableChunks.Count);
        if (firstChunk)
        {
            newChunk = Instantiate(useableChunks[rand], Vector3.zero, Quaternion.identity,transform);
        }
        else
        {
            newChunk = Instantiate(useableChunks[rand], lastChunk.rtPoint.position,Quaternion.identity,transform);
            //newChunk.transform.position += lastChunk.rtPoint.localPosition;
            newChunk.transform.position -= newChunk.GetComponent<RaceChunk>().lftPoint.localPosition;
        }
        lastChunk = newChunk.GetComponent<RaceChunk>();
    }
	
	public void PlaceStart() {
		GameObject newChunk = Instantiate(start, Vector3.zero, Quaternion.identity);
		lastChunk = newChunk.GetComponent<RaceChunk>();
	}

    public void PlaceFinish()
    {
        GameObject newChunk = Instantiate(finish, lastChunk.rtPoint.position,Quaternion.identity,transform);
        newChunk.transform.position -= newChunk.GetComponent<RaceChunk>().lftPoint.localPosition;
        finishChunk = newChunk.GetComponent<RaceChunk>();
    }

    public void Restart()
    {
    //    gm.UpdateLists(raceSegments);
        
        firstChunk = true;
    //    killbox.dead = 0;
        RaceChunk[] oldChunks = FindObjectsOfType<RaceChunk>();
        foreach (var chunk in oldChunks)
        {
            Destroy(chunk.gameObject);
        }
        for (int i = 0; i < raceSegments; i++)
        {
			if (i == 0) {
				PlaceStart();
				firstChunk = false;
			} else {
			
				GenerateTrack();
				if (i == raceSegments - 1)
				{
					PlaceFinish();
				}
			}
        }
		GameManager.Instance.RestartRace();
    }
	
	public void UpdateRaceSegments() {
		int segments = (int)GameplayUI.Instance.raceSegmentSlider.value;
		this.raceSegments = segments;
		GameplayUI.Instance.raceCourseSegments.SetText(segments.ToString());
	}
	
	public void UpdateRun() {
		float num = GameplayUI.Instance.runSlider.value;
		this.runWeight = num;
		GameplayUI.Instance.run.SetText(num.ToString("F2"));
	}
	
	public void UpdateFly() {
		float num = GameplayUI.Instance.flySlider.value;
		this.flyWeight = num;
		GameplayUI.Instance.fly.SetText(num.ToString("F2"));		
	}
	
	public void UpdateClimb() {
		float num = GameplayUI.Instance.climbSlider.value;
		this.climbWeight = num;
		GameplayUI.Instance.climb.SetText(num.ToString("F2"));		
	}
	
	public void UpdateSwim() {
		float num = GameplayUI.Instance.swimSlider.value;
		this.swimWeight = num;
		GameplayUI.Instance.swim.SetText(num.ToString("F2"));
	}
}