using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xylophone : MonoBehaviour
{
    AudioSource audioS;

    // Start is called before the first frame update
    void Start()
    {
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider otherCollider){
        if(otherCollider.gameObject.name.StartsWith("Flipper") || otherCollider.gameObject.name.StartsWith("Beak")){
            audioS.Play();
        }
    }
}
