using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UIInkStorage : MonoBehaviour
{
    [SerializeField] private Image bar;

    [SerializeField] private Image foreseenBar;
    [SerializeField] private Image uiHarden;
    private Animator animator;
    private bool squidIdle = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInkStorageChanged(object sender, InkStorageChangedEventArgs e)
    {
        bar.fillAmount = e.inkStorage;
    }

    public void OnForeseenInkStorageChanged(object sender, InkStorageChangedEventArgs e)
    {
        foreseenBar.fillAmount = e.inkStorage;
    }

    public void OnInkInsufficient(object sender, EventArgs e)
    {
        animator.Play("UIInkWarn");
    }

    public void OnTransform(object sender, TransformEventArgs e)
    {
        if (squidIdle)
        {
            uiHarden.enabled = true;
            bar.enabled = false;
            foreseenBar.enabled = false;
            GetComponent<Image>().enabled = false;
        }
        else
        {
            uiHarden.enabled = false;
            bar.enabled = true;
            foreseenBar.enabled = true;
            GetComponent<Image>().enabled = true;
        }

        squidIdle = !squidIdle;
    }
}

public class InkStorageChangedEventArgs : EventArgs
{
    public float inkStorage;
}