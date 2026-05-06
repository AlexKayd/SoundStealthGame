using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Движение")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;

    [Header("Детектор пола")]
    public LayerMask groundMask = -1;
    public float groundCheckDistance = 5f;
    public float heightAboveGround = 1.0f;

    public string CurrentSurfaceTag { get; private set; }
    public float CurrentPitch { get; private set; }
    public float CurrentVolume { get; private set; }

    private float currentSpeed;
    private Animator animator;

    void Start()
    {
        // начальные значения
        CurrentSurfaceTag = "Wood";
        CurrentPitch = 1.0f;
        CurrentVolume = 0.5f;

        animator = GetComponentInChildren<Animator>();

        SnapToGround();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0, v);

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && move.magnitude > 0.1f;
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        if (animator != null)
            animator.SetFloat("Speed", move.magnitude * currentSpeed);
        else
            Debug.LogWarning("Animator не найден");

        transform.Translate(move.normalized * currentSpeed * Time.deltaTime, Space.World);

        SnapToGround();
    }

    void SnapToGround()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.1f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance, groundMask))
        {
            string tag = hit.collider.tag;
            if (tag == "Wood" || tag == "Earth" || tag == "Metal")
            {
                CurrentSurfaceTag = tag;
                switch (tag)
                {
                    case "Wood": CurrentPitch = 1.0f; CurrentVolume = 0.8f; break;
                    case "Earth": CurrentPitch = 0.7f; CurrentVolume = 0.5f; break;
                    case "Metal": CurrentPitch = 1.3f; CurrentVolume = 1.0f; break;
                }

                Vector3 pos = transform.position;
                pos.y = hit.point.y + heightAboveGround;
                transform.position = pos;
            }
        }
    }
}