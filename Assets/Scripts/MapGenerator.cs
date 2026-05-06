using UnityEngine;
using Unity.AI.Navigation;

public class MapGenerator : MonoBehaviour
{
    [Header("Сетка")]
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float tileSpacing = 1.1f;

    [Header("Префаб плитки")]
    public GameObject tilePrefab;

    [Header("Материал")]
    public Material tileMaterial;

    [Header("Игрок")]
    public Transform playerTransform;

    private string[] surfaceTags = { "Earth", "Wood", "Metal" };

    void Start()
    {
        GenerateMap();
    }

    void GenerateMap()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridHeight; z++)
            {
                Vector3 pos = new Vector3(
                    x * tileSpacing - (gridWidth * tileSpacing) / 2f,
                    0f,
                    z * tileSpacing - (gridHeight * tileSpacing) / 2f
                );

                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity);
                tile.isStatic = true;

                int typeIndex = Random.Range(0, surfaceTags.Length);
                string tag = surfaceTags[typeIndex];
                tile.tag = tag;

                Renderer rend = tile.GetComponent<Renderer>();
                rend.material = tileMaterial;

                MaterialPropertyBlock props = new MaterialPropertyBlock();
                props.SetFloat("_SurfaceType", typeIndex);
                rend.SetPropertyBlock(props);

                tile.name = tag + "_" + x + "_" + z;
            }
        }

        // запекаем навигацию
        NavMeshSurface surface = FindObjectOfType<NavMeshSurface>();
        if (surface != null)
            surface.BuildNavMesh();
        else
            Debug.LogError("NavMeshSurface не найден");

        // установка игрока в центр первого ряда
        if (playerTransform != null)
        {
            int centerX = gridWidth / 2;
            float startX = centerX * tileSpacing - (gridWidth * tileSpacing) / 2f;
            float startZ = 0 * tileSpacing - (gridHeight * tileSpacing) / 2f;
            Vector3 startPos = new Vector3(startX, 1.5f, startZ);
            playerTransform.position = startPos;
            PlayerController pc = playerTransform.GetComponent<PlayerController>();
        }
    }
}