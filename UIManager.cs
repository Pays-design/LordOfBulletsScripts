using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LordOfBullets.Core
{
public class UIManager : MonoBehaviour
{
    public GameObject _tapToDrawTip, _fingerTip;
    
    public TextMeshProUGUI _countOfEnemies;

    #region UI animators
    public Animator _nextLevelButton, _retryLevelButton, _CompletePanel, _victoryLevelText, _lossLevelText; 
    #endregion

    public static bool isShowCompleteLevelUI = false, isShowLossLevelUI = false;
    public static bool isPlayerBulletFade = false;

    #region BlockPanel
    Image _image_UIManager;
    Button _button_UIManager;
    #endregion

    #region Ammo
    public GunMagazine _gunMagazineScript;
    public RawImage _bullet5, _bullet4, _bullet3, _bullet2, _bullet1;
    #endregion

    private Player m_player;

    void Awake()
    {
        isShowCompleteLevelUI = false;
        isShowLossLevelUI = false;
        isPlayerBulletFade = false;
    }

    void Start()
    {
        _image_UIManager = GetComponent<Image>();
        _button_UIManager = GetComponent<Button>();
        
        #region Tips
        if (_tapToDrawTip != null)
        {
            _tapToDrawTip.SetActive(true);
        }
        
        if (_fingerTip != null)
        {
           _fingerTip.SetActive(false);  
        }
        #endregion

        LevelStateObserver.GetInstance().OnEnemyDeath += (countOfDeadEnemies) =>
        {
            _countOfEnemies.text = $"{countOfDeadEnemies}/{LevelStateObserver.GetInstance().CountOfEnemies}";
        };

        m_player = FindObjectOfType<Player>();

        m_player.OnShoot += TrackFiredBullet;
    }

    void Update()
    {
        if (_gunMagazineScript != null)
        {
            if (_gunMagazineScript.CountOfBullets == 5)
            {
                _bullet5.enabled = true;
                _bullet4.enabled = true;
                _bullet3.enabled = true;
                _bullet2.enabled = true;
                _bullet1.enabled = true;
            }

            if (_gunMagazineScript.CountOfBullets == 4)
            {
                _bullet5.enabled = false;
                _bullet4.enabled = true;
                _bullet3.enabled = true;
                _bullet2.enabled = true;
                _bullet1.enabled = true;
            }

            if (_gunMagazineScript.CountOfBullets == 3)
            {
                _bullet5.enabled = false;
                _bullet4.enabled = false;
                _bullet3.enabled = true;
                _bullet2.enabled = true;
                _bullet1.enabled = true;
            }

            if (_gunMagazineScript.CountOfBullets == 2)
            {
                _bullet5.enabled = false;
                _bullet4.enabled = false;
                _bullet3.enabled = false;
                _bullet2.enabled = true;
                _bullet1.enabled = true;
            }

            if (_gunMagazineScript.CountOfBullets == 1)
            {
                _bullet5.enabled = false;
                _bullet4.enabled = false;
                _bullet3.enabled = false;
                _bullet2.enabled = false;
                _bullet1.enabled = true;
            }

            if (_gunMagazineScript.CountOfBullets <= 0)
            {
                _bullet5.enabled = false;
                _bullet4.enabled = false;
                _bullet3.enabled = false;
                _bullet2.enabled = false;
                _bullet1.enabled = false;
            }
        }

        if (isShowCompleteLevelUI == true && isPlayerBulletFade == true)
        {
            isShowLossLevelUI = false;

            _CompletePanel.SetBool("CompletePanel", true);
            _victoryLevelText.SetBool("VictoryTextAppear", true);
            _nextLevelButton.SetBool("ButtonAppear", true);
        }

        if (isShowLossLevelUI == true && isPlayerBulletFade == true)
        {
            isShowCompleteLevelUI = false;

            _CompletePanel.SetBool("CompletePanel", true);
            _lossLevelText.SetBool("LossTextAppear", true);
            _retryLevelButton.SetBool("ButtonAppear", true);
        }
    }

    public void InitialBlock()
    {
        if (_tapToDrawTip != null)
        {
            _tapToDrawTip.SetActive(false);
        }
        
        if (_fingerTip != null)
        {
            _fingerTip.SetActive(true);   
        }
        
        // this.gameObject.SetActive(false);

        if (_image_UIManager != null)
        {
            _image_UIManager.enabled = false;
        }

        if (_button_UIManager != null)
        {
            _button_UIManager.enabled = false;
        }
    }

    public void NextLevel()
    {
        PlayerPrefsManager.levelNumber++;
        PlayerPrefsManager.UpdateLevelNumber();
        SceneManager.LoadScene(PlayerPrefsManager.levelNumber); // load next level
    }

    public void Replay()
    {
        SceneManager.LoadScene(PlayerPrefsManager.levelNumber);
    }

    private void TrackFiredBullet(float bulletSpeed, Bullet firedBullet) 
    {
        firedBullet.OnFade += TryToShowUI;
    }

    private void TryToShowUI()
    {
        if (isShowCompleteLevelUI == true || isShowLossLevelUI == true)
        {
            isPlayerBulletFade = true;
        }
    }
}
}
