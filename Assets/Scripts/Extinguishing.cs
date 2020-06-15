using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Effects;
using UnityStandardAssets.Utility;

public class Extinguishing : MonoBehaviour
{
	[SerializeField] private float reduceFactor = 0.8f;
	[SerializeField] private float increaseFactor = 1.2f;
	[SerializeField] private GameObject checkbox = null;
	[SerializeField] private AudioSource clearedSoundSource;
	[SerializeField] private AudioClip clearedSound;
	
	private AudioSource _audioSource;
	private GameManager _gameManager;
	private float _fireTimer;
	
	public float multiplier = 1f;

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
	private void Update()
	{
		_fireTimer += Time.deltaTime;

		if (_fireTimer > 1 && multiplier < 2)
		{
			ChangeFireSize(increaseFactor);

			if (multiplier >= 2)
			{
				_fireTimer = 0;
			}
		}
	}

	private void ChangeFireSize(float factor)
	{
		multiplier *= factor;
		_audioSource.volume *= factor;
		
		var systems = GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem system in systems)
		{
			ParticleSystem.MainModule mainModule = system.main;
			mainModule.startSizeMultiplier *= factor;
			mainModule.startSpeedMultiplier *= factor;
			system.Play();
		}
	}
	
	void Extinguish()
	{
		// Reduce fire size because it is being extinguished
		ChangeFireSize(reduceFactor);
		
		// Fire is being extinguished, reset timer
		_fireTimer = 0;
		
		if (multiplier <= 0.01f)
		{
			GetComponent<ParticleSystemDestroyer>().Stop();
			checkbox.SetActive(true);
			checkbox.transform.parent = null;
			
			// Play fire cleared sound
			clearedSoundSource.PlayOneShot(clearedSound);

			// Destroy fire after it is extinguished
			_gameManager.DestroyFire();
		}
	}
}
