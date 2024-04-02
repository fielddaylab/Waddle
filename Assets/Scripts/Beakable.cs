using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Waddle;


public class Beakable : MonoBehaviour, IBeakInteract
{
	
	public enum BeakableType
	{
		ROCK,
		PENGUIN,
		NEST,
		SKUA
	}
	
	public BeakableType _type;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void OnBeakInteract(PlayerBeakState state,  BeakTrigger trigger, Collider collider)
	{
		//Vector3 pos = Vector3.zero;
        //Quaternion view = Quaternion.identity;
        //PenguinPlayer.Instance.GetGaze(out pos, out view);    
		
		if(_type == BeakableType.ROCK)
		{
			//Debug.Log("Beaked rock!");
			PenguinAnalytics.Instance.LogPeckRock(collider.gameObject.name, collider.gameObject.transform.position);
		}
		else if(_type == BeakableType.PENGUIN)
		{
			//Debug.Log("Beaked penguin!");
			PenguinAnalytics.Instance.LogPeckPenguin(collider.gameObject.name, collider.gameObject.transform.position);
		}
		else if(_type == BeakableType.NEST)
		{
			//Debug.Log("Beaked nest!");
			PenguinAnalytics.Instance.LogPeckNest(collider.gameObject.name, collider.gameObject.transform.position);
		}
		else if(_type == BeakableType.SKUA)
		{
			//Debug.Log("Beaked skua!");
			PenguinAnalytics.Instance.LogPeckSkua(collider.gameObject.name, collider.gameObject.transform.position);
		}
	}
}
