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

    [SerializeField] Camera MainCamera;

    private void Start()
    {
        for (int i = 0; i < _initialNumAgents; i++)
            SpawnAgent();
    }

    void Update()
    {
        // spawn every "spawnTimer" seconds
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
        // range
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

        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        // change camera position to change the camera's visibility range
        // this equation will return 6 for plane size = 10, and 14 for plane size = 30
        MainCamera.orthographicSize = 2f / 5f * Mathf.Max((float)_planeSize.x, (float)_planeSize.y) + 2f;
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
    public readonly string AgentTag = "Agent";
    public Popup InfoPopup;

    [SerializeField] GameObject _agentPrefab;
    [SerializeField, Range(3, 30)] int _maxNumAgents = 30;
    [SerializeField, Range(3, 5)] int _initialNumAgents = 3;

    [HideInInspector] public int NumAgents
    {
        get { return _numAgents; }
        set
        {
            _numAgents = value < 0 ? 0 : value; // prevent setting less than 0
        }
    }

    int _numAgents = 0;
    const int _minSpawnFrequency = 1;
    const int _maxSpawnFrequency = 1;
    float _spawnTimer;
    Agent _currentAgentInfo;

    public string GetRandomName()
    {
        return Names[Random.Range(0, Names.Count)];
    }

    void HandleAgentClick()
    {
        if (_currentAgentInfo)
            InfoPopup.SetHealth(_currentAgentInfo.LifePoints);
        else
            InfoPopup.SetActive(false);
        
        if (Input.GetMouseButtonDown(0))
        {
            // user clicked UI
            bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            if (isOverUI) return;

            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
        
            // user clicked Agent
            if (Physics.Raycast(ray, out hit)
                && hit.transform.CompareTag(AgentTag))
            {
                var agent = hit.transform.gameObject
                    .GetComponent<Agent>();

                if(_currentAgentInfo) 
                    _currentAgentInfo.SetOutline(false);

                InfoPopup.SetHealth(agent.LifePoints);
                InfoPopup.SetName(agent.name);
                InfoPopup.SetActive(true);

                agent.SetOutline(true);

                _currentAgentInfo = agent;
            }
            // deselect agent, just click somewhere else
            else
            {
                if (_currentAgentInfo)
                    _currentAgentInfo.SetOutline(false);

                InfoPopup.SetActive(false);
                _currentAgentInfo = null;
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
