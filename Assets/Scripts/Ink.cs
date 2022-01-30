using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ink : MonoBehaviour
{
    [SerializeField] private float scaleUpRate;

    [SerializeField] private float fadeOutDest;

    [SerializeField] private float fadeOutTime;
    private Vector3 originalScale;
    
    void Awake()
    {
        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        originalScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        var time = 0f;
        var spriteRenderer = GetComponent<SpriteRenderer>();      
        var color = spriteRenderer.color;
        
        Destroy(GetComponent<Rigidbody2D>(), 1);
        Destroy(GetComponent<Collider2D>(), 1);
        while (time <= fadeOutTime)
        {
            // color.a = Mathf.Lerp(1, fadeOutDest, time / fadeOutTime);
            color.a = (1 - fadeOutDest) / (time + 1) + fadeOutDest;
            spriteRenderer.color = color;
            // transform.localScale = originalScale * Mathf.Lerp(1, scaleUpRate, time / fadeOutTime);
            transform.localScale = originalScale * ((1 - scaleUpRate) / (time + 1) + scaleUpRate);
            time += Time.deltaTime;
            yield return null;
        }


    }
}
