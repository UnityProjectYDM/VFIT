using UnityEngine;

public class LevitatingSign : MonoBehaviour
{
    public float levitationHeight = 0.05f;        // Adjust this value to set the height of levitation
    public float levitationSpeed = 2.0f;         // Adjust this value to set the speed of levitation
    public float maxRotationAngle = 30.0f;       // Maximum rotation angle in degrees
    public float rotationChangeInterval = 5.0f;  // Time interval to change rotation
    public float rotationSmoothing = 2.0f;       // Adjust this value to set the smoothness of rotation

    private Vector3 initialPosition;
    private Quaternion targetRotation;
    private float rotationTimer = 0.0f;
    void Start()
    {
        // Store the initial position of the sign
        initialPosition = transform.position;

        // Set the initial target rotation
        targetRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }
    void Update()
    {
        // Calculate the vertical offset using a sine wave
        float yOffset = Mathf.Sin(Time.time * levitationSpeed) * levitationHeight;

        // Update the sign's position with the levitation motion
        transform.position = new Vector3(initialPosition.x, initialPosition.y + yOffset, initialPosition.z);

        // Change rotation at specified intervals
        rotationTimer += Time.deltaTime;
        if (rotationTimer >= rotationChangeInterval)
        {
            // Randomly choose a rotation angle within the specified range
            float randomRotation = Random.Range(-maxRotationAngle, maxRotationAngle);

            // Set the target rotation
            targetRotation = Quaternion.Euler(0.0f, randomRotation, 0.0f);

            // Reset the timer
            rotationTimer = 0.0f;
        }
        // Smoothly interpolate between the current rotation and the target rotation
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, rotationSmoothing * Time.deltaTime);
    }
}



