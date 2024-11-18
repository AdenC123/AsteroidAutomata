using UnityEngine;

public class BotController : MonoBehaviour
{
    // @formatter:off
    [SerializeField] private float thrust;
    [SerializeField] private float maxTurnForce;
    [SerializeField] private float minTurnForce;
    [SerializeField] private float turnTime;
    // @formatter:on

    private Rigidbody _rb;
    private float _turnTimer;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _turnTimer += Time.fixedDeltaTime;
        
        // always apply thrust
        _rb.AddForce(thrust * transform.up);

        if (_turnTimer >= turnTime)
        {
            _turnTimer = 0f;
            // apply a random torque
            float turnForce = Random.Range(minTurnForce, maxTurnForce);
            _rb.AddTorque(turnForce * transform.forward);
        }
    }
}
