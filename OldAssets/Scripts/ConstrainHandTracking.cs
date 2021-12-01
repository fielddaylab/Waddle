using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstrainHandTracking : MonoBehaviour
{
	public Vector3 _rotMin;
	public Vector3 _rotMax;
	
	public Vector3 _posMin;
	public Vector3 _posMax;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
		/*Vector3 v = transform.rotation.eulerAngles;
		//Debug.Log(v);
        v.x = Mathf.Clamp(v.x, _rotMin.x, _rotMax.x);
		v.y = Mathf.Clamp(v.y, _rotMin.y, _rotMax.y);
		v.z = Mathf.Clamp(v.z, _rotMin.z, _rotMax.z);
		Quaternion q = transform.rotation;
		q.eulerAngles = v;
		//Debug.Log(v);
		transform.rotation = q;*/
    }
}
