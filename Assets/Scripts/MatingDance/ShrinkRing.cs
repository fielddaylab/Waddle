using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkRing : MonoBehaviour
{
    bool _isShrinking = false;
    bool _isValidWindow = false;
	bool _wasPopped = false;
	
	public bool IsValidWindow => _isValidWindow;
	
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isShrinking)
        {
            _isShrinking = true;
            StartCoroutine(Shrink(1.5f));
        }
    }

    IEnumerator Shrink(float duration)
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.one;

        float t = 0f;

        while(t < (duration + 0.5f))
        {
            if(t < duration)
            {
                transform.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            }
			
            if(t < duration - 0.5f)
            {
                _isValidWindow = true;
				
            }
            else if(t >= duration && t < duration + 0.5f)
            {
                //only need to set this once...
				gameObject.transform.parent.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
            }
			
			t += UnityEngine.Time.deltaTime;
			
            yield return null;
        }

        _isValidWindow = false;
        
		if(!_wasPopped)
		{
			AudioSource audio = GetComponent<AudioSource>();
			if(audio != null)
			{
				audio.Play();
			}
			
			//destroy the bubble
			DestroyObject(gameObject.transform.parent.gameObject);
		}
    }

	public void Popped()
	{
		_wasPopped = true;
		DestroyObject(gameObject.transform.parent.gameObject);
	}
}
