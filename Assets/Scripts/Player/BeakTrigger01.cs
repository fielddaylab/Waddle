//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeakTrigger01 : MonoBehaviour
{

	float _gackTimer = 0.0f;
	//count the number of pebbles collected
	int pebbleCount = 0;
	int nestRockCount = 0;
	GameObject pebbleTextObject;
	GameObject nest;
	GameObject[] nestRocks;
	Text pebbleText; 
	
	//variables for snowball Bowling
	GameObject[] bowlingPenguins;
	GameObject[] bowlingBalls;

	[SerializeField]
	float _gackTimerLimit = 1.5f;
	float GackTimerLimit => _gackTimerLimit;
	
    // Start is called before the first frame update
    void Start()
    {
		//initialize pebble collection
		nestRocks = GameObject.FindGameObjectsWithTag("NestRock");
		for(int i=0; i<nestRocks.Length; i++){
			nestRocks[i].SetActive(false);
		}
        pebbleTextObject = GameObject.Find("PebbleText");
		pebbleText = pebbleTextObject.GetComponent<Text>();
		pebbleText.text = "0 pebble collected";
		nest = GameObject.Find("Nest");

		//initialize Penguin Bowling game
		bowlingPenguins = GameObject.FindGameObjectsWithTag("BowlingPenguin");
		bowlingBalls = GameObject.FindGameObjectsWithTag("BowlingBall");
		for(int i=0; i<bowlingPenguins.Length; i++){
			bowlingPenguins[i].SetActive(false);
		}
		for(int i=0; i<bowlingBalls.Length; i++){
			bowlingBalls[i].SetActive(false);
		}
    }

    // Update is called once per frame
    void Update()
    {
		if(pebbleCount < 10)
			pebbleText.text = pebbleCount + " pebble collected";
		else
			pebbleText.text = "You have collected enough pebbles!";
    }
	
	/*IEnumerator MoveForward()
	{
		while(_isInNav)
		{
			_playerObject.GetComponent<OVRPlayerController>().UpdateMovement();
			yield return null;
		}
	}*/
	
	void OnTriggerEnter(Collider otherCollider)
	{
		//Debug.Log(otherCollider.gameObject.name);
		if(otherCollider.gameObject.name.StartsWith("Rock"))
		{
			//pick up a rock with your beak
			pebbleCount = pebbleCount + 1;
			PebbleMove(otherCollider);
			
			
			if(gameObject.transform.childCount == 0)
			{
				Debug.Log("2");
			//	otherCollider.gameObject.transform.parent = gameObject.transform;
				Rigidbody rb = otherCollider.gameObject.GetComponent<Rigidbody>();
				if(rb != null)
				{
					rb.isKinematic = true;
					rb.detectCollisions = false;
				}
			
				
				//enable the navigationtrigger collider... so that we can drop the rock..
				/*if(navigationTrigger != null)
				{
					navigationTrigger.GetComponent<Collider>().enabled = true;
					navigationTrigger.GetComponent<Rigidbody>().detectCollisions = true;
				}*/
			}
			
			//Debug.Log(otherCollider.gameObject.name);
		}
		
		//picking up bowling ball
		if(otherCollider.gameObject.name.StartsWith("BowlingBall"))
		{
			//pick up a bowling with your beak
			if(gameObject.transform.childCount == 0)
			{
				otherCollider.gameObject.transform.parent = gameObject.transform;
				Rigidbody rb = otherCollider.gameObject.GetComponent<Rigidbody>();
				if(rb != null)
				{
					rb.isKinematic = true;
					rb.detectCollisions = false;
				}
			}
		}

		if(otherCollider.gameObject.name.StartsWith("SlopeTrigger"))
		{
			if(gameObject.transform.childCount != 0)
			{
				GameObject ball = gameObject.transform.GetChild(0).gameObject;
				ball.name = "DetachedBall";
				gameObject.transform.DetachChildren();
				ball.transform.rotation = Camera.main.transform.rotation;
				//ball.transform.rotation = Quaternion.Inverse(gameObject.transform.rotation);
				Rigidbody rb = ball.GetComponent<Rigidbody>();
				if(rb != null)
				{
					rb.isKinematic = false;
					rb.detectCollisions = true;
					rb.AddForce(ball.transform.forward*10000.0f);
					StartCoroutine(BowlingBallGrow(ball));
				}
			}
		}


		//start penguin bowling game when the player touches the attraction with the beak
		if(otherCollider.gameObject.name.StartsWith("BowlingAttraction"))
		{
			for(int i=0; i<bowlingPenguins.Length; i++){
				bowlingPenguins[i].SetActive(true);
			}
			for(int i=0; i<bowlingBalls.Length; i++){
				bowlingBalls[i].SetActive(true);
			}
		}



		
	}
	
	
	void OnTriggerStay(Collider otherCollider)
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
		/*if(otherCollider.gameObject.name == "NavigationTrigger")
		{
			//Debug.Log("Beak left navigation trigger");
			_isInNav = false;
			if(_playerObject != null)
			{
				OVRPlayerController ovrPC = _playerObject.GetComponent<OVRPlayerController>();
				ovrPC.OverrideOculusForward = false;
			}
		}*/
		
		if(otherCollider.gameObject.name == "SqwaukBox")
		{
			_gackTimer = 0f;
		}
		
		//have on trigger exit cause movement?
	}

	

	void PebbleMove(Collider pebble){
		StartCoroutine(MoveToPos(pebble, 1));
	}

	IEnumerator MoveToPos(Collider pebble, float duration){

		Vector3 startPosition = pebble.gameObject.transform.position;
		Vector3 newSpot = pebbleTextObject.transform.position;

		//(xPosition,yPosition,zPosition) = pebble.gameObject.tranform.position;

		//move the pebble to the position of text
		float t = 0f;
		float timeSlice = 0.02f;
		float timeSliceCount = 0f;
		while(t < duration)
		{
			if(timeSliceCount > timeSlice){
				timeSliceCount = 0;
				newSpot = pebbleTextObject.transform.position;
			}
			pebble.gameObject.transform.position = Vector3.Lerp(startPosition, newSpot,  (t/duration) );
			
			t += (Time.deltaTime);
			timeSliceCount += (Time.deltaTime);	
			yield return null;
		}

		//move the pebble to the nest
		yield return new WaitForSeconds(3);
		t = 0f;
		startPosition = pebble.gameObject.transform.position;
		newSpot = nest.transform.position;
		while(t < 3)
		{
			pebble.gameObject.transform.position = Vector3.Lerp(startPosition, newSpot,  (t/duration) );
			
			t += (Time.deltaTime);
			yield return null;
		}
		pebble.gameObject.SetActive(false);

		//If the player havn't collected enough pebbles, continue to construct the nest
		//assume that there are 30 pebbles in total for the nest
		if(pebbleCount <= 10){
			for(int i=0; i<3; i++){
				nestRocks[nestRockCount].SetActive(true);
				nestRockCount += 1;
			}
		}

	}

	IEnumerator BowlingBallGrow(GameObject ball){
		for(int i=0; i<40; i++){
			ball.transform.localScale += new Vector3(0.01f,0.01f,0.01f);
			yield return new WaitForSeconds(0.05f);
		}
	}


}
