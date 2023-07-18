using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectablePebble : MonoBehaviour
{
    [SerializeField] bool selectable = true;
    private Renderer rend;

    private Material material;

    void Start()
    {
        rend = GetComponent<Renderer>();
        material = rend.material;
    }

    private void OnDestroy() {
        Destroy(material);
    }

    void Update()
    {
        if (selectable) {
            float glow = Mathf.Lerp(0, 1, Mathf.PingPong(Time.time, 1));
            material.SetFloat("_SelectGlow", glow);
        }
    }

    public void SetEnabled(bool enabled) {
        selectable = enabled;
        if (!enabled) {
            material.SetFloat("_SelectGlow", 0);
        }
    }
}
