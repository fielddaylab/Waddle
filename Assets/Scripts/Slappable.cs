using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Waddle;


public class Slappable : MonoBehaviour, ISlapInteract
{
	
	public enum SlappableType
	{
		ROCK,
		PENGUIN,
		NEST,
		SKUA
	}
	
	public SlappableType _type;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void OnSlapInteract(PlayerHeadState state, SlapTrigger trigger, Collider slappedCollider, Vector3 slapDirection, Collision collisionInfo)
	{
		Vector3 pos = Vector3.zero;
        Quaternion view = Quaternion.identity;
        PenguinPlayer.Instance.GetGaze(out pos, out view);    
		
		if(_type == SlappableType.ROCK)
		{
			//Debug.Log("Slapped rock!");
			PenguinAnalytics.Instance.LogFlipperBashRock(slappedCollider.gameObject.name, (trigger.Flipper == SlapFlipper.Right), slappedCollider.gameObject.transform.position, pos);
		}
		else if(_type == SlappableType.PENGUIN)
		{
			//Debug.Log("Slapped penguin!");
			PenguinAnalytics.Instance.LogFlipperBashPenguin(slappedCollider.gameObject.name, (trigger.Flipper == SlapFlipper.Right), slappedCollider.gameObject.transform.position, pos);
		}
		else if(_type == SlappableType.NEST)
		{
			//Debug.Log("Slapped nest!");
			PenguinAnalytics.Instance.LogFlipperBashNest(slappedCollider.gameObject.name, (trigger.Flipper == SlapFlipper.Right), slappedCollider.gameObject.transform.position, pos);
		}
		else if(_type == SlappableType.SKUA)
		{
			//Debug.Log("Slapped skua!");
			PenguinAnalytics.Instance.LogFlipperBashSkua(slappedCollider.gameObject.name, (trigger.Flipper == SlapFlipper.Right), slappedCollider.gameObject.transform.position, pos);
		}
	}
}
