using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCurrentLevel : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefsManager.levelNumber == 0)
        {
            PlayerPrefsManager.levelNumber = 1;
            PlayerPrefsManager.UpdateLevelNumber();
        }

        // if (PlayerPrefsManager.noAds == null || PlayerPrefsManager.noAds == 0)
        // {
        //     PlayerPrefsManager.noAds = 0;
        //     PlayerPrefsManager.UpdateAds();
        // }
    }

    void Update()
    {
        SceneManager.LoadScene(PlayerPrefsManager.levelNumber); 
    }
}
