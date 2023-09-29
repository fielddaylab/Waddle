//NSF Penguins VR Experience
//Ross Tredinnick - WID Virtual Environments Group / Field Day Lab - 2021

using System.Collections;
using System.Collections.Generic;
using BeauRoutine;
using UnityEngine;
using Waddle;

public class SkuaHitState : MonoBehaviour, ISkuaState
{
	private SkuaController _sc;
	
	Camera _mainCam = null;
    // Start is called before the first frame update
    void Start()
    {
        _mainCam = Camera.main;
    }
	
	public void Handle(SkuaController sc)
	{
		if(_sc == null)
		{
			_sc = sc;
		}
		
		//adjust position of egg...
		
		Animator a = sc.GetAnimController();
		if(a != null)
		{

			//a.SetBool("walk", false);
			//a.SetBool("eat", false);
			a.SetBool("idle", false);
			a.SetBool("slapped", true);
			a.SetBool("break", false);
			
			gameObject.GetComponent<Rigidbody>().useGravity = true;
			gameObject.GetComponent<Rigidbody>().isKinematic = false;
			
			PenguinAnalytics.Instance.LogFlipperBash(sc.gameObject.name, false);

			//a.enabled = false;
			
            SFXUtility.Play(sc.Sounds, sc.HitSound);
			GetComponent<Rigidbody>().AddForce((_mainCam.transform.forward*3f + transform.up*6), ForceMode.Impulse);

            Routine.Start(this, FlashRoutine());
		}
	}

    private IEnumerator FlashRoutine() {
        int count = 4;
        SkinnedMeshRenderer meshRenderer = _sc.Renderer;
        Material defaultMaterial = meshRenderer.sharedMaterial;
        try {
            while (count-- > 0) {
                meshRenderer.sharedMaterial = _sc.FlashMaterial;
                yield return 0.1f;
                meshRenderer.sharedMaterial = defaultMaterial;
                yield return 0.1f;
            }
        } finally {
            if (meshRenderer) {
                meshRenderer.sharedMaterial = defaultMaterial;
            }
        }
    }
}
