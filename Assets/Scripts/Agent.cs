using System.Collections;
using TMPro;
using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField] float _speed = 1f;
    [SerializeField] Color _defaultColor;
    [SerializeField] Color _triggerColor;

    [HideInInspector] public int LifePoints { get { return _lifePoints; } }

    Vector2 _target;
    Material _agentMaterial;
    bool _targetSet = false;
    int _lifePoints = 3;

    void Start()
    {
        name = GameManager.Instance.GetRandomName();
        _agentMaterial = GetComponent<Renderer>().material;
    }

    void Update()
    {
        if (!_targetSet) {
            SetTarget();
            StartCoroutine(MoveToTarget());
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag(GameManager.Instance.AgentTag))
        {
            _lifePoints--;
            _agentMaterial.color = _triggerColor;

            if (_lifePoints <= 0)
                Destroy(gameObject);
        }
    }

    void OnTriggerExit(Collider collider)
    {
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
        
        while (Vector3.Distance(transform.position, targetPosition) > .1)
        {
            transform.position += direction * Time.fixedDeltaTime * _speed;
            yield return new WaitForFixedUpdate();
        }
        _targetSet = false;
        yield return null;
    }
}
