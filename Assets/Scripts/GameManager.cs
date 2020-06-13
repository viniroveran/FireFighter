using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levels = null; // List of all available levels
    [SerializeField] private Text txtFire;
    [SerializeField] private Text txtTimer;
    [SerializeField] private int preventionScore = 10;
    
    private int _fires = 0; // Number of active fires
    private int _totalFires = 0; // Number of total fires activated
    private int _currentLevel = 0; // Current level index
    private int _points = 0; // Player points
    private float _startTime; // Start time
    private float _timeElapsed = 0; // Time elapsed till now
    private GameObject _levelObj; // Container for the current level

    public static GameManager instance = null; // Reference to the singleton

    private void Awake()
    {
        // Singleton implementation
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this);
        }
    }

    // Initialize the level, disabling all checkboxes
    void Init()
    {
        GameObject[] checkboxes = GameObject.FindGameObjectsWithTag("Checkbox");

        foreach (var checkbox in checkboxes)
        {
            checkbox.SetActive(false);
        }
    }

    public void Death()
    {
        Debug.Log("Death");
    }

    public void AddFire()
    {
        // Increase the number of active fires
        _fires++;
        // Increase the number of total fires
        _totalFires++;
    }

    public void AddPoints()
    {
        _points += ((14 - _totalFires) * preventionScore);
        Debug.Log("Points: " + _points);
    }

    public List<GameObject> LitRandomFire(List<GameObject> neverActiveFires, int index)
    {
        // Light up a random fire
        neverActiveFires[index].SetActive(true);
        // Remove from list of never active fires
        neverActiveFires.Remove(neverActiveFires[index]);
        
        return neverActiveFires;
    }

    public void DestroyFire()
    {
        // Decrease the number of active fires
        _fires--;

        if (_fires <= 0)
        {
            // Last level?
            if (_currentLevel < levels.Length - 1)
            {
                _currentLevel++;
            }
            else
            {
                Debug.Log("Levels ended, repeating...");
            }
            Init(); // Set all checkboxes to disabled
            Invoke(nameof(LoadLevel), 0.1f); // Load the next level after a 0.1 second delay to make sure all fires are ok
            _startTime = Time.time; // Reset the level Timer
        }
    }

    private void LoadLevel()
    {
        // If old level is loaded
        if (_levelObj)
        {
            // Destroy it
            Destroy(_levelObj);
        }
        // Clear any value in the fire counter
        _fires = 0;
        // Copy the level at position _currentLevel
        _levelObj = Instantiate(levels[_currentLevel]);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadLevel(); // Load the first level
    }

    private void Update()
    {
        txtFire.text = _fires.ToString();
        _timeElapsed = Time.time - _startTime;

        string minutes = ((int) _timeElapsed / 60).ToString();
        string seconds = (_timeElapsed % 60).ToString("00");

        txtTimer.text = minutes + ":" + seconds;
    }
}
