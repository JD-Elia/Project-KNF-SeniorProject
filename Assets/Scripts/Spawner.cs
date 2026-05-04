using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclePrefabs;
    [SerializeField] private Transform obstacleParent;
    public float obstacleSpawnTime = 2f;
    [Range(0,1)] public float obstacleSpawnTimeFactor = 0.1f;
    public float obstacleSpeed = 1f;
    [Range(0,1)] public float obstacleSpeedFactor = 0.2f;

    private float _obstacleSpawnTime;
    public float _obstacleSpeed;

    private float timeAlive;
    private float timeUntilObstacleSpawn;

    private void Start() {
        GameManager.Instance.onPlay.AddListener(ClearObstacles);
        GameManager.Instance.onPlay.AddListener(ResetFactors);
    }

    private void Update(){
        if (GameManager.Instance.isPlaying) {
            timeAlive += Time.deltaTime;
            CalculateFactors();
            SpawnLoop();
        }
    }

    private void SpawnLoop(){
        timeUntilObstacleSpawn += Time.deltaTime;
        if (timeUntilObstacleSpawn >= _obstacleSpawnTime) {
            timeUntilObstacleSpawn = 0f;
            Spawn();
        }
    }

    private void ClearObstacles() {
        foreach (Transform child in obstacleParent) {
            Destroy(child.gameObject);
        }
    }
    
    private void CalculateFactors() {
        _obstacleSpawnTime = obstacleSpawnTime / Mathf.Pow(timeAlive, obstacleSpawnTimeFactor);
        _obstacleSpeed = obstacleSpeed * Mathf.Pow(timeAlive, obstacleSpeedFactor);
    }

    private void ResetFactors () {
        timeAlive = 1f;
        _obstacleSpawnTime = obstacleSpawnTime;
        _obstacleSpeed = obstacleSpeed;
    }
    
    private void Spawn() {
        RaycastHit2D hit = Physics2D.Raycast(
            new Vector2(transform.position.x, transform.position.y),
            Vector2.down,
            10f,
            LayerMask.GetMask("Ground")
        );

        // If no ground found, or ground is raised, skip spawn
        if (hit.collider == null) return;
        if (hit.point.y > -4f) return; // raised tile

        GameObject obstacleToSpawn = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
        GameObject spawnedObstacle = Instantiate(obstacleToSpawn, transform.position, Quaternion.identity);
        spawnedObstacle.transform.parent = obstacleParent.transform;

        Rigidbody2D obstacleRB = spawnedObstacle.GetComponent<Rigidbody2D>();
        obstacleRB.linearVelocity = Vector2.left * _obstacleSpeed;
    }
}