using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pursuer : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;
    [SerializeField] private float _stopDistance;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
        Look();
    }

    private void Move()
    {
        Vector3 direction = (_target.position - transform.position);
        float distance = direction.magnitude;

        if (distance > _stopDistance)
        {
            direction = direction.normalized * _speed;
        }
        else
        {
            direction = Vector3.zero;
        }

        direction.y = _rigidbody.velocity.y + Physics.gravity.y * Time.deltaTime;
        _rigidbody.velocity = direction;
    }

    private void Look()
    {
        transform.LookAt(_target.position);
    }
}
