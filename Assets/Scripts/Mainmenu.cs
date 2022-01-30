using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
   public void playGame()
       
    {

        Debug.Log("!!");
        SceneManager.LoadScene("Main");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
