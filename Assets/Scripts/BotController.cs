using UnityEngine;

public class BotController : MonoBehaviour
{
    // @formatter:off
    [SerializeField] private float thrust;
    [SerializeField] private float minTurnSpeed;
    [SerializeField] private float maxTurnSpeed;
    [SerializeField] private float turnInterval;
    [SerializeField] private float turnTime;
    // @formatter:on

    private Rigidbody _rb;
    private float _timer;
    private bool _turning;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;

        // always apply thrust
        _rb.AddForce(thrust * transform.up);

        if (_turning && _timer >= turnTime)
        {
            _timer = 0f;
            _turning = false;
            _rb.angularVelocity = Vector3.zero;
        }
        else if (!_turning && _timer >= turnInterval)
        {
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