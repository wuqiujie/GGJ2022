using System;
using UnityEngine;


public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private AudioSource squidAudioSource;
    [SerializeField] private AudioSource fallAudioSource;
    [SerializeField] private AudioClip holdSound, transformSound, fallStartSound, bgmClip;
    [SerializeField] private AudioClip[] shootSounds, inkInsufficientSounds, absorbSounds;
    private void Awake()
    {
        Instance = this;
        //squidAudioSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnShoot(object sender, EventArgs e)
    {
        squidAudioSource.PlayOneShot(shootSounds[UnityEngine.Random.Range(0, shootSounds.Length)]);
    }

    public void OnHold(object sender, EventArgs e)
    {
        squidAudioSource.PlayOneShot(holdSound);

    }

    public void OnAbsorbInk(object sender, EventArgs e)
    {
        squidAudioSource.PlayOneShot(absorbSounds[UnityEngine.Random.Range(0, absorbSounds.Length)]);
    }
    
    public void OnTransform(object sender, EventArgs e)
    {
        squidAudioSource.Stop();
        squidAudioSource.PlayOneShot(transformSound);
    }
    
    public void OnFallStarts(object sender, EventArgs e)
    {
        fallAudioSource.PlayOneShot(fallStartSound);
    }
    
    public void OnFallEnds(object sender, EventArgs e)
    {
        fallAudioSource.Stop();
    }

    public void OnMobHit(object mob, EventArgs e)
    {
        var mmob = (Mob) mob;
        squidAudioSource.PlayOneShot(mmob.hitSound);
    }

    public void OnChrisHit(object sender, EventArgs e)
    {
        
    }

    public void OnInkInsufficient(object sender, EventArgs e)
    {
        squidAudioSource.PlayOneShot(inkInsufficientSounds[UnityEngine.Random.Range(0, inkInsufficientSounds.Length)]);
    }

    public void OnWin(object sender, EventArgs e)
    {
        
    }

    public void OnLose(object sender, EventArgs e)
    {
        
    }
}
