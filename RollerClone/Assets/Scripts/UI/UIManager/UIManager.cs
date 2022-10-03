using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PanelAlpha gameWinPanel;

    private void OnEnable()
    {
        GameManager.GameWin += GameManager_GameWin;
        GameManager.NextLevel += GameManager_NextLevel;
    }

    private void GameManager_NextLevel()
    {
        gameWinPanel.PanelClose();
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
}
