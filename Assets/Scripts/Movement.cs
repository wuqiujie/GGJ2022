using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private GameObject inkPrefab;

    [SerializeField] private float inkSpeedConstant;

    [SerializeField] private float speedConstant;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            var ink = Instantiate(inkPrefab, transform.position - transform.up, transform.rotation);
            var dir = new Vector2(transform.up.x, transform.up.y);
            ink.GetComponent<Rigidbody2D>().velocity -= dir * inkSpeedConstant;
            
            GetComponent<Rigidbody2D>().velocity += dir * speedConstant;
        }
    }
}
