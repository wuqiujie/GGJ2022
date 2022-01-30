using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endMenu : MonoBehaviour
{
    public void restartGame()
    {
        SceneManager.LoadScene("Main");

    }

    public void quitGame()
    {
        Application.Quit();
    }


}
