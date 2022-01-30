using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [SerializeField] public int hp;

    [SerializeField] public int inkDropNumber;

    [SerializeField] public float inkAmountMin, inkAmountMax;
    [SerializeField] public AudioClip hitSound;
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
