using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Движение")]
    public float walkSpeed = 4f;
    public float runSpeed = 8f;

    [Header("Детектор пола")]
    public float groundCheckDistance = 100f;

    public string CurrentSurfaceTag { get; private set; }
    public float CurrentPitch { get; private set; }
    public float CurrentVolume { get; private set; }

    private float currentSpeed;

    void Start()
    {
        // начальные значения
        CurrentSurfaceTag = "Wood";
        CurrentPitch = 1.0f;
        CurrentVolume = 0.5f;

        Vector3 pos = transform.position;
        pos.y = 1.5f;
        transform.position = pos;
    }

    void Update()
    {
        // движение
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontal, 0, vertical);

        if (horizontal != 0 || vertical != 0)
            Debug.Log("Двигаюсь: " + horizontal + ", " + vertical);

        bool isRunning = Input.GetKey(KeyCode.LeftShift) && move.magnitude > 0.1f;
        currentSpeed = isRunning ? runSpeed : walkSpeed;

        transform.Translate(move.normalized * currentSpeed * Time.deltaTime, Space.World);

        Vector3 pos = transform.position;
        pos.y = 1.5f;
        transform.position = pos;

        DetectSurface();
    }

    void DetectSurface()
    {
        // луч из центра капсулы вниз
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, groundCheckDistance))
        {
            string tag = hit.collider.tag;
            if (tag == "Wood" || tag == "Earth" || tag == "Metal")
            {
                CurrentSurfaceTag = tag;
                switch (tag)
                {
                    case "Wood":
                        CurrentPitch = 1.0f;
                        CurrentVolume = 0.8f;
                        break;
                    case "Earth":
                        CurrentPitch = 0.7f;
                        CurrentVolume = 0.5f;
                        break;
                    case "Metal":
                        CurrentPitch = 1.3f;
                        CurrentVolume = 1.0f;
                        break;
                }
            }
        }
        else
        {

            CurrentSurfaceTag = "Wood";
            CurrentPitch = 1.0f;
            CurrentVolume = 0.5f;
        }
    }
}