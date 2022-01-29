using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Change : MonoBehaviour
{

    [SerializeField] public float speed = 4f;
    [SerializeField] public float end_y= 19f;

    private Vector2 Startpostion;
    void Start()
    {
        Startpostion = transform.position;
        Debug.Log(Startpostion);
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
        if (transform.position.y > end_y)
        {
            transform.position = Startpostion;
        }
    }
}
