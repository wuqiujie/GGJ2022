using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialImgChange : MonoBehaviour
{
    public GameObject[] Images;
    public GameObject next;
    int j = 0;

    public void changeImg()
    {
        for(int i = 0; i < 6; i++) { Images[i].SetActive(false); }

        
        if (j < 5) { j++; }
        else
        {
            j = 5;
            next.SetActive(false);
        }
        Images[j].SetActive(true);
    }

}
