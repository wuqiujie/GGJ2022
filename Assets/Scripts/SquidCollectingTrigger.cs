using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidCollectingTrigger : MonoBehaviour
{
    public event EventHandler<CollectInkEventArgs> collectInkEvent;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<InkCollectable>())
        {
            collectInkEvent?.Invoke(this, new  CollectInkEventArgs
            {
                inkAmount = other.gameObject.GetComponent<InkCollectable>().inkAmount
            });
            Destroy(other.gameObject);
        }
    }
}

public class CollectInkEventArgs : EventArgs
{
    public float inkAmount;
}