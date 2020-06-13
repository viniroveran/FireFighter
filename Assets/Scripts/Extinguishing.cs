using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Effects;
using UnityStandardAssets.Utility;

public class Extinguishing : MonoBehaviour
{
	public float multiplier = 1f;
	[SerializeField] private float reduceFactor = 0.8f;
	[SerializeField] private GameObject checkbox = null;
	private AudioSource _audioSource;
	
	private GameManager _gameManager;

	// Use this for initialization
	void Start()
	{
		checkbox.SetActive(false);
		ParticleSystemMultiplier sysMul = GetComponent<ParticleSystemMultiplier>();
		multiplier = sysMul.multiplier;
		_audioSource = GetComponent<AudioSource>();
		
		// This is called every time a GameObject with this script is set to active
		// Add fire
		_gameManager = GameManager.instance;
		_gameManager.AddFire();
		// Change tag to "Fire" so we know it won't be lit randomly later
		this.gameObject.tag = "Fire";
	}

	// Update is called once per frame
	void Extinguish()
	{
		multiplier *= reduceFactor;
		_audioSource.volume *= reduceFactor;
		var systems = GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem system in systems)
		{
			ParticleSystem.MainModule mainModule = system.main;
			mainModule.startSizeMultiplier *= reduceFactor;
			mainModule.startSpeedMultiplier *= reduceFactor;
			system.Play();
		}

		if (multiplier <= 0.01f)
		{
			GetComponent<ParticleSystemDestroyer>().Stop();
			checkbox.SetActive(true);
			checkbox.transform.parent = null;

			// Destroy fire after it is extinguished
			_gameManager.DestroyFire();
		}
	}
}
