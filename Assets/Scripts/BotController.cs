using UnityEngine;

public class BotController : MonoBehaviour
{
    // @formatter:off
    [SerializeField] private float thrust;
    [SerializeField] private float minTurnSpeed;
    [SerializeField] private float maxTurnSpeed;
    [SerializeField] private float turnInterval;
    [SerializeField] private float turnTime;
    [SerializeField] private float minLifetime;
    [SerializeField] private float maxLifetime;
    // @formatter:on

    private Rigidbody _rb;
    private float _timer;
    private bool _turning;
    private float _lifeTimer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        _lifeTimer = Random.Range(minLifetime, maxLifetime);
    }

    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
        _lifeTimer -= Time.fixedDeltaTime;

        if (_lifeTimer <= 0) Destroy(gameObject);

        // always apply thrust
        _rb.AddForce(thrust * transform.up);

        if (_turning && _timer >= turnTime)
        {
            // time up, stop turning
            _timer = 0f;
            _turning = false;
            _rb.angularVelocity = Vector3.zero;
        }
        else if (!_turning && _timer >= turnInterval)
        {
            // turn interval elapsed, start turning again
            _timer = 0f;
            _turning = true;
            // pick new random turn velocity
            float turnAngle = Random.Range(minTurnSpeed, maxTurnSpeed) *
                              (Random.value >= 0.5 ? 1 : -1)
                              * Mathf.Deg2Rad;
            _rb.angularVelocity = new Vector3(0f, 0f, turnAngle);
        }
    }
}