using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Effects;

public class WaterHose : MonoBehaviour {

	private AudioSource audioS;
	private Hose hoseScript ;


	void Start () {
		audioS = GetComponent<AudioSource>();
		audioS.volume = 0;
	}

	void Update () {
		if (Input.GetButton("Fire1")){
			audioS.volume =  Mathf.Lerp(audioS.volume,1,Time.deltaTime*5);
		}else{
			audioS.volume =  Mathf.Lerp(audioS.volume,0,Time.deltaTime*5);
		}
	}

}
