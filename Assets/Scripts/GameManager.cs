using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text txtFire;
    [SerializeField] private Text txtTimer;
    [SerializeField] private int preventionScore = 10;
    [SerializeField] private int pointsPerLevelComplete = 200;
    [SerializeField] private int timeScore = 50;
    [SerializeField] private GameObject summaryScreen;
    [SerializeField] private AudioSource _audioSource; // Hose sound
    [SerializeField] private GameObject[] levels = null; // List of all available levels

    private int _fires = 0; // Number of active fires
    private int _totalFires = 0; // Number of total fires activated
    private int _currentLevel = 0; // Current level index
    private float _points = 0; // Player points
    private float _startTime; // Start time
    private float _timeElapsed = 0; // Time elapsed till now
    private bool _victory = true; // If player has won or not
    private GameObject _levelObj; // Container for the current level
    
    // Scoring
    public int averageTime;
    private string _pointsForCompletingLevel;
    private string _pointsForTime;
    private string _bonusPoints;
    private string _totalPoints;
    [SerializeField] private Text txtVictory;
    [SerializeField] private Text txtPointsLevelPassed;
    [SerializeField] private Text txtPointsTime;
    [SerializeField] private Text txtPointsBonus;
    [SerializeField] private Text txtPointsTotal;
    [SerializeField] private string scoreFormat = "000";

    private static int _initialActiveFires = 0;

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
        // Find all active checkboxes and disable them
        GameObject[] checkboxes = GameObject.FindGameObjectsWithTag("Checkbox");
        foreach (var checkbox in checkboxes)
        {
            checkbox.SetActive(false);
        }
        
        // Save how many active fires we had at the beginning
        GameObject[] initiallyActiveFires = GameObject.FindGameObjectsWithTag("Fire");
        _initialActiveFires = initiallyActiveFires.Length + 1;
        
        // Reset points
        _points = 0;
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

    private void AddPoints(int levelPoints)
    {
        // Add Level Points only if player has won
        if (_victory)
        {
            _points += levelPoints;
            _pointsForCompletingLevel = levelPoints.ToString(scoreFormat);
        }
        else
        {
            _pointsForCompletingLevel = "0";
        }
        
        // Add points for time, average time per fire: 5 seconds
        _points += ((averageTime) / _timeElapsed) * timeScore;
        // (20 / 20) * 50 = 50
        _pointsForTime = ((averageTime / _timeElapsed) * timeScore).ToString(scoreFormat);
        Debug.Log("Points for time: " + _pointsForTime + "Time elapsed: " + _timeElapsed);
        // Add bonus points per unlit fire
        _points += ((14 - _totalFires) * preventionScore);
        _bonusPoints = ((14 - _totalFires) * preventionScore).ToString(scoreFormat);
        
        Debug.Log("Points: " + _points.ToString(scoreFormat));
    }

    public void ToggleSummary(bool activate)
    {
        if (activate)
        {
            summaryScreen.SetActive(true);
            Debug.Log("Enabling summary");
            
            if (_victory)
            {
                txtVictory.text = "Victory!";
                txtPointsLevelPassed.text = "Level passed: + " + _pointsForCompletingLevel;
            }
            else
            {
                txtVictory.text = "Defeat!";
                txtPointsLevelPassed.text = "Level passed: + 0";
            }
            
            txtPointsTime.text = "Points for time: + " + _pointsForTime;
            txtPointsBonus.text = "Bonus Points: + " + _bonusPoints;
            txtPointsTotal.text = "Total: " + _points.ToString(scoreFormat);
            
            Time.timeScale = 0;
            _audioSource.volume = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            summaryScreen.SetActive(false);
            Time.timeScale = 1;
            _audioSource.volume = 1;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
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
        Debug.Log("Fire destroyed, fires: " + _fires);

        if (_fires <= 0)
        {
            // Stop the clock now that we finished the level
            // Time.timeScale = 0;
            // Add points for completing the level
            AddPoints(pointsPerLevelComplete); 
            // Toggle Summary screen
            ToggleSummary(true);
            // Last level?
            if (_currentLevel < levels.Length - 1)
            {
                _currentLevel++;
            }
            else
            {
                Debug.Log("Levels ended, repeating...");
            }
            Invoke(nameof(Init), 0); // Set all checkboxes to disabled
            Invoke(nameof(LoadLevel), 0.1f); // Load the next level after a 0.1 second delay to make sure all fires are ok
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
        // Reset the level Timer
        _startTime = Time.time;
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
