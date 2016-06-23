using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class BicycleController : MonoBehaviour
{
    public WheelJoint2D rearWheelJoint;

    // The joint that connects bicycler body with bicycle
    public HingeJoint2D bicyclerHingeJoint;

    public Rigidbody2D rearWheelRigidBody;

    [Range(0, 2000)]
    public float motorForceOnBrake;

    // Linear force applied to bicycle to maintain better balance and avoid back flips on pedaling
    public float horizontalForce;
    public float xBackFlipAnchorDistance;
    public float yBackFlipAnchorDistance;
    public float handsAngleOnFrontFlip;
    public float forceToReachAngleOnFrontFlip;
    public float flipForce;
    public float flipAnchorSpeed;

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

    // Anchor position before doing any flips, used to reset the body postion to its origin
    Vector2 bicyclerHingeAnchorOrigin;

    // True if specific wheel is touching ground
    public bool rearWheelGrounded;
    public bool frontWheelGrounded;

    // Scripts that controls the hinge joints between arm and upper arm
    public AngleReacher leftArmAngleReacher;
    public AngleReacher rightArmAngleReacher;

    // Used to store the last value before changing it
    float saveAngle;
    float saveForce;

    // Current gear of bicycle
    public int currentGear;

    // Used to change value of motor every frame;
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
        bicyclerHingeAnchorOrigin = bicyclerHingeJoint.anchor;
        saveAngle = leftArmAngleReacher.angleToReach;
        saveForce = leftArmAngleReacher.maximumForce;

        // Configure motor force and motor speed;
        currentGear = 1;
        motorForce = gearsForce[currentGear];
        motorSpeed = gearsSpeed[currentGear];
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
        // Front flip
        if (horizontalAxis > 0)
        {
            leftArmAngleReacher.angleToReach = handsAngleOnFrontFlip;
            rightArmAngleReacher.angleToReach = handsAngleOnFrontFlip;

            leftArmAngleReacher.maximumForce = forceToReachAngleOnFrontFlip * Mathf.Clamp(horizontalAxis, 0f, 1f);
            rightArmAngleReacher.maximumForce = forceToReachAngleOnFrontFlip * Mathf.Clamp(horizontalAxis, 0f, 1f);
        }
        else
        {
            leftArmAngleReacher.angleToReach = saveAngle;
            rightArmAngleReacher.angleToReach = saveAngle;

            leftArmAngleReacher.maximumForce = saveForce;
            rightArmAngleReacher.maximumForce = saveForce;
        }

        // Back flip
        bicyclerHingeJoint.anchor = Vector2.Lerp
        (
            bicyclerHingeJoint.anchor,
            new Vector2
            (
                bicyclerHingeAnchorOrigin.x + xBackFlipAnchorDistance * Mathf.Clamp(horizontalAxis, -1f, 0f),
                bicyclerHingeAnchorOrigin.y + yBackFlipAnchorDistance * Mathf.Clamp(horizontalAxis, -1f, 0f)
            ),
            Time.deltaTime * flipAnchorSpeed
        );

        // Add angular velocity only if current velocity is not beyond limits
        if (!(rigidBody2D.angularVelocity < -maxAngularVelocity && horizontalAxis > 0.0f || rigidBody2D.angularVelocity > maxAngularVelocity && horizontalAxis < 0.0f))
        {
            rigidBody2D.angularVelocity -= flipForce * horizontalAxis;
        }
    }

    void ProcessVerticalAxis()
    {
        // Gas and brake processing
        if (verticalAxis > 0.0f)
        {
            // Add linear (horizontal) force to bicycle to avoid falling back too often
            if (rearWheelGrounded)
            {
                rigidBody2D.AddForce(new Vector2(verticalAxis * horizontalForce, 0));
            }

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
