using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Pause Menu canvas
    public Canvas PauseMenu;

    // Both Bicycle and Bicycler gameobjects parent
    public GameObject bikeAndBikerParent;
    public GameObject bicycler;

    public CameraFollow mainCamera;

    // Joints that connects bicycler with bicycle which will be disabled on crash
    public Joint2D[] jointsToDisable;

    // Biker body rigidbodies which masses are changed on crash impact to simulate more realistic fall
    Rigidbody2D[] bicyclerRigidBodies;

    // Wheels rigidbodies
    public Rigidbody2D rearWheelRigidbody;
    public Rigidbody2D frontWheelRigidbody;

    public Rigidbody2D bicycleRigidBody;

    // The joint that is allowed to break for rear wheel
    public HingeJoint2D rearSuspensionToWheelJoint;

    // Wheel joint
    public WheelJoint2D rearWheel;
    public WheelJoint2D frontWheel;

    // Angle reacher components which force is set to zero on impact
    AngleReacher[] angleReachers;

    JointSuspension2D tempSuspension;

    // Delay time before performing automatic restart
    public float restartDelay;

    // Delay time before player can manually restart
    public float beforeRestartDelay;

    // Mass for all body sprites after crash
    public float bodyPartsMassesAfterImpact;

    // Wheel mass after crash impact
    public float wheelMassAfterImpact;

    // Bicycle mass after impact
    public float bicycleMassAfterImpact;

    float saveTimeScale;

    public bool isCrashing;

    public AudioManager audioManager;

    // Use this for initialization
    void Start()
    {
        angleReachers = bikeAndBikerParent.GetComponentsInChildren<AngleReacher>();
        bicyclerRigidBodies = bicycler.GetComponentsInChildren<Rigidbody2D>();
        ResetTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCrashing)
        {
            if (restartDelay <= 0f)
            {
                Restart();
            }
            else
            {
                if (beforeRestartDelay > 0f)
                {
                    beforeRestartDelay -= Time.deltaTime;
                }
                else
                {
                    if (Input.anyKeyDown && !Input.GetButtonDown("Cancel"))
                    {
                        restartDelay = 0f;
                    }
                    else
                    {
                        restartDelay -= Time.deltaTime;
                    }
                }
            }
        }

        // Pause Game
        if (Input.GetButtonDown("Cancel"))
        {
            if (PauseMenu.enabled)
            {
                Time.timeScale = saveTimeScale;
                PauseMenu.enabled = false;
            }
            else
            {
                saveTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                PauseMenu.enabled = true;
            }
        }
    }
    public void BeforeRestart()
    {
        // Return in case if player is already crashing
        if (isCrashing)
        {
            return;
        }

        isCrashing = true;

        // Turn on slow-mo
        Time.timeScale = Constants.SLOWMO_TIME_SCALE;
        Time.fixedDeltaTime = Constants.SLOWMO_FIXED_DELTA_TIME;

        // Speed up camera and change the position to focus on slow-mo
        mainCamera.distanceFollowSpeed = 25f;
        mainCamera.xOffset = -10f;
        mainCamera.yOffset = 0.0f;

        // Disable motor if wheel joint is enabled
        if (rearSuspensionToWheelJoint != null)
        {
            rearSuspensionToWheelJoint.breakForce = Constants.UNBREAKABLE_FORCE;
            rearWheel.useMotor = false;
        }

        // Turn off the controller
        bikeAndBikerParent.GetComponentInChildren<BicycleController>().enabled = false;

        // Disable joints that connects bicycler with bicycle
        for (int i = 0; i < jointsToDisable.Length; i++)
        {
            jointsToDisable[i].enabled = false;
        }

        // Disable Angle Reacher force for all objects
        for (int i = 0; i < angleReachers.Length; i++)
        {
            angleReachers[i].maximumForce = 0f;
        }

        // Change mass of the bicycler body parts
        for (int i = 0; i < bicyclerRigidBodies.Length; i++)
        {
            bicyclerRigidBodies[i].mass = bodyPartsMassesAfterImpact;
        }

        rearWheelRigidbody.mass = wheelMassAfterImpact;
        frontWheelRigidbody.mass = wheelMassAfterImpact;
        bicycleRigidBody.mass = bicycleMassAfterImpact;

        // Asign new suspension
        if (frontWheel != null)
        {
            tempSuspension = frontWheel.suspension;
            tempSuspension.frequency = Constants.SLOWMO_SUSPENSION_FREQUENCY;
            frontWheel.suspension = tempSuspension;
            frontWheel.breakForce = Constants.UNBREAKABLE_FORCE;
        }

        // Change pitch so slow motion sounds real
        audioManager.SetSlowMotionPitch();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetTime()
    {
        Time.timeScale = Constants.NORMAL_TIME_SCALE;
        Time.fixedDeltaTime = Constants.NORMAL_FIXED_DELTA_TIME;
    }
}
