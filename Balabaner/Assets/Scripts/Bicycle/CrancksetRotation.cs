using UnityEngine;

public class CrancksetRotation : MonoBehaviour
{
    Rigidbody2D crancksetRb2D;
    public BicycleController bikeController;
    public Rigidbody2D cranckset2Rb2D;
    public WheelJoint2D rearWheel;

    public float rotationSpeed;
    public float maximumBackRotationSpeed;

    // Use this for initialization
    void Start()
    {
        crancksetRb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rearWheel != null)
        {
            if (rearWheel.jointSpeed < 0)
            {
                if (rearWheel.useMotor && bikeController.gearsSpeed[bikeController.currentGear] != 0f)
                {
                    crancksetRb2D.angularVelocity = Mathf.Clamp(rearWheel.jointSpeed / -bikeController.gearsSpeed[bikeController.currentGear], 0f, 1f) * rotationSpeed;
                }
                else
                {
                    crancksetRb2D.angularVelocity = 0f;
                }
            }
            else
            {
                crancksetRb2D.angularVelocity = Mathf.Clamp(Mathf.Clamp(rearWheel.jointSpeed / -bikeController.gearsSpeed[bikeController.currentGear], -1f, 0f) * rotationSpeed, 0f, maximumBackRotationSpeed);
            }
            if (Mathf.Abs(cranckset2Rb2D.rotation - crancksetRb2D.rotation) > 2f)
            {
                cranckset2Rb2D.angularVelocity = 0f;
                cranckset2Rb2D.MoveRotation(crancksetRb2D.transform.eulerAngles.z - 180f);
            }
        }
    }
}
