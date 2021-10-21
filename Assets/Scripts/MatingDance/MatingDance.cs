using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatingDance : MonoBehaviour
{
    // Start is called before the first frame update
    float[] _sampleData = null;
    
    [SerializeField]
    GameObject _bubblePrefab=null;

    void Start()
    {
        Debug.Log(GetComponent<AudioSource>().clip.samples);
        _sampleData = new float[GetComponent<AudioSource>().clip.samples];
        GetComponent<AudioSource>().clip.GetData(_sampleData, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //about 44,100 samples per second...
        //Debug.Log(_sampleData[GetComponent<AudioSource>().timeSamples].ToString("F4"));
        //pre-define a set of samples for when we need to spawn a bubble, based on the music...
        /*if(_sampleData[GetComponent<AudioSource>().timeSamples] > 0.5f)
        {
            if(_bubblePrefab != null)
            {
                GameObject.Instantiate(_bubblePrefab);
            }
        }*/
    }
}
