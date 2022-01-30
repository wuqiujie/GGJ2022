using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject tutorialMenu;

   public void playGame()
       
    {
        SceneManager.LoadScene("Main");
    }

    public void quitGame()
    {
        Application.Quit();
    }


    public void backGame()
    {
        SceneManager.LoadScene("Start");
         mainMenu.SetActive(true);
        tutorialMenu.SetActive(false);

    }
}
