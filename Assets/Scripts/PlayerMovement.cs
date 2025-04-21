using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float acceleration = 10f;
    public float deceleration = 10f;

    private Rigidbody rb;
    private Vector3 targetVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Lock rotation + Y position (so player can't fall or be pushed vertically)
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
    }

    void Update()
    {
        if (TetrisGameManager.IsPlayingTetris || GameUIManager.IsUIOpen) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 inputDirection = new Vector3(moveX + moveZ, 0, moveZ - moveX).normalized;

        if (inputDirection.magnitude > 0.1f)
        {
            targetVelocity = inputDirection * moveSpeed;
        }
        else
        {
            targetVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        if (TetrisGameManager.IsPlayingTetris || GameUIManager.IsUIOpen) return;

        rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        if (targetVelocity != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward, targetVelocity.normalized, deceleration * Time.fixedDeltaTime);
        }
    }

}
