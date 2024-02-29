using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    static GameManager _instance;

    public static GameManager Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        if (_instance)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);
    }
    #endregion

    [SerializeField] Transform _plane;
    [SerializeField] Vector2Int _planeSize = new Vector2Int(10, 10);

    Vector2Int _planeSizeRange = new Vector2Int(10, 30);

    private void Start()
    {

    }

    void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0f)
        {
            SpawnAgent();
            _spawnTimer = Random.Range(_minSpawnFrequency, _maxSpawnFrequency);
        }
    }

    void OnValidate()
    {
        _planeSize.x = Mathf.Clamp(_planeSize.x, _planeSizeRange.x, _planeSizeRange.y);
        _planeSize.y = Mathf.Clamp(_planeSize.y, _planeSizeRange.x, _planeSizeRange.y);

        _plane.localScale = new Vector3(_planeSize.x, _plane.localScale.y, _planeSize.y);
    }

    #region SpawnAgent
    [SerializeField] GameObject _agentPrefab;

    const int _minSpawnFrequency = 0;
    const int _maxSpawnFrequency = 1;
    float _spawnTimer;

    void SpawnAgent()
    {
        // random position on plane
        var horizontalBounds = new Vector2Int(
            (int)_plane.position.x - _planeSize.x / 2 + 1, 
            (int)_plane.position.x + _planeSize.x / 2);

        var verticalBounds = new Vector2Int(
            (int)_plane.position.x - _planeSize.y / 2 + 1,
            (int)_plane.position.x + _planeSize.y / 2);

        var spawnPoint = new Vector3(
            Random.Range(horizontalBounds.x, horizontalBounds.y),
            0f,
            Random.Range(verticalBounds.x, verticalBounds.y));

        var newAgent = Instantiate(_agentPrefab, spawnPoint, Quaternion.identity);
    }

    #endregion
}
