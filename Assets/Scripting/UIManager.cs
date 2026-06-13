using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour

{
    public GameObject HelpPanel;


  // Start is called once before the first execution of Update after the MonoBehaviour is created
  public void GameStartButtonAction()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenHelpPanenl()
    {
        HelpPanel.SetActive(true);
    }

    public void CloseHelpPanel()
    {
        HelpPanel.SetActive(false);
    }
}
