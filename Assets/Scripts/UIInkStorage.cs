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

    private Animator animator;

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
}

public class InkStorageChangedEventArgs : EventArgs
{
    public float inkStorage;
}