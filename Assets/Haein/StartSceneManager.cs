using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneManager : MonoBehaviour
{
    public GameObject challengePanel;
    
    public void StartBtnOnClick()
    {
        challengePanel.SetActive(true);
    }
    
    public void ChallengeBtnOnClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
