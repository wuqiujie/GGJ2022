using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CartoonImgChange : MonoBehaviour
{
    public GameObject[] Images;
    public GameObject next;
    public GameObject start;
    int j = 0;

    public void changeImg()
    {
        for (int i = 0; i < 4; i++) { Images[i].SetActive(false); }


        if (j < 4) { j++; }
        if(j==3)
        {
       
            next.SetActive(false);
            start.SetActive(true);
           
        }
        Images[j].SetActive(true);
    }
    public void startGame()
    {
        SceneManager.LoadScene("Main");
    }
}
