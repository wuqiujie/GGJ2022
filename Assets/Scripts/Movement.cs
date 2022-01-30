using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Random = UnityEngine.Random;


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
    [SerializeField] private GameObject inkCollectable;
    [SerializeField] private float inkExplodeSpeedConstant;
    private bool transformCooling = false;
    [SerializeField] private GameObject explosionParticle; 
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
        if (state != SquidState.HardIdle)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.up = new Vector3(mousePos.x - transform.position.x, mousePos.y - transform.position.y, 0);
        }
        
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

        if (Input.GetKeyDown(KeyCode.F) && !transformCooling && (state == SquidState.Idle || state == SquidState.HardIdle))
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
        ink.transform.localScale *= Mathf.Lerp(1, 2, holdTime / holdMaxTime);
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
        transformCooling = true;
        if (state == SquidState.Idle)
        {
            rigidbody2D.sharedMaterial = HardenPhysicsMat;
            GetComponent<Collider2D>().sharedMaterial = HardenPhysicsMat;
            rigidbody2D.drag = 0;
            rigidbody2D.gravityScale = 10;
            state = SquidState.HardIdle;
            // yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("SquidHardIdle"));
        } else if (state == SquidState.HardIdle)
        {
            rigidbody2D.sharedMaterial = IdlePhysicsMat;
            GetComponent<Collider2D>().sharedMaterial = IdlePhysicsMat;
            rigidbody2D.drag = 1;
            rigidbody2D.gravityScale = 0.1f;
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"));
            state = SquidState.Idle;
            transformCooling = false;
        }

        yield return null;
    }

    private void OnInkCollected(object sender, CollectInkEventArgs e)
    {
        InkStorage += e.inkAmount;
        // TODO: play animation
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Env");
        {
            transformCooling = false;
        }
        if (LayerMask.LayerToName(other.gameObject.layer) == "Mob" && state == SquidState.HardIdle)
        {
            transformCooling = false;
            var mob = other.gameObject.GetComponent<Mob>();
            mob.hp -= 1;
            if (mob.hp == 0)
            {
                Destroy(mob.gameObject);
                // TODO: particles, spread inks
                for (var i = 0; i < 10; i++)
                {
                    var randDirTheta = Random.Range(0, Mathf.PI);
                    var randDir = new Vector2(Mathf.Cos(randDirTheta), Mathf.Sin(randDirTheta));
                    var randDir3 = new Vector3(randDir.x, randDir.y, 0);
                    var inkObject = Instantiate(inkCollectable, mob.transform.position + randDir3 * 0.5f, new Quaternion());
                    var inkAmount = Random.Range(0.05f, 0.2f);
                    inkObject.GetComponent<InkCollectable>().inkAmount = inkAmount;
                    inkObject.GetComponent<Rigidbody2D>().velocity = randDir * inkExplodeSpeedConstant * inkAmount;

                }

                var obj = Instantiate(explosionParticle, mob.transform.position, new Quaternion());
                Destroy(obj, 1f);
            }
        }
    }
}

enum SquidState
{
    Idle,
    Hold,
    Shoot,
    HardIdle
}
