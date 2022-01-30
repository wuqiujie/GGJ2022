using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("End_Lose");
        }
    }

    public void Win()
    {
        StartCoroutine(WinDelay());
    }

    public IEnumerator WinDelay()
    {
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("End_Win");
    }
}
