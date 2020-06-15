using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHose : MonoBehaviour
{
    private AudioSource _audioSource;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Recover Game Manager singleton
        _gameManager = GameManager.instance;
        
        // Recover the component AudioSource
        _audioSource = GetComponent<AudioSource>();
        _audioSource.volume = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && !_gameManager.isPaused)
        {
            // Decrease the amount of water by 1% per second (waterAmountAtStart / 100 = 1%)
            _gameManager.DecreaseWater((_gameManager.waterAmountAtStart / 100f) * Time.deltaTime);
            
            // Increase the volume over time
            _audioSource.volume = Mathf.Lerp(_audioSource.volume, 1, Time.deltaTime * 5);
        }
        else
        {
            // Decrease the volume over time
            _audioSource.volume = Mathf.Lerp(_audioSource.volume, 0, Time.deltaTime * 5);
        }
    }
}