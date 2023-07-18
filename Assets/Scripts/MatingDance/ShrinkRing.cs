//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkRing : MonoBehaviour
{
    bool _isShrinking = false;
    bool _isValidWindow = false;
    bool _wasPopped = false;

    public bool WasPopped => _wasPopped;
    public bool IsValidWindow => _isValidWindow;

    float _timing = 0f;

    public float GetTiming() => _timing;

    int _whichBubble = -1;

    public int GetWhichBubble() => _whichBubble;
    public void SetWhichBubble(int b) { _whichBubble = b; }

    IEnumerator _coroutine = null;

    Color _orange = new Color(1f, 0.0f, 0.592f, 0.27f);

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (!_isShrinking) {
            _isShrinking = true;
            _isValidWindow = false;
            _coroutine = Shrink(.75f);
            StartCoroutine(_coroutine);
        }
    }

    void HideBubble() {
        gameObject.transform.parent.gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.transform.parent.gameObject.GetComponent<Collider>().enabled = false;
    }

    IEnumerator DestroyCo(float duration) {
        yield return new WaitForSeconds(duration);

        Object.Destroy(gameObject.transform.parent.gameObject);
    }

    IEnumerator Shrink(float duration) {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.one;

        float t = 0f;
        bool bSet = false;

        while (t < (duration + 0.5f)) {
            if (t < duration) {
                transform.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            }

            if (t >= duration - 0.5f) {
                _isValidWindow = true;

            }

            if ((t >= duration && t < (duration + 0.5f)) && !bSet) {
                //only need to set this once...
                //gameObject.transform.parent.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                gameObject.transform.parent.GetComponent<MeshRenderer>().material.SetColor("_BaseColor", _orange);
                bSet = true;
            }

            t += UnityEngine.Time.deltaTime;

            if (_isValidWindow) {
                _timing = t - duration;
            }

            yield return null;
        }


        // if routine made it this far, bubble was not popped
        _isValidWindow = false;

        if (!_wasPopped) {
            AudioSource audio = GetComponent<AudioSource>();
            if (audio != null) {
                audio.PlayOneShot(audio.clip);
            }

            MatingDance._popCount = 0;
            PenguinAnalytics.Instance.LogBubbleDisappeared(_whichBubble);

            HideBubble();

            StartCoroutine(DestroyCo(5f));
        }
    }

    public void Popped() {
        _wasPopped = true;
        if (_coroutine != null) {
            StopCoroutine(_coroutine);
        }

        MatingDance._popCount++;

        HideBubble();

        StartCoroutine(DestroyCo(5f));
    }
}
