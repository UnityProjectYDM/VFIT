using UnityEngine;

public class LevitatingText : MonoBehaviour
{
    public float levitationHeight = 0.05f;        // Adjust this value to set the height of levitation
    public float levitationSpeed = 2.0f;         // Adjust this value to set the speed of levitation

    private Vector3 initialPosition;

    void Start()
    {
        // Store the initial position of the sign
        initialPosition = transform.position;
    }

    void Update()
    {
        // Calculate the vertical offset using a sine wave
        float yOffset = Mathf.Sin(Time.time * levitationSpeed) * levitationHeight;

        // Update the sign's position with the levitation motion
        transform.position = new Vector3(initialPosition.x, initialPosition.y + yOffset, initialPosition.z);
    }
}
