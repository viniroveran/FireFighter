using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    private AsyncOperation _asyncOperation;
    
    // Load game scene
    public void LoadNextScene()
    {
        // If haven't tried to load anything yet
        if (_asyncOperation == null)
        {
            // Restore the current scene
            Scene currentScene = SceneManager.GetActiveScene();
            // Load next scene
            _asyncOperation = SceneManager.LoadSceneAsync(currentScene.buildIndex + 1);
            _asyncOperation.allowSceneActivation = true;
        }
    }
}
