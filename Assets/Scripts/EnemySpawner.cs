using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Header("Префаб зомби")]
    public GameObject zombiePrefab;

    [Header("Настройки карты")]
    public MapGenerator mapGenerator;
    public int safeRows = 2;

    [Header("Параметры сложности")]
    public float minSpawnChance = 0.05f;
    public float maxSpawnChance = 0.3f;
    public float exponent = 3.0f; // чем выше тем медленнее рост в начале и больше в конце
    public bool preventAdjacent = true;

    [Header("Другие параметры")]
    public float spawnHeight = 1.5f;
    public float checkRadius = 0.4f;

    void Start()
    {
        SpawnZombies();
    }

    void SpawnZombies()
    {
        int width = mapGenerator.gridWidth;
        int height = mapGenerator.gridHeight;
        float spacing = mapGenerator.tileSpacing;

        bool[,] occupied = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int z = safeRows; z < height - 1; z++)
            {
                if (preventAdjacent)
                {
                    if (IsAdjacentOccupied(occupied, x, z, width, height))
                        continue;
                }

                float t = (float)(z - safeRows) / (height - 2 - safeRows);
                float chance = Mathf.Lerp(minSpawnChance, maxSpawnChance, Mathf.Pow(t, exponent));

                if (Random.value < chance)
                {
                    Vector3 pos = new Vector3(
                        x * spacing - (width * spacing) / 2f,
                        0f,
                        z * spacing - (height * spacing) / 2f
                    );
                    pos.y += spawnHeight;

                    GameObject zombie = Instantiate(zombiePrefab, pos, Quaternion.identity);
                    zombie.name = "Zombie_" + x + "_" + z;

                    occupied[x, z] = true;
                }
            }
        }
    }

    bool IsAdjacentOccupied(bool[,] occupied, int x, int z, int width, int height)
    {
        if (x > 0 && occupied[x - 1, z]) return true;
        if (x < width - 1 && occupied[x + 1, z]) return true;
        if (z > 0 && occupied[x, z - 1]) return true;
        if (z < height - 1 && occupied[x, z + 1]) return true;

        return false;
    }
}