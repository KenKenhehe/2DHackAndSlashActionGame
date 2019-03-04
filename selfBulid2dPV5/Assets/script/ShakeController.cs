using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeController : MonoBehaviour {
    public Animator camShake;
    public float smoothness;
    WaitForSeconds shakeDuration = new WaitForSeconds(.1f);
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void CamShake()
    {
        camShake.SetTrigger("Shake");
    }

   
}
