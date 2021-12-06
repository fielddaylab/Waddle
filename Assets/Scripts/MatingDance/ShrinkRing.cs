using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkRing : MonoBehaviour
{
    bool _isShrinking = false;
    bool _isValidWindow = false;
	bool _wasPopped = false;
	
	public bool IsValidWindow => _isValidWindow;
	
	IEnumerator _coroutine = null;
	
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
			_isValidWindow = false;
			_coroutine = Shrink(2f);
            StartCoroutine(_coroutine);
        }
    }
	
	void HideBubble()
	{
		gameObject.transform.parent.gameObject.GetComponent<MeshRenderer>().enabled = false;
		gameObject.GetComponent<MeshRenderer>().enabled = false;
	}

	IEnumerator DestroyCo(float duration)
	{
		yield return new WaitForSeconds(duration);
		
		Object.Destroy(gameObject.transform.parent.gameObject);
	}
	
    IEnumerator Shrink(float duration)
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.one;

        float t = 0f;
		//bool bSet = false;
		
        while(t < (duration + 0.5f))
        {
            if(t < duration)
            {
                transform.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            }
			
            if(t >= duration - 0.5f)
            {
                _isValidWindow = true;
				
            }
            else if((t >= duration && t < (duration + 0.5f)))
            {
                //only need to set this once...
				gameObject.transform.parent.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
				//bSet = true;
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
			
			HideBubble();
			
			//destroy the bubble
			StartCoroutine(DestroyCo(5f));
		}
    }

	public void Popped()
	{
		_wasPopped = true;
		if(_coroutine != null)
		{
			StopCoroutine(_coroutine);
		}
		
		HideBubble();
		
		StartCoroutine(DestroyCo(5f));
	}
}
