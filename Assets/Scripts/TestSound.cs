using UnityEngine;
using UnityEngine.VFX;

public class TestSound : MonoBehaviour
{
    public GameObject wavePrefab;
    public float testRadius = 10f; // Увеличили радиус
    public LayerMask zombieLayer = -1;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Vector3 pos = transform.position;
            pos.y += 0.2f;

            GameObject wave = Instantiate(wavePrefab, pos, Quaternion.identity);
            VisualEffect vfx = wave.GetComponent<VisualEffect>();
            if (vfx != null)
            {
                vfx.SetFloat("Radius", testRadius);
                vfx.Play(); // Принудительный запуск (иногда требуется)
                Debug.Log("VFX запущен с радиусом " + testRadius);
            }
            else
            {
                Debug.LogWarning("VFX не найден!");
            }

            Collider[] hits = Physics.OverlapSphere(pos, testRadius, zombieLayer);
            Debug.Log("Найдено коллайдеров: " + hits.Length);
            foreach (Collider hit in hits)
            {
                ZombieAI zombie = hit.GetComponent<ZombieAI>();
                if (zombie != null)
                {
                    zombie.OnSoundHeard(pos);
                    Debug.Log("Зомби услышал: " + zombie.name);
                }
            }

            Destroy(wave, 2f); // Дадим волне пожить подольше
        }
    }
}