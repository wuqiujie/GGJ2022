using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    [SerializeField] private GameObject inkPrefab;

    [SerializeField] private float inkSpeedMin, inkSpeedMax;
    [SerializeField] private float inkConsumptionMin, inkConsumptionMax;
    [SerializeField] private float speedMin, speedMax;
    [SerializeField] private float holdPowerRate;
    [SerializeField] private float holdMaxTime;
    [SerializeField] private PhysicsMaterial2D IdlePhysicsMat, HardenPhysicsMat;
    [SerializeField] private UIInkStorage uiInkStorage;
    private float InkStorage
    {
        get => inkStorage;
        set
        {
            inkStorage = Mathf.Clamp(value, 0, 1);
            ForeseenInkStorage = inkStorage;
            inkStorageChangedEvent?.Invoke(this, new InkStorageChangedEventArgs
            {
                inkStorage = inkStorage
            });
        }
    }
    private float inkStorage = 1;
    
    private float ForeseenInkStorage
    {
        get => foreseenInkStorage;
        set
        {
            foreseenInkStorage = value;
            foreseenInkStorageChangedEvent?.Invoke(this, new InkStorageChangedEventArgs
            {
                inkStorage = value
            });
        }
    }
    private float foreseenInkStorage = 1;
    
    private event EventHandler<InkStorageChangedEventArgs> inkStorageChangedEvent, foreseenInkStorageChangedEvent;
    private event EventHandler inkInsufficientEvent;
    private float holdTime = 0;
    private SquidState state = SquidState.Idle;
    private Coroutine holdRoutine;
    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private Collider2D collider2D;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (uiInkStorage)
        {
            inkStorageChangedEvent += uiInkStorage.OnInkStorageChanged;
            foreseenInkStorageChangedEvent += uiInkStorage.OnForeseenInkStorageChanged;
            inkInsufficientEvent += uiInkStorage.OnInkInsufficient;
            GetComponentInChildren<SquidCollectingTrigger>().collectInkEvent += OnInkCollected;
        }
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (state == SquidState.Idle)
            {
                if (InkStorage > inkConsumptionMin)
                {
                    holdRoutine = StartCoroutine(Hold());
                }
                else
                {
                    inkInsufficientEvent?.Invoke(this, EventArgs.Empty);
                }
            }
            
            // var ink = Instantiate(inkPrefab, transform.position - transform.up, transform.rotation);
            // var dir = new Vector2(transform.up.x, transform.up.y);
            // ink.GetComponent<Rigidbody2D>().velocity -= dir * inkSpeedConstant;
            //
            // GetComponent<Rigidbody2D>().velocity += dir * speedConstant;
            //
            // animator.SetTrigger("Shoot");
        }

        if (Input.GetKeyUp(KeyCode.Space) && state == SquidState.Hold)
        {
            StopCoroutine(holdRoutine);
            StartCoroutine(Shoot());
        }

        if (Input.GetKeyDown(KeyCode.F) && (state == SquidState.Idle || state == SquidState.HardIdle))
        {
            StartCoroutine(TransformHard());
        }
    }

    private IEnumerator Hold()
    {
        state = SquidState.Hold;
        animator.SetTrigger("Hold");
        holdTime = 0f;
        var holdMaxTime = this.holdMaxTime * Mathf.Clamp(InkStorage / inkConsumptionMax, 0, 1);
        while (holdTime < holdMaxTime)
        {
            holdTime += Time.deltaTime;
            ForeseenInkStorage = InkStorage - Mathf.Lerp(inkConsumptionMin, inkConsumptionMax, holdTime / this.holdMaxTime);
            yield return null;
        }


        StartCoroutine(Shoot());
    }
    
    private IEnumerator Shoot()
    {
        state = SquidState.Shoot;
        animator.SetTrigger("Shoot");
        yield return new WaitForSeconds(.25f);
        
        var ink = Instantiate(inkPrefab, transform.position - transform.up, transform.rotation);
        ink.transform.localScale = ink.transform.localScale * Mathf.Lerp(1, 2, holdTime / holdMaxTime);
        var dir = new Vector2(transform.up.x, transform.up.y);
        ink.GetComponent<Rigidbody2D>().velocity -= dir * Mathf.Lerp(inkSpeedMin, inkSpeedMax, holdTime / holdMaxTime);
        
        GetComponent<Rigidbody2D>().velocity += dir * Mathf.Lerp(speedMin, speedMax, holdTime / holdMaxTime);
        InkStorage -= Mathf.Lerp(inkConsumptionMin, inkConsumptionMax, holdTime / holdMaxTime);
        ForeseenInkStorage = InkStorage;
        state = SquidState.Idle;
        yield return null;
    }

    private IEnumerator TransformHard()
    {
        animator.SetTrigger("Transform");
        if (state == SquidState.Idle)
        {
            rigidbody2D.sharedMaterial = HardenPhysicsMat;
            collider2D.sharedMaterial = HardenPhysicsMat;
            rigidbody2D.drag = 0;
            rigidbody2D.gravityScale = 10;

            state = SquidState.HardIdle;
        } else if (state == SquidState.HardIdle)
        {
            rigidbody2D.sharedMaterial = IdlePhysicsMat;
            collider2D.sharedMaterial = IdlePhysicsMat;
            rigidbody2D.drag = 1;
            rigidbody2D.gravityScale = 0.1f;

            state = SquidState.Idle;

        }

        yield return null;
    }

    private void OnInkCollected(object sender, CollectInkEventArgs e)
    {
        InkStorage += e.inkAmount;
        // TODO: play animation
    }
}

enum SquidState
{
    Idle,
    Hold,
    Shoot,
    HardIdle
}
