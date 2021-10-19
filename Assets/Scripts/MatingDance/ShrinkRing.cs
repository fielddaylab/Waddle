using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkRing : MonoBehaviour
{
    bool _isShrinking = false;
    bool _isValidWindow = false;

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
            StartCoroutine(Shrink(10f));
        }
    }

    IEnumerator Shrink(float duration)
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.one;

        float t = 0f;

        while(t < duration + 0.5f)
        {
            if(t < duration)
            {
                transform.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            }
            t += UnityEngine.Time.deltaTime;
            if(t < duration - 0.5f)
            {
                _isValidWindow = true;
            }
            else if(t > duration && t < duration + 0.5f)
            {
                //make bubbble glow here...
            }
            yield return null;
        }

        _isValidWindow = false;
        
        //destroy the bubble?
    }

}
