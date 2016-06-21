using UnityEngine;
using System.Collections;

public class AngleReacher : MonoBehaviour
{
    [Range(0, 359)]
    public float angleToReach;

    // Maximum speed used to reach the desired angle
    public float speed;

    // Maximum torque force apllied to joint to reach the maximum speed
    public float maximumForce;

    public HingeJoint2D hingeJoint2D;

    JointMotor2D jointMotor2D;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        jointMotor2D = hingeJoint2D.motor;
        jointMotor2D.motorSpeed = speed * getAngleRatio();
        jointMotor2D.maxMotorTorque = maximumForce;
        hingeJoint2D.motor = jointMotor2D;
    }

    float getAngleRatio()
    {
        if (hingeJoint2D.jointAngle > angleToReach)
        {
            if (hingeJoint2D.jointAngle - angleToReach <= angleToReach + 360f - hingeJoint2D.jointAngle)
            {
                return -1f + (hingeJoint2D.jointAngle - angleToReach) / 180f;
            }
            else
            {
                return 1f - (angleToReach + 360f - hingeJoint2D.jointAngle) / 180f;
            }
        }
        else
        {
            if (angleToReach - hingeJoint2D.jointAngle <= hingeJoint2D.jointAngle + 360f - angleToReach)
            {
                return -1f + (angleToReach - hingeJoint2D.jointAngle) / 180f;
            }
            else
            {
                return 1f - (hingeJoint2D.jointAngle + 360f - angleToReach) / 180f;
            }
        }
    }
}
