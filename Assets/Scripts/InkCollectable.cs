using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkCollectable : MonoBehaviour
{
    public float inkAmount;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale *= inkAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
