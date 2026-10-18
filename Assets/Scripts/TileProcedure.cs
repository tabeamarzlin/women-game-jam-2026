using UnityEngine;

public class TileProcedure : MonoBehaviour
{
    [SerializeField] 
    private MeshFilter planeToTile;
    [SerializeField] 
    private GameObject tilePrefab;
    [SerializeField] 
    private float tileSize = 1f;

    [SerializeField] 
    private Transform tileParent;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        if (planeToTile == null || tilePrefab == null)
        {
            Debug.LogError("Assign the plane and tile prefab.");
            return;
        }

        Bounds bounds = planeToTile
            .GetComponent<Renderer>()
            .bounds;

        float width = bounds.size.x;
        float depth = bounds.size.z;

        int columns = Mathf.FloorToInt(width / tileSize);
        int rows = Mathf.FloorToInt(depth / tileSize);

        Vector3 startPosition = new Vector3(
            bounds.min.x + tileSize / 2f,
            bounds.max.y,
            bounds.min.z + tileSize / 2f
        );

        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                Vector3 position = startPosition +
                                   new Vector3(x * tileSize, 0f, z * tileSize);

                GameObject tile = Instantiate(
                    tilePrefab,
                    position,
                    Quaternion.Euler(270f, 0f, 0f)
                );
            }
        }
    }
}