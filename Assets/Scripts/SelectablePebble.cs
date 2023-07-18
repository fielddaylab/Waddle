using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectablePebble : MonoBehaviour
{

    [SerializeField] bool selectable = true;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

    }

    void Update()
    {
        if (selectable)
        {
            float glow = Mathf.Lerp(0, 1, Mathf.PingPong(Time.time, 1));
            rend.material.SetFloat("_SelectGlow", glow);
        }
    }
}
