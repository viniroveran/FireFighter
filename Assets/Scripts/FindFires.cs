using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FindFires : MonoBehaviour
{
    [SerializeField] private int timeToAddRandomFire = 20; // Time in seconds to lit a random fire
    [SerializeField] private GameObject[] availableFires;
    [SerializeField] private int averageTimeForLevel;
    [SerializeField] private int waterAmountForLevel;

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
        // Debug.Log("Fires: " + availableFires.Length + 1);
        
        // Set average time for level (User Story 3.2)
        _gameManager = GameManager.instance;
        _gameManager.averageTime = averageTimeForLevel;
        
        // Set water amount for level (User Story 5.3)
        _gameManager.waterAmount = waterAmountForLevel;
        _gameManager.waterAmountAtStart = waterAmountForLevel;
    }

    // Update is called once per frame
    void Update()
    {
        _timerRandomFire += Time.deltaTime;
        // Only lit up a random fire if there is any to be lit up
        if (_neverActiveFires.Count > 0)
        {
            _randomFireIndex = Random.Range(0, _neverActiveFires.Count);
            if (_timerRandomFire > timeToAddRandomFire)
            {
                // Light up random fire
                _neverActiveFires = _gameManager.LitRandomFire(_neverActiveFires, _randomFireIndex);
                
                // Reset timer so we can activate another random fire in 20 seconds
                _timerRandomFire = 0;
            }
        }
    }
}
