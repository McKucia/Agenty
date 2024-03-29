using System.Collections;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] float _speed = 3f;
    [SerializeField] Color _defaultColor;
    [SerializeField] Color _triggerColor;
    [SerializeField] Outline _outline;
    [SerializeField] Transform _model;

    [HideInInspector] public int LifePoints { get { return _lifePoints; } }

    Vector2 _target;
    Material _agentMaterial;
    int _lifePoints = 3;
    float _rotationSpeed = 10f;
    bool _targetSet = false;
    bool _isColliding = false;

    void Start()
    {
        name = GameManager.Instance.GetRandomName();
        _agentMaterial = _model
            .GetChild(0)
            .GetComponent<Renderer>().material;
    }

    public void SetOutline(bool active)
    {
        _outline.enabled = active;
    }

    void Update()
    {
        if (!_targetSet) {
            SetTarget();
            StartCoroutine(MoveToTarget());
        }

        UpdateColor();
    }

    // before OnTriggerStay
    void FixedUpdate()
    {
        _isColliding = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(GameManager.Instance.AgentTag))
        {
            _lifePoints--;

            if (_lifePoints <= 0)
            {
                Destroy(gameObject);
                GameManager.Instance.NumAgents--;
            }
        }
    }

    // change color if object is colliding
    void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.CompareTag(GameManager.Instance.AgentTag))
            _isColliding = true;
    }
    // note - cannot use onTriggerExit cause another object can be destroyed, in that case onTriggerExit would not work properly

    void UpdateColor()
    {
        // check if color is not already set, to optimize
        if (_isColliding && _agentMaterial.color != _triggerColor)
            _agentMaterial.color = _triggerColor;
        else if (!_isColliding && _agentMaterial.color != _defaultColor)
            _agentMaterial.color = _defaultColor;
    }

    void SetTarget()
    {
        _target = new Vector2(
            Random.Range(GameManager.Instance.PlaneHorizontalBounds.x, GameManager.Instance.PlaneHorizontalBounds.y),
            Random.Range(GameManager.Instance.PlaneVerticalBounds.x, GameManager.Instance.PlaneVerticalBounds.y));

        _targetSet = true;
    }

    IEnumerator MoveToTarget()
    {
        var targetPosition = new Vector3(_target.x, transform.position.y, _target.y);
        
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion targetRotation = Quaternion.identity;

        while (Vector3.Distance(transform.position, targetPosition) > .1)
        {
            transform.position += direction * Time.fixedDeltaTime * _speed;

            Vector3 targetDirection = targetPosition - _model.position;
            targetRotation = Quaternion.LookRotation(targetDirection);
            Quaternion nextRotation = Quaternion.Lerp(_model.localRotation, targetRotation, Time.fixedDeltaTime * _rotationSpeed);
            _model.localRotation = nextRotation;

            yield return new WaitForFixedUpdate();
        }
        _targetSet = false;
        yield return null;
    }
}
