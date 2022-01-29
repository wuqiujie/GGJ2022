using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidVacuum : MonoBehaviour
{
    [SerializeField] private float vacuumConstant;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.GetComponent<InkCollectable>())
        {
            var dir3 = transform.position - collider.transform.position;
            var dir = new Vector2(dir3.x, dir3.y);
            collider.GetComponent<Rigidbody2D>().velocity += vacuumConstant * dir.normalized / dir.magnitude / dir.magnitude;
        }
    }
}
