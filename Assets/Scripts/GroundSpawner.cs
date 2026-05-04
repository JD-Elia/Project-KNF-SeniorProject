using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] private GameObject groundTilePrefab;
    [SerializeField] private GameObject raisedTilePrefab;
    [SerializeField] private Transform groundParent;
    [SerializeField] private Spawner spawner;

    [Header("Tile Settings")]
    public float tileWidth = 5f;
    public float groundY = -4.5f;

    [Header("Gap Settings")]
    public float minGapSize = 1f;
    public float maxGapSize = 3f;
    public float gapChance = 0.3f;

    [Header("Height Settings")]
    public float raisedHeight = 0.5f;
    public float raiseChance = 0.2f;

    [Header("Spawn Settings")]
    public float spawnX = 12f;
    public float despawnX = -12f;
    public int safeTiles = 3;

    // Spawner reads this to know if it's safe to spawn an enemy
    public bool lastTileWasLower = true;

    private float _nextSpawnX;
    private int _safeTileCount = 0;

    private float MoveSpeed => spawner != null ? spawner._obstacleSpeed : 5f;

    private void Start() {
        GameManager.Instance.onPlay.AddListener(OnPlay);

        _nextSpawnX = -10f;
        while (_nextSpawnX < spawnX) {
            SpawnTile(_nextSpawnX);
            _nextSpawnX += tileWidth;
        }
    }

    private void OnPlay() {
        _safeTileCount = 0;
        lastTileWasLower = true;
        foreach (Transform child in groundParent) {
            Destroy(child.gameObject);
        }
        _nextSpawnX = -10f;
        while (_nextSpawnX < spawnX) {
            SpawnTile(_nextSpawnX);
            _nextSpawnX += tileWidth;
        }
    }

    private void Update() {
        if (!GameManager.Instance.isPlaying) return;

        foreach (Transform tile in groundParent) {
            tile.position += Vector3.left * MoveSpeed * Time.deltaTime;

            if (tile.position.x < despawnX) {
                Destroy(tile.gameObject);
            }
        }

        if (_nextSpawnX < spawnX) {
            if (Random.value < gapChance) {
                float gapSize = Random.Range(minGapSize, maxGapSize);
                _nextSpawnX += gapSize;
                lastTileWasLower = false; // gap coming, not safe
            }

            SpawnTile(_nextSpawnX);
            _nextSpawnX += tileWidth;
        }

        _nextSpawnX -= MoveSpeed * Time.deltaTime;
    }

    private void SpawnTile(float xPos) {
        bool isRaised = _safeTileCount >= safeTiles && Random.value < raiseChance;

        float yPos = isRaised ? groundY + raisedHeight : groundY;

        // Update flag so Spawner knows if last tile was lower ground
        lastTileWasLower = !isRaised;

        GameObject prefabToUse = (isRaised && raisedTilePrefab != null) ? raisedTilePrefab : groundTilePrefab;

        GameObject tile = Instantiate(prefabToUse,
            new Vector3(xPos, yPos, 0f),
            Quaternion.identity,
            groundParent);

        _safeTileCount++;
    }
}