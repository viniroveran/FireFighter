﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text txtFire;
    [SerializeField] private Text txtWater;
    [SerializeField] private Text txtBuildingDamage;
    [SerializeField] private Text txtTimer;
    [SerializeField] private int preventionScore = 10;
    [SerializeField] private int pointsPerLevelComplete = 200;
    [SerializeField] private int timeScore = 50;
    [SerializeField] private int waterScore = 1000;
    [SerializeField] private float damagePerSecond = 0.5f; // Increase 0.5% of damage per active fire every second
    [SerializeField] private GameObject summaryScreen; // Summary screen
    [SerializeField] private GameObject victoryScreen; // Victory screen
    [SerializeField] private AudioSource audioSource; // Hose sound
    [SerializeField] private GameObject[] levels = null; // List of all available levels

    private int _fires = 0; // Number of active fires
    private int _totalFires = 0; // Number of total fires activated
    private static int _currentLevel = 0; // Current level index
    private float _points = 0; // Player points
    private float _startTime; // Start time
    private float _timeElapsed = 0; // Time elapsed till now
    private bool _victory = true; // If player has won or not
    private float _waterAmountPercentage = 100f; // Percentage of water amount
    private GameObject _levelObj; // Container for the current level
    private AsyncOperation _asyncOperation;
    
    // Building damage
    private float _buildingDamage = 0;
    
    // Scoring
    private string _pointsForCompletingLevel;
    private string _pointsForTime;
    private string _bonusPoints;
    private string _penaltyPoints;
    private string _waterPoints;
    private string _totalPoints;
    private bool _newRecord;
    [SerializeField] private Text txtVictory;
    [SerializeField] private Text txtPointsLevelPassed;
    [SerializeField] private Text txtPointsTime;
    [SerializeField] private Text txtPointsBonus;
    [SerializeField] private Text txtPointsPenalty;
    [SerializeField] private Text txtPointsWater;
    [SerializeField] private Text txtBestScore;
    [SerializeField] private Text txtPointsTotal;
    [SerializeField] private string scoreFormat = "000";
    [SerializeField] private Button summaryScreenButtonExit;
    [SerializeField] private Button summaryScreenButtonRestart;
    [SerializeField] private Text summaryScreenButtonExitText;
    [SerializeField] private Text summaryScreenButtonRestartText;

    public static GameManager instance = null; // Reference to the singleton
    [HideInInspector] public int averageTime; // Average time to complete a level, determined by FindFires script, on level prefab
    [HideInInspector] public float waterAmount; // Water amount for the level, determined by FindFires script, on level prefab
    [HideInInspector] public float waterAmountAtStart; // Water amount at level start
    [HideInInspector] public bool isPaused; // Is the game paused?

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

        // Reset points
        _points = 0f;
        
        // Reset building damage
        _buildingDamage = 0f;
        txtBuildingDamage.text = _buildingDamage.ToString("00") + "%";
        
        // Reset Water Amount Percentage
        _waterAmountPercentage = 100f;
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
        _newRecord = false;
        
        // Add points if player has won
        if (_victory)
        {
            // Add points for completing level
            _points += levelPoints;
            _pointsForCompletingLevel = levelPoints.ToString(scoreFormat);
            
            // Add points for time, average time per fire: 5 seconds
            // (20 / 20) * 50 = 50
            _points += ((averageTime) / _timeElapsed) * timeScore;
            _pointsForTime = ((averageTime / _timeElapsed) * timeScore).ToString(scoreFormat);
            //Debug.Log("Points for time: " + _pointsForTime + "Time elapsed: " + _timeElapsed);
        
            // Add bonus points per unlit fire
            _points += ((14f - _totalFires) * preventionScore);
            _bonusPoints = ((14f - _totalFires) * preventionScore).ToString(scoreFormat);
        
            // Add points for amount of water remaining
            _points += ((int)_waterAmountPercentage * waterScore);
            _waterPoints = ((int)_waterAmountPercentage * waterScore).ToString(scoreFormat);
        
            // Add penalty points
            _points -= 10f * _buildingDamage;
            _penaltyPoints = (10f * _buildingDamage).ToString(scoreFormat);
            
            // Add score to best score if score > BestScore[levelIndex]
            if (_points > PlayerPrefs.GetFloat("BestScore[" + _currentLevel + "]"))
            {
                _newRecord = true;
                PlayerPrefs.SetFloat("BestScore[" + _currentLevel + "]", _points);
            
                // Save changes to the disk
                PlayerPrefs.Save();
            }
        }
        // Points = 0 if player lost
        else
        {
            _pointsForCompletingLevel = "0";
            _pointsForTime = "0";
            _bonusPoints = "0";
            _waterPoints = "0";
            _penaltyPoints = "0";
        }

        if (_points < 0f)
        {
            _points = 0f;
        }

        //Debug.Log("Points: " + _points.ToString(scoreFormat));
    }

    public void Exit()
    {
        #if UNITY_STANDALONE
        Application.Quit();
        #endif

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void LoadMainMenu()
    {
        // If haven't tried to load anything yet
        if (_asyncOperation == null)
        {
            _asyncOperation = SceneManager.LoadSceneAsync("MainMenu"); // Load Main Menu
            _asyncOperation.allowSceneActivation = true;
        }
    }

    public void Pause(bool paused)
    {
        // Clear all listeners to avoid duplicate/not wanted listener
        summaryScreenButtonExit.onClick.RemoveAllListeners();
        summaryScreenButtonRestart.onClick.RemoveAllListeners();
        
        if (paused)
        {
            isPaused = true;
            Time.timeScale = 0f;
            audioSource.volume = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            summaryScreen.SetActive(false);
            isPaused = false;
            Time.timeScale = 1.0f;
            audioSource.volume = 1.0f;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }

    private void LoadVictoryScreen()
    {
        Pause(true);
        victoryScreen.SetActive(true);
        _currentLevel = 0;
    }

    public void ToggleSummary(bool activate)
    {
        if (activate)
        {
            summaryScreen.SetActive(true);
            //Debug.Log("Enabling summary");
            Pause(true);
            
            txtPointsTime.text = "Time: + " + _pointsForTime;
            txtPointsBonus.text = "Bonus Points: + " + _bonusPoints;
            txtPointsPenalty.text = "Penalty: - " + _penaltyPoints;
            txtPointsWater.text = "Water Left: + " + _waterPoints;
            txtPointsTotal.text = "Total: " + _points.ToString(scoreFormat);
            
            // If there is a Best Score for the current level, display it
            if (PlayerPrefs.HasKey("BestScore[" + _currentLevel + "]"))
            {
                txtBestScore.text = "Best Score: " + PlayerPrefs.GetFloat("BestScore[" + _currentLevel + "]").ToString(scoreFormat);
            }
            
            if (_victory)
            {
                // Display best score in red if player did not set a new best score
                txtBestScore.color = Color.red;
                txtVictory.text = "Level Passed!";
                if (_newRecord)
                {
                    txtVictory.text = "Level Passed! New Record!";
                    // Display best score in green if player just set a new best score
                    txtBestScore.color = Color.green;
                }
                txtVictory.color = Color.green;
                txtPointsLevelPassed.text = "Level passed: + " + _pointsForCompletingLevel;
                summaryScreenButtonExitText.text = "Exit Game";
                summaryScreenButtonExit.onClick.AddListener(Exit);
                summaryScreenButtonRestart.onClick.AddListener(delegate { ToggleSummary(false); });
                
                Debug.Log("_currentLevel: " + _currentLevel + "levels.Length: " + levels.Length);
                // If we are not on last level
                if (_currentLevel < levels.Length - 1)
                {
                    summaryScreenButtonRestartText.text = "Next Level";
                    _currentLevel++;
                    summaryScreenButtonRestart.onClick.AddListener(delegate { LoadLevel(_currentLevel); });
                    Debug.Log("Loading Level (Next Level Button): " + _currentLevel);
                    
                }
                // If we are on last level, proceed to victory screen
                else
                {
                    summaryScreenButtonRestartText.text = "Next";
                    summaryScreenButtonRestart.onClick.AddListener(LoadVictoryScreen);
                    Debug.Log("Loading Victory Screen");
                }
            }
            else
            {
                txtVictory.text = "Game Over!";
                txtVictory.color = Color.red;
                txtPointsLevelPassed.text = "Level failed: + 0";
                
                summaryScreenButtonExitText.text = "Main Menu";
                summaryScreenButtonExit.onClick.AddListener(delegate { LoadMainMenu(); });
                
                summaryScreenButtonRestartText.text = "Restart";
                summaryScreenButtonRestart.onClick.AddListener(delegate { ToggleSummary(false); });
                _currentLevel = 0;
                summaryScreenButtonRestart.onClick.AddListener(delegate { LoadLevel(_currentLevel); });
            }
        }
        else
        {
            Pause(false);
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
        //Debug.Log("Fire destroyed, fires: " + _fires);

        // Victory if _fires <= 0
        if (_fires <= 0)
        {
            // Stop the clock now that we finished the level
            // Time.timeScale = 0;
            
            FinishLevel(true);
        }
    }

    public void LoadLevel()
    {
        // Initialize level to make sure everything is how it is supposed to be
        Invoke(nameof(Init), 0);

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
        Debug.Log("Loading Level: " + _currentLevel);
    }

    public void LoadLevel(int levelIndex)
    {
        // Initialize level to make sure everything is how it is supposed to be
        Invoke(nameof(Init), 0);
        
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
        _levelObj = Instantiate(levels[levelIndex]);
        Debug.Log("Loading Level (parameters): " + levelIndex);
    }

    // Implements all methods called when the player wins or loses
    private void FinishLevel(bool victory)
    {
        // Player lost, _victory = false
        _victory = victory;
            
        // Add points for completing the level
        AddPoints(pointsPerLevelComplete); 
            
        // Toggle Summary screen
        ToggleSummary(true);
    }

    public void DecreaseWater(float amount)
    {
        waterAmount -= amount;
        _waterAmountPercentage = (waterAmount * 100) / waterAmountAtStart;
        if (waterAmount <= 0)
        {
            FinishLevel(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadLevel(); // Load the first level
    }

    private void Update()
    {
        // Update number of active fires on UI
        txtFire.text = _fires.ToString();
        
        // Update water amount on UI
        txtWater.text = _waterAmountPercentage.ToString("0") + "%";

        // Store how much time passed since level was loaded
        _timeElapsed = Time.time - _startTime;
        
        // Displays elapsed time on UI
        string minutes = ((int) _timeElapsed / 60).ToString();
        string seconds = (_timeElapsed % 60).ToString("00");
        txtTimer.text = minutes + ":" + seconds;
        
        // Increase building damage by 1% per fire every second
        // Fix for FinishLevel being called infinite times because of buildingDamage > 100
        if (_buildingDamage <= 100)
        {
            _buildingDamage += (damagePerSecond * _fires) * Time.deltaTime;
        }
        else
        {
            _buildingDamage = 100;
        }
        txtBuildingDamage.text = _buildingDamage.ToString("0") + "%";
    }

    private void LateUpdate()
    {
        // Game over if _buildingDamage > 100
        if (_buildingDamage > 100)
        {
            FinishLevel(false);
        }
        
        // Game over if waterAmountPercentage < 0
        // if (waterAmount < 0)
        // {
        //     FinishLevel(false);
        // }
    }
}