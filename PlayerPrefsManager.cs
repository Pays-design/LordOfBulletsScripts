using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    public const string LevelNumber = "LevelNumber";
    // public const string Vibration = "Vibration";
    // public const string NoAds = "NoAds";
    // public static int noAds;
    public static int levelNumber;
    // public static int vibration = 1;
    public bool deleteData = false, deleteDataVibration = false, deleteDataAds = false;

    void Awake()
    {
        levelNumber = PlayerPrefs.GetInt("LevelNumber");
    }

    // void Start()
    // {
    //     vibration = PlayerPrefs.GetInt("Vibration");
    //     noAds = PlayerPrefs.GetInt("NoAds");
    // }

    void Update()
    {
        if (deleteData)
        {
            DeleteData();
        }

        // if (deleteDataVibration)
        // {
        //     DeleteDataVibration();
        // }

        // if (deleteDataAds)
        // {
        //     DeleteDataAds();
        // }
    }

    public static void UpdateLevelNumber()
    {
        PlayerPrefs.SetInt ("LevelNumber", levelNumber);
        levelNumber = PlayerPrefs.GetInt("LevelNumber");
        PlayerPrefs.Save();
    }

    // public static void UpdateVibration()
    // {
    //     PlayerPrefs.SetInt ("Vibration", vibration);
    //     vibration = PlayerPrefs.GetInt("Vibration");
    //     PlayerPrefs.Save();
    // }

    // public static void UpdateAds()
    // {
    //     PlayerPrefs.SetInt ("NoAds", noAds);
    //     noAds = PlayerPrefs.GetInt("NoAds");
    //     PlayerPrefs.Save();
    // }

    public void DeleteData()
    {
        levelNumber = 1;
        PlayerPrefs.SetInt("LevelNumber", levelNumber);
        UpdateLevelNumber();
    }

    // public void DeleteDataVibration()
    // {
    //     vibration = 1;
    //     PlayerPrefs.SetInt("Vibration", vibration);
    //     UpdateVibration();
    // }

    // public void DeleteDataAds()
    // {
    //     noAds = 0;
    //     PlayerPrefs.SetInt("NoAds", noAds);
    // }
}
