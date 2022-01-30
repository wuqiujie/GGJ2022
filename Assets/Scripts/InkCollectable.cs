using System.Collections;
using UnityEngine;

public class InkCollectable : MonoBehaviour
{
    public float inkAmount;

    public bool collectable = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale *= inkAmount;
        StartCoroutine(setActive());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator setActive()
    {
        yield return new WaitForSeconds(1f);
        collectable = true;
    }
}
