using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Rigidbody2D bicyclerRigidBody;

    public WheelJoint2D rearWheel;

    public AudioSource windSound;
    public AudioSource gearChangeSound;
    public AudioSource bicycleChainLowSpeedSound;
    public AudioSource bicycleChainFullSpeedSound;
    public AudioSource rearTireHit;
    public AudioSource frontTireHit;

    public float minimumChainLowSpeedSoundDelay;
    public float maximumChainLowSpeedSoundDelay;
    public float chainSingleSoundMaximumSpeed;

    // Rear wheel joint speed absolute value
    float jointSpeedAbs;

    // Used to count the time before repetition
    float timer = 0f;

    // Sound pitch subtracted number
    public float substractedPitch;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (bicyclerRigidBody.velocity.magnitude > Constants.MINIMUM_WIND_VELOCITY)
        {
            windSound.volume = Mathf.Clamp(bicyclerRigidBody.velocity.magnitude / Constants.MAXIMUM_WIND_VELOCITY - 0.1f, 0f, 1f);
        }
        else
        {
            windSound.volume = 0f;
        }

        jointSpeedAbs = Mathf.Abs(rearWheel.jointSpeed);

        if (jointSpeedAbs > 10f)
        {
            //if (jointSpeedAbs < chainSingleSoundMaximumSpeed)
            //{
            //    if (bicycleChainFullSpeedSound.isPlaying)
            //    {
            //        bicycleChainFullSpeedSound.Stop();
            //    }

            //    if (timer > Mathf.Clamp(maximumChainLowSpeedSoundDelay * (1f - jointSpeedAbs / chainSingleSoundMaximumSpeed), minimumChainLowSpeedSoundDelay, 1f))
            //    {
            //        bicycleChainLowSpeedSound.pitch = 0.75f + Mathf.Clamp(jointSpeedAbs / chainSingleSoundMaximumSpeed, 0f, 0.75f);
            //        bicycleChainLowSpeedSound.Play();
            //        timer = 0f;
            //    }
            //    else
            //    {
            //        timer += Time.fixedDeltaTime;
            //    }
            //}
            //else
            //{
            //    if (!bicycleChainFullSpeedSound.isPlaying)
            //    {
            //        bicycleChainFullSpeedSound.Play();
            //    }
            //}
        }
    }

    public void SetSlowMotionPitch()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.pitch -= substractedPitch;
        }
    }
}
