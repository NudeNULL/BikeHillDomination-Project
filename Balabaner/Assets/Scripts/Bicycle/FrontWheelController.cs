using UnityEngine;

public class FrontWheelController : MonoBehaviour
{
    public LevelManager levelManager;

    public string groundTag;

    Rigidbody2D rigidBody2D;

    public BicycleController bicycleController;
    public ParticleSystem tireParticle;
    public AudioManager audioManager;

    // The minimum tire velocity on hit to play the "hit" audio source
    public float tireMinimumHitVelocity;

    // Particle System parameters
    public float maximumTireEmissionRate;

    // Temporary variables
    ParticleSystem.EmissionModule emission;
    ParticleSystem.MinMaxCurve rate;

    // After wheel joint crash new material and mass for better physics
    public PhysicsMaterial2D afterCrashWeelMaterial;
    public float afterCrashMass;

    // Joints that are connected to fork, disabled on front wheel crash
    public HingeJoint2D forkHingeJoint;
    public SliderJoint2D forkSliderJoint;
    public DistanceJoint2D forkDistanceJoint;

    // Wheel joint
    public WheelJoint2D frontWheel;

    // Use this for initialization
    void Start()
    {
        emission = tireParticle.emission;
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (frontWheel == null)
        {
            forkHingeJoint.enabled = false;
            forkDistanceJoint.enabled = false;
            forkSliderJoint.enabled = false;

            gameObject.GetComponent<Rigidbody2D>().mass = afterCrashMass;
            gameObject.GetComponent<CircleCollider2D>().sharedMaterial = afterCrashWeelMaterial;

            levelManager.BeforeRestart();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == groundTag)
        {
            if (col.relativeVelocity.magnitude > tireMinimumHitVelocity)
            {
                audioManager.frontTireHit.Play();
            }

            bicycleController.frontWheelGrounded = true;
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == groundTag)
        {
            rate = emission.rate;
            rate.constantMin = Mathf.Abs(rigidBody2D.angularVelocity) / bicycleController.gearsSpeed[bicycleController.gearsSpeed.Length - 1] * maximumTireEmissionRate;

            rate.constantMax = rate.constantMin;
            emission.rate = rate;
            tireParticle.transform.position = col.contacts[0].point;
        }

        if (tireParticle.isStopped)
        {
            tireParticle.Play();
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == groundTag)
        {
            if (tireParticle.isPlaying)
            {
                tireParticle.Stop();
            }

            bicycleController.frontWheelGrounded = false;
        }
    }
}
