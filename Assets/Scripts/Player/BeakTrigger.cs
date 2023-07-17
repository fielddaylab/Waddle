//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeakTrigger : MonoBehaviour
{
	/*float _gackTimer = 0.0f;

	[SerializeField]
	float _gackTimerLimit = 1.5f;
	float GackTimerLimit => _gackTimerLimit;*/
	
	GameObject _pebbleTarget = null;
	GameObject PebbleTarget => _pebbleTarget;
	
	//variables for snowball Bowling
	GameObject[] bowlingPenguins;
	GameObject[] bowlingBalls;

    [SerializeField] private float _minBeatTime = 0.5f; // min time between mating dance beats
    private float _minBeatTimer = 0; // elapsed time since prev beat 

    AudioSource _audioFile = null;
    
    void Start()
    {
        //_pebbleTarget = GameObject.FindWithTag("Egg");
		_audioFile = GetComponent<AudioSource>();
		
		bowlingPenguins = GameObject.FindGameObjectsWithTag("BowlingPenguin");
		bowlingBalls = GameObject.FindGameObjectsWithTag("BowlingBall");
		/*for(int i=0; i<bowlingPenguins.Length; i++){
			bowlingPenguins[i].SetActive(false);
		}
		for(int i=0; i<bowlingBalls.Length; i++){
			bowlingBalls[i].SetActive(false);
		}*/
    }

    // Update is called once per frame
    void Update()
    {
		/*if(_pebbleTarget == null)
		{
			 _pebbleTarget = GameObject.FindWithTag("Egg");
		}*/
		if (_minBeatTimer > 0) {
            _minBeatTimer -= Time.deltaTime;
        }
    }

	IEnumerator BowlingBallGrow(GameObject ball){
		for(int i=0; i<40; i++){
			ball.transform.localScale += new Vector3(0.1f,0.1f,0.1f);
			yield return new WaitForSeconds(0.05f);
		}
	}


	IEnumerator MoveToPos(Collider pebble, float duration)
	{
		if(_pebbleTarget != null)
		{
			Vector3 heightAdjust = new Vector3(0,0.5f,0);
			
			Vector3 startPosition = pebble.gameObject.transform.position;
			Vector3 newSpot = _pebbleTarget.transform.position;

			Vector3[] points = new Vector3[]{
				startPosition,
				Vector3.Lerp(startPosition, newSpot,  0.25f) + heightAdjust,
				Vector3.Lerp(startPosition, newSpot,  0.75f) + heightAdjust,
				newSpot
			};

			//move the pebble to the position of text
			float t = 0f;
			//float timeSlice = 0.02f;
			//float timeSliceCount = 0f;
			while(t < duration)
			{
				/*if(timeSliceCount > timeSlice){
					timeSliceCount = 0;
					newSpot = _pebbleTarget.transform.position;
				}*/

				//pebble.gameObject.transform.position = Vector3.Lerp(startPosition, newSpot,  (t/duration) );
				pebble.gameObject.transform.position = GetPoint(t/duration, points);
				t += (Time.deltaTime);
				//timeSliceCount += (Time.deltaTime);	
				yield return null;
			}

			//move the pebble to the nest
			/*yield return new WaitForSeconds(3);
			t = 0f;
			startPosition = pebble.gameObject.transform.position;
			newSpot = nest.transform.position;
			while(t < 3)
			{
				pebble.gameObject.transform.position = Vector3.Lerp(startPosition, newSpot,  (t/duration) );
				
				t += (Time.deltaTime);
				yield return null;
			}*/
		
			Transform p = pebble.gameObject.transform.parent;
			if(p != null)
			{
				Transform gp = p.parent;
				if(gp != null)
				{
					MiniGameUnlocker unlocker = gp.GetComponent<MiniGameUnlocker>();
					if(unlocker != null)
					{
						unlocker.CollectPebble();
					}
				}
			}
		}
		
		MeshRenderer mr = pebble.gameObject.GetComponent<MeshRenderer>();
		if(mr != null)
		{
			mr.enabled = false;
			//pebble.gameObject.SetActive(false);
		}
		//If the player havn't collected enough pebbles, continue to construct the nest
		/*if(pebbleCount <= 10){
			for(int i=0; i<3; i++){
				nestRocks[nestRockCount].SetActive(true);
				nestRockCount += 1;
			}
		}*/

	}
	
	void OnTriggerEnter(Collider otherCollider)
	{
		//Debug.Log(otherCollider.gameObject.name);
		//todo - get rid of string checks here.
		if(otherCollider.gameObject.name.StartsWith("Pebble"))
		{
			MeshRenderer mr = otherCollider.gameObject.GetComponent<MeshRenderer>();
			if(mr.enabled)
			{
				if(_audioFile != null)
				{
					_audioFile.Play();
				}
				
				//if these rocks are coming from a mini game, set pebble target dynamically to that mini game's nest
				Transform p = otherCollider.gameObject.transform.parent;
				if(p != null)
				{
					if(p.gameObject.name == "Pebbles")
					{
						//current assumption - Pebbles and SparseNest object's have the same transform.
						_pebbleTarget = p.gameObject;
					}
				}
				
				Vector3 pos = Vector3.zero;
				Quaternion view = Quaternion.identity;
				PenguinPlayer.Instance.GetGaze(out pos, out view);
				PenguinAnalytics.Instance.LogPickupRock(pos, view);

				StartCoroutine(MoveToPos(otherCollider, 1));
				
				//if(gameObject.transform.childCount == 0)
				{
					//pick up a rock with your beak - commented out line below...
					//otherCollider.gameObject.transform.parent = gameObject.transform;

					Rigidbody rb = otherCollider.gameObject.GetComponent<Rigidbody>();
					if(rb != null)
					{
						rb.isKinematic = true;
						rb.detectCollisions = false;
					}
				}
			}
			//Debug.Log(otherCollider.gameObject.name);
		}
		else if(otherCollider.gameObject.name.StartsWith("Bubble"))
		{
			//play positive sound effect if they collide with bubble...
			//also destroy bubble...
			ShrinkRing sr = otherCollider.gameObject.transform.GetChild(0).GetComponent<ShrinkRing>();
			if (sr != null && _minBeatTimer <= 0)
			{
				AudioSource audio = otherCollider.gameObject.GetComponent<AudioSource>();
				if(audio != null)
				{
					//Debug.Log("Bubble Hit!");
					PenguinAnalytics.Instance.LogBubblePop(sr.GetWhichBubble(), sr.GetTiming());
					audio.PlayOneShot(audio.clip);
                }

                sr.Popped();
				_minBeatTimer = _minBeatTime;
			}
		}
		
		//picking up bowling ball
		if(otherCollider.gameObject.name.StartsWith("BowlingBall"))
		{
			//pick up a bowling with your beak
			//if(gameObject.transform.childCount == 4)
			{
				//otherCollider.gameObject.transform.parent = gameObject.transform;
				Rigidbody rb = otherCollider.gameObject.GetComponent<Rigidbody>();
				if(rb != null)
				{
					Vector3 f = transform.forward;
					rb.AddForce(f*10000.0f);
					StartCoroutine(BowlingBallGrow(otherCollider.gameObject));
					
					Vector3 pos = Vector3.zero;
					Quaternion view = Quaternion.identity;
					PenguinPlayer.Instance.GetGaze(out pos, out view);
					PenguinAnalytics.Instance.LogPushSnowball(pos, view);
					//rb.isKinematic = true;
					//rb.detectCollisions = false;
				}
				
				/*BowlingSlope bs = otherCollider.gameObject.GetComponent<BowlingSlope>();
				if(bs != null)
				{
					bs.SlopeTrigger.GetComponent<MeshRenderer>().enabled = true;
				}*/
			}
		}

		/*if(otherCollider.gameObject.name.StartsWith("SlopeTrigger"))
		{
			if(gameObject.transform.childCount != 0)
			{
				int ballIdx = gameObject.transform.childCount-1;
				GameObject ball = gameObject.transform.GetChild(ballIdx).gameObject;
				Debug.Log(ball.tag);
				Debug.Log(ball.name);
				if(ball.tag == "BowlingBall")
				{
					//Debug.Log("Detaching");
					ball.name = "DetachedBall";
					ball.transform.parent = null;
					ball.transform.rotation = Camera.main.transform.rotation;
					//ball.transform.rotation = Quaternion.Inverse(gameObject.transform.rotation);
					Rigidbody rb = ball.GetComponent<Rigidbody>();
					if(rb != null)
					{
						rb.isKinematic = false;
						rb.detectCollisions = true;
						rb.AddForce(ball.transform.forward*10000.0f);
						StartCoroutine(BowlingBallGrow(ball));
						
						otherCollider.gameObject.GetComponent<MeshRenderer>().enabled = false;
					}
				}
			}
		}*/

		if(otherCollider.gameObject.name.StartsWith("SkewerTrigger")){
			//Debug.Log("Skewer!!!!!!!");
			AudioSource audioS = otherCollider.gameObject.transform.parent.parent.gameObject.GetComponent<AudioSource>();
			GameObject fish = otherCollider.gameObject.transform.parent.gameObject;
			if(fish != null)
			{
				if(fish.GetComponent<MeshRenderer>().enabled)
				{
					audioS.Play();
				}
				
				fish.GetComponent<MeshRenderer>().enabled = false;
				
				Vector3 pos = Vector3.zero;
				Quaternion view = Quaternion.identity;
				PenguinPlayer.Instance.GetGaze(out pos, out view);
				PenguinAnalytics.Instance.LogEatFish(pos, view);
			}
			
            //fish.SetActive(false);
		}


		//start penguin bowling game when the player touches the attraction with the beak
		/*if(otherCollider.gameObject.name.StartsWith("BowlingAttraction"))
		{
			for(int i=0; i<bowlingPenguins.Length; i++){
				bowlingPenguins[i].SetActive(true);
			}
			for(int i=0; i<bowlingBalls.Length; i++){
				bowlingBalls[i].SetActive(true);
			}
		}*/
	}
	
	/*void OnTriggerStay(Collider otherCollider)
	{
		if(otherCollider.gameObject.name == "SqwaukBox")
		{
			_gackTimer += UnityEngine.Time.deltaTime;
			if(_gackTimer > _gackTimerLimit)
			{
				//play gack sound.
				otherCollider.gameObject.GetComponent<AudioSource>().Play();
				_gackTimer = 0f;
			}
		}
	}
	
	void OnTriggerExit(Collider otherCollider)
	{
		
		if(otherCollider.gameObject.name == "SqwaukBox")
		{
			_gackTimer = 0f;
		}
		
		//have on trigger exit cause movement?
	}*/
	
	//Jack's edit
    Vector3 GetPoint (float t, Vector3[] points) {
		//return transform.TransformPoint(GetBezierPoint(points[0], points[1], points[2], points[3], t));
		return CalculateCubicBezierPoint(t, points[0], points[1], points[2], points[3]);
	}

	Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        
        Vector3 p = uuu * p0; 
        p += 3 * uu * t * p1; 
        p += 3 * u * tt * p2; 
        p += ttt * p3; 
        
        return p;
    }

}
