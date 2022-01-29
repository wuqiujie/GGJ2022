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
        while (time <= fadeOutTime)
        {
            color.a = Mathf.Lerp(1, fadeOutDest, time / fadeOutTime);
            spriteRenderer.color = color;
            transform.localScale = originalScale * Mathf.Lerp(1, scaleUpRate, time / fadeOutTime);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(GetComponent<Rigidbody2D>());
        Destroy(GetComponent<Collider2D>());

    }
}
