using UnityEngine;

public class RearWheelController : MonoBehaviour
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
    public float TireAngularPerLinearVelocityRate;

    // After wheel joint crash new material and mass for better physics
    public PhysicsMaterial2D afterCrashWeelMaterial;
    public float afterCrashMass;

    // temporary variables
    ParticleSystem.EmissionModule emission;
    ParticleSystem.MinMaxCurve rate;

    // The joint that is allowed to break for rear wheel
    public HingeJoint2D rearSuspensionToWheelJoint;

    // Wheel joint
    public WheelJoint2D rearWheel;

    // Use this for initialization
    void Start()
    {
        emission = tireParticle.emission;
        rigidBody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (rearSuspensionToWheelJoint == null)
        {
            rearWheel.enabled = false;
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
                audioManager.rearTireHit.Play();
            }

            bicycleController.rearWheelGrounded = true;
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.tag == groundTag)
        {
            rate = emission.rate;
            rate.constantMin = Mathf.Abs(rigidBody2D.angularVelocity) / bicycleController.gearsSpeed[bicycleController.gearsSpeed.Length - 1] * maximumTireEmissionRate;

            //Add to emission rate the coeficient that defines the rate between angular velocity and linear velocity of wheel
            if (Mathf.Abs(rigidBody2D.angularVelocity) / (rigidBody2D.velocity.magnitude * TireAngularPerLinearVelocityRate) < 0.01f)
            {
                rate.constantMin = maximumTireEmissionRate;
            }

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

            bicycleController.rearWheelGrounded = false;
        }
    }
}
