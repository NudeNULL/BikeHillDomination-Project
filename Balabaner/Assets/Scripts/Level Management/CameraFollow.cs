using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject followedObject;

    public float distanceFollowSpeed;
    public float followSpeed;
    public float maxCameraDistance;
    public float minCameraDistance;
    public float xOffset;
    public float yOffset;
    public float zOffset;

    float cameraDistance;

    Rigidbody2D bodyRigidbody;

    // Use this for initialization
    void Start()
    {
        bodyRigidbody = followedObject.GetComponent<Rigidbody2D>();
        transform.position = new Vector3(followedObject.transform.position.x + bodyRigidbody.velocity.magnitude / xOffset, followedObject.transform.position.y + yOffset, Mathf.Lerp(transform.position.z, zOffset - bodyRigidbody.velocity.magnitude, Time.fixedDeltaTime * distanceFollowSpeed));
    }

    void FixedUpdate()
    {
        cameraDistance = Mathf.Lerp(transform.position.z, zOffset - bodyRigidbody.velocity.magnitude, Time.fixedDeltaTime * distanceFollowSpeed);

        transform.position = Vector3.Lerp
        (
            transform.position,
            new Vector3
            (
                followedObject.transform.position.x + (transform.position.z + zOffset) / xOffset,
                followedObject.transform.position.y + yOffset,
                Mathf.Clamp(cameraDistance, minCameraDistance, maxCameraDistance)
            ),
            followSpeed * Time.fixedDeltaTime
        );
    }
}
