using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    AudioSource audioSource;

    public ParticleSystem mainParticleSystem;

    public string playerTag;

    public float minimumVelocity;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == playerTag)
        {
            if (col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude > minimumVelocity)
            {
                audioSource.Play();

                mainParticleSystem.Play();
            }
        }
    }
}
