using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PanelAlpha gameWinPanel;
    [SerializeField] private Text _levelText;

    private void OnEnable()
    {
        GameManager.GameWin += GameManager_GameWin;
        GameManager.NextLevel += GameManager_NextLevel;
    }

    private void GameManager_NextLevel()
    {
        gameWinPanel.PanelClose();
        _levelText.text = "Level"+" "+(GameManager.Instance.currentLevelIndex+1).ToString();
    }

    private void GameManager_GameWin()
    {
        gameWinPanel.PanelOpen();
    }

    private void OnDisable()
    {
        GameManager.GameWin -= GameManager_GameWin;
        GameManager.NextLevel -= GameManager_NextLevel;
    }
    

    public void NextLevel()
    {
        GameManager.Instance.OnNextLevel();
    }

    private void Start()
    {
        _levelText.text ="Level"+" "+ (GameManager.Instance.currentLevelIndex+1).ToString();
    }
}
