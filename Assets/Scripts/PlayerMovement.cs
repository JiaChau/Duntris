using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float acceleration = 10f; // Controls how fast we reach max speed
    public float deceleration = 10f; // Controls how quickly we stop
    private Rigidbody rb;
    private Vector3 movement;
    private Vector3 targetVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; // Smoother movement
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent unwanted rotation
    }

    void Update()
    {
        // Get smooth WASD / Arrow key input
        float moveX = Input.GetAxis("Horizontal"); // A/D or Left/Right (Smooth)
        float moveZ = Input.GetAxis("Vertical");   // W/S or Up/Down (Smooth)

        // Convert to isometric movement
        Vector3 inputDirection = new Vector3(moveX + moveZ, 0, moveZ - moveX).normalized;

        // Apply acceleration for smoother movement transitions
        if (inputDirection.magnitude > 0.1f)
        {
            targetVelocity = inputDirection * moveSpeed;
        }
        else
        {
            targetVelocity = Vector3.zero; // Smoothly stop movement when no input is detected
        }
    }

    void FixedUpdate()
    {
        // Smooth movement transition using velocity
        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        // Rotate the player towards the movement direction
        if (targetVelocity != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, targetVelocity.normalized, deceleration * Time.fixedDeltaTime);
        }

        // Lock Y position to ground level (use whatever Y height is appropriate, like 0)
        Vector3 pos = rb.position;
        pos.y = 1.2f;
        rb.position = pos;

    }
}
