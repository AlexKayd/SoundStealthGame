using UnityEngine;

public class RingController : MonoBehaviour
{
    [Header("Настройки кольца")]
    public float duration = 0.8f;
    public float startWidth = 0.4f;
    public Color startColor = new Color(1f, 0.92f, 0f, 1f);
    public Color endColor = new Color(1f, 0.92f, 0f, 0f);

    private LineRenderer lineRenderer;
    private float timer;
    private float maxRadius;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.loop = true;
            lineRenderer.material = new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));
            lineRenderer.widthMultiplier = startWidth;
        }

        lineRenderer.useWorldSpace = false;
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
        timer = 0f;
    }

    public void Init(float radius)
    {
        maxRadius = radius;
        timer = 0f;
        DrawCircle(0f);
    }

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / duration;
        if (t > 1f)
        {
            Destroy(gameObject);
            return;
        }

        float currentRadius = Mathf.Lerp(0, maxRadius, t);
        DrawCircle(currentRadius);

        if (lineRenderer != null)
        {
            lineRenderer.widthMultiplier = Mathf.Lerp(startWidth, 0f, t);
            Color col = Color.Lerp(startColor, endColor, t);
            lineRenderer.startColor = col;
            lineRenderer.endColor = col;
        }
    }

    void DrawCircle(float radius)
    {
        if (lineRenderer == null) return;

        int segments = 60;
        lineRenderer.positionCount = segments + 1;
        Vector3[] positions = new Vector3[lineRenderer.positionCount];
        float angleStep = 2 * Mathf.PI / segments;
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            positions[i] = new Vector3(x, 0f, z);
        }
        lineRenderer.SetPositions(positions);
    }
}