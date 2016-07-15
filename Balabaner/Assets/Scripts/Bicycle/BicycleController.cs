using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class BicycleController : MonoBehaviour
{
    public WheelJoint2D rearWheelJoint;

    public Rigidbody2D rearWheelRigidBody;

    // Spring joint used to pull back the bicycler
    public SpringJoint2D leanBackSpring;

    // Spring joint used to pull forward the bicycler
    public SpringJoint2D leanForwardSpring;

    [Range(0, 2000)]
    public float motorForceOnBrake;

    // Linear force applied to bicycle to maintain better balance and avoid back flips on pedaling
    public float horizontalForce;
    public float flipForce;
    public float flipAnchorSpeed;
    public float leanBackFrequency;
    public float leanForwardDistance;

    // Gear system
    public float[] gearsSpeed;
    public float[] gearsForce;
    public float gearChangeOffset;
    public AudioManager audioManager;

    // Maximum angular velocity on doing flips
    [Range(0, 359)]
    public float maxAngularVelocity;

    // Vertical and Horizontal axis values
    float verticalAxis;
    float horizontalAxis;

    float leanForwardDefaultDistance;
    float leanBackDefaultFrequency;

    // True if specific wheel is touching ground
    public bool rearWheelGrounded;
    public bool frontWheelGrounded;

    // Current gear of bicycle
    public int currentGear;

    // Used to change value of motor every frame
    JointMotor2D rearWheelJointMotor;

    // The gameobject own RigidBody2D
    Rigidbody2D rigidBody2D;

    // Current motor maximum speed and maximum applied force for current gear
    float motorSpeed, motorForce;

    // Use this for initialization
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        rearWheelJointMotor = rearWheelJoint.motor;

        // Configure motor force and motor speed;
        currentGear = 1;
        motorForce = gearsForce[currentGear];
        motorSpeed = gearsSpeed[currentGear];

        leanBackDefaultFrequency = leanBackSpring.frequency;
        leanForwardDefaultDistance = leanForwardSpring.distance;
    }

    // Update is called once per frame
    void Update()
    {
        if (rearWheelJoint != null)
        {
            // Get Axis
            verticalAxis = CrossPlatformInputManager.GetAxis("Vertical");
            horizontalAxis = CrossPlatformInputManager.GetAxis("Horizontal");

            // Perform all processing on bike
            ProcessHorizontalAxis();
            ProcessVerticalAxis();
            ProcessGearSystem();
        }
    }

    void ProcessHorizontalAxis()
    {
        // Add angular velocity only if current velocity is not beyond limits
        if (!(rigidBody2D.angularVelocity < -maxAngularVelocity && horizontalAxis > 0.0f || rigidBody2D.angularVelocity > maxAngularVelocity && horizontalAxis < 0.0f))
        {
            rigidBody2D.angularVelocity -= flipForce * horizontalAxis;
        }

        // Lean processing
        if (horizontalAxis > 0)
        {
            if (leanBackSpring.enabled)
            {
                leanBackSpring.frequency = leanBackDefaultFrequency;
                leanBackSpring.enabled = false;
            }

            leanForwardSpring.distance = leanForwardDefaultDistance + leanForwardDistance * horizontalAxis;
        }
        else if (horizontalAxis < 0)
        {
            leanBackSpring.enabled = true;
            leanBackSpring.frequency = leanBackDefaultFrequency + leanBackFrequency * -horizontalAxis;
            leanForwardSpring.distance = leanForwardDefaultDistance;
        }
        else
        {
            if (leanBackSpring.enabled)
            {
                leanBackSpring.frequency = leanBackDefaultFrequency;
                leanBackSpring.enabled = false;
            }

            leanForwardSpring.distance = leanForwardDefaultDistance;
        }
    }

    void ProcessVerticalAxis()
    {
        // Gas and brake processing
        if (verticalAxis > 0.0f && rearWheelGrounded)
        {
            // Add linear (horizontal) force to bicycle to avoid falling back too often
            rigidBody2D.AddForce(new Vector2(verticalAxis * horizontalForce, 0));

            rearWheelJointMotor.motorSpeed = -motorSpeed;
            rearWheelJointMotor.maxMotorTorque = motorForce * verticalAxis;
            rearWheelJoint.motor = rearWheelJointMotor;
            rearWheelJoint.useMotor = true;
        }
        else if (verticalAxis < 0.0f)
        {
            rearWheelJointMotor.motorSpeed = 0f;
            rearWheelJointMotor.maxMotorTorque = motorForceOnBrake * -verticalAxis;
            rearWheelJoint.motor = rearWheelJointMotor;
            rearWheelJoint.useMotor = true;
        }
        else
        {
            rearWheelJoint.useMotor = false;
        }
    }

    void ProcessGearSystem()
    {
        if (-rearWheelJoint.jointSpeed > 0f && rearWheelGrounded && (-rearWheelJoint.jointSpeed < (gearsSpeed[currentGear - 1] - gearChangeOffset) || -rearWheelJoint.jointSpeed > gearsSpeed[currentGear]))
        {
            if (-rearWheelJoint.jointSpeed < gearsSpeed[gearsSpeed.Length - 1])
            {
                for (int i = 0; i < gearsSpeed.Length - 1; i++)
                {
                    if (-rearWheelJoint.jointSpeed < gearsSpeed[i])
                    {
                        motorForce = gearsForce[i];
                        motorSpeed = gearsSpeed[i];
                        currentGear = i;

                        //if (!audioManager.gearChangeSound.isPlaying)
                        //{
                        //    audioManager.gearChangeSound.Play();
                        //}
                        break;
                    }
                }
            }
        }

    }
}
