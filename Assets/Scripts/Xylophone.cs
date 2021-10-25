using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xylophone : MonoBehaviour
{
    AudioSource audio;


    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider otherCollider){
        if(otherCollider.gameObject.name.StartsWith("Flipper")){
            audio.Play();
        }
    }



}
