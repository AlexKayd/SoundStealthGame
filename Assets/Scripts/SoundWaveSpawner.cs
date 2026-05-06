using UnityEngine;
using UnityEngine.VFX;

public class SoundWaveSpawner : MonoBehaviour
{
    [Header("Префаб волны")]
    public GameObject wavePrefab;

    [Header("Звук шага")]
    public AudioClip footstepClip;

    [Header("Настройки волны")]
    public float walkRadius = 3f;
    public float runRadius = 6f; 
    public float emitInterval = 0.5f;
    public float runEmitInterval = 0.3f; // интервал между шагами при беге

    [Header("Слой зомби")]
    public LayerMask zombieLayer = -1;

    private PlayerController playerController;
    private float nextEmitTime;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        if (playerController == null)
            Debug.LogError("SoundWaveSpawner: Не найден PlayerController на игроке");
    }

    void Update()
    {
        // движется ли игрок и прошло ли уже 0.5 секунды с прошлой волны
        if (IsMoving() && Time.time >= nextEmitTime)
        {
            EmitWave(); // создаём волну

            // какой интервал использовать
            bool running = Input.GetKey(KeyCode.LeftShift);
            float currentInterval = running ? runEmitInterval : emitInterval;
            nextEmitTime = Time.time + currentInterval;
        }
    }

    bool IsMoving()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        return Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f;
    }

    void EmitWave()
    {
        // определяем радиус
        bool running = Input.GetKey(KeyCode.LeftShift);
        float radius = running ? runRadius : walkRadius;

        // берём питч и громкость из PlayerController
        float pitch = playerController.CurrentPitch;
        float volume = playerController.CurrentVolume;

        // место появления волны 
        Vector3 spawnPos = transform.position;
        spawnPos.y += 0.2f;

        GameObject wave = Instantiate(wavePrefab, spawnPos, Quaternion.identity);

        RingController ring = wave.GetComponent<RingController>();
        if (ring != null)
        {
            ring.Init(radius);
        }

        // проигрываем звук шага
        AudioSource audio = wave.GetComponent<AudioSource>();
        if (audio != null && footstepClip != null)
        {
            audio.pitch = pitch;
            audio.volume = volume;
            audio.PlayOneShot(footstepClip);
        }

        // ищем всех зомби вокруг
        Collider[] hits = Physics.OverlapSphere(spawnPos, radius, zombieLayer);
        foreach (Collider hit in hits)
        {
            ZombieAI zombie = hit.GetComponent<ZombieAI>();
            if (zombie != null)
            {
                // говорим зомби бежать к месту
                zombie.OnSoundHeard(spawnPos);
            }
        }

        Destroy(wave, 1f);
    }
}