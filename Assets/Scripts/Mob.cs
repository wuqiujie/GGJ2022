using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mob : MonoBehaviour
{
    [SerializeField] public int hp;

    [SerializeField] public int inkDropNumber;

    [SerializeField] public float inkAmountMin, inkAmountMax;
    [SerializeField] public AudioClip hitSound;
    [SerializeField] public UnityEvent onMobDies;
    public Animator animator;

    void Awake()
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

}
