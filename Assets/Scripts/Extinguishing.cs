using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Effects;
using UnityStandardAssets.Utility;

public class Extinguishing : MonoBehaviour {
	public float multiplier = 1f;
	[SerializeField] private float reduceFactor = 0.8f;
	[SerializeField] private GameObject checkbox = null;
	private AudioSource audioS;

	// Use this for initialization
	void Start () {
		checkbox.SetActive(false);
		ParticleSystemMultiplier sysMul = GetComponent<ParticleSystemMultiplier>();
		multiplier = sysMul.multiplier;
		audioS = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Extinguish () {
		multiplier *= reduceFactor;
		audioS.volume *= reduceFactor;
		 var systems = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem system in systems)
            {
				ParticleSystem.MainModule mainModule = system.main;
				mainModule.startSizeMultiplier *= reduceFactor;
                mainModule.startSpeedMultiplier *= reduceFactor;
                //mainModule.startLifetimeMultiplier *= Mathf.Lerp(multiplier, 1, 0.5f);
                //system.Clear();
                system.Play();
            }
		if (multiplier <= 0.01f) {
			GetComponent<ParticleSystemDestroyer>().Stop();
			checkbox.SetActive(true);
			checkbox.transform.parent = null;
		}
	}
}
