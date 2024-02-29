using JetBrains.Annotations;
using System.Collections.Generic;
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

    public Camera MainCamera;

    private void Start()
    {
        for (int i = 0; i < _initialNumAgents; i++)
            SpawnAgent();
    }

    void Update()
    {
        if (_numAgents < _maxNumAgents)
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f)
            {
                SpawnAgent();
                _spawnTimer = Random.Range(_minSpawnFrequency, _maxSpawnFrequency);
            }
        }

        HandleAgentClick();
    }

    void OnValidate()
    {
        UpdatePlaneSize();
    }

    #region Plane
    [SerializeField] Transform _plane;
    [SerializeField] Vector2Int _planeSize = new Vector2Int(10, 10);

    [HideInInspector]
    public Vector2Int PlaneHorizontalBounds { get { return _planeHorizontalBounds; } }
    [HideInInspector]
    public Vector2Int PlaneVerticalBounds { get { return _planeVerticalBounds; } }

    Vector2Int _planeSizeRange = new Vector2Int(10, 30);
    Vector2Int _planeHorizontalBounds;
    Vector2Int _planeVerticalBounds;

    void UpdatePlaneSize()
    {
        _planeSize.x = Mathf.Clamp(_planeSize.x, _planeSizeRange.x, _planeSizeRange.y);
        _planeSize.y = Mathf.Clamp(_planeSize.y, _planeSizeRange.x, _planeSizeRange.y);

        _plane.localScale = new Vector3(_planeSize.x, _plane.localScale.y, _planeSize.y);

        // plane bounds
        _planeHorizontalBounds = new Vector2Int(
            (int)_plane.position.x - _planeSize.x / 2 + 1,
            (int)_plane.position.x + _planeSize.x / 2);

        _planeVerticalBounds = new Vector2Int(
            (int)_plane.position.x - _planeSize.y / 2 + 1,
            (int)_plane.position.x + _planeSize.y / 2);
    }

    #endregion

    #region Agent
    public readonly List<string> Names = new List<string>
    {
        "Bonifacy", "Grażyna", "Bazyli", "Helga", "Zygmunt", 
        "Fryderyka", "Walenty", "Zdzisława", "Kazimierz", "Wiesława", 
        "Bogumił", "Dzidka", "Zyta", "Bolesław", "Ruda", "Feliks", 
        "Eustachy", "Ludmiła", "Hipolit", "Natasza", "Benedykt", 
        "Zdzisław", "Walentyna", "Mieczysław", "Bogusława", "Anatol",
        "Jadzia", "Bogdan", "Wanda"
    };
    public string AgentTag = "Agent";
    public Popup InfoPopup;

    [SerializeField] GameObject _agentPrefab;
    [SerializeField, Range(1, 30)] int _maxNumAgents = 30;
    [SerializeField, Range(1, 5)] int _initialNumAgents = 1;

    int _numAgents = 0;
    const int _minSpawnFrequency = 0;
    const int _maxSpawnFrequency = 1;
    float _spawnTimer;
    Agent _currentAgentInfo;

    public string GetRandomName()
    {
        return Names[Random.Range(0, Names.Count)];
    }

    void HandleAgentClick()
    {
        if (_currentAgentInfo.LifePoints != InfoPopup.GetSliderValue())
            InfoPopup.SetHealth(_currentAgentInfo.LifePoints);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)
                && hit.transform.CompareTag(AgentTag))
            {
                var agent = hit.transform.gameObject
                    .GetComponent<Agent>();

                InfoPopup.SetHealth(agent.LifePoints);
                InfoPopup.SetName(agent.name);
                InfoPopup.gameObject.SetActive(true);
            }
            else
            {
                InfoPopup.gameObject.SetActive(false);
            }
        }
    }

    void SpawnAgent()
    {
        var spawnPoint = new Vector3(
            Random.Range(_planeHorizontalBounds.x, _planeHorizontalBounds.y),
            0f,
            Random.Range(_planeVerticalBounds.x, _planeVerticalBounds.y));

        Instantiate(_agentPrefab, spawnPoint, Quaternion.identity);
        _numAgents++;
    }

    #endregion
}
