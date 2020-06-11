using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] levels = null; // List of all available levels
    
    private int _fires = 0; // Number of active fires
    private int _currentLevel = 0; // Current level index
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
            LoadLevel(); // Load the next level
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
}
