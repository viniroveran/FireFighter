using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindFires : MonoBehaviour
{
    [SerializeField] private int _timeToAddRandomFire = 20; // Time in seconds to lit a random fire
    [SerializeField] private GameObject[] availableFires;
    
    private List<GameObject> _neverActiveFires = new List<GameObject>();
    private float _timerRandomFire = 0; // Timer to lit a random fire
    private int _randomFireIndex;
    private GameManager _gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        //GameObject[] availableFires = GameObject.FindGameObjectsWithTag("FireNeverActive");
        foreach (var fire in availableFires)
        {
            if (fire.CompareTag("FireNeverActive"))
            {
                // Debug.Log("Found: " + i++);
                _neverActiveFires.Add(fire);
            }
        }
        // Debug.Log("Fires: " + fires.Count);
    }

    // Update is called once per frame
    void Update()
    {
        _timerRandomFire += Time.deltaTime;
        // Only lit up a random fire if there is any to be lit up
        if (_neverActiveFires.Count > 0)
        {
            _randomFireIndex = Random.Range(0, _neverActiveFires.Count);
            if (_timerRandomFire > _timeToAddRandomFire)
            {
                // Light up random fire
                _gameManager = GameManager.instance;
                _neverActiveFires = _gameManager.LitRandomFire(_neverActiveFires, _randomFireIndex);
                
                // Reset timer so we can activate another random fire in 20 seconds
                _timerRandomFire = 0;
            }
        }
    }
}
