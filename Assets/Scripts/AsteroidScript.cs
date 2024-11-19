using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    // @formatter:off
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private Material infectedMaterial;
    [SerializeField] private bool infected;
    [SerializeField] private float initialMass;
    [SerializeField] private int initialBots;  // only for initial asteroid
    [SerializeField] private float botSpawnRadius;
    [SerializeField] private float botConversionRate;
    // @formatter:on

    private const float OutOfBoundsCheckDelay = 0.5f;

    private CameraView _camView;
    private MeshRenderer _meshRenderer;
    private Rigidbody _rb;
    private float _outOfBoundsTimer;
    private float _botSpawnTimer;
    private float _mass;
    private int _bots;
    private float _initialScale;

    private void Awake()
    {
        _camView = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraView>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _rb = GetComponent<Rigidbody>();
        _mass = initialMass;
        _initialScale = transform.localScale.x;
        if (infected)
        {
            _bots = initialBots;
            _meshRenderer.material = infectedMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Bot entered");
        // when colliding with a bot, enter it into asteroid
        if (other.CompareTag("Bot"))
        {
            if (!infected)
            {
                infected = true;
                _meshRenderer.material = infectedMaterial;
            }

            _bots += 1;
            Destroy(other.gameObject);
        }
    }

    private void FixedUpdate()
    {
        CheckOutOfBounds();
        if (infected) UpdateInfection();
    }

    private void UpdateInfection()
    {
        // convert mass into bots (based on number of bots)
        float newMass = _mass - _bots * botConversionRate * Time.fixedDeltaTime;
        _bots += Mathf.FloorToInt(_mass) - Mathf.FloorToInt(newMass);
        _mass = newMass;
        Debug.Log($"mass: {_mass}, bots: {_bots}");
        
        // TODO: shoot off bots while shrinking

        // if out of mass, fire all bots and delete
        if (_mass <= 0f)
        {
            for (int i = 0; i < _bots; i++) TrySpawnBot();
            Destroy(gameObject);
        }

        // update asteroid size based on mass
        float newScale = _mass / initialMass * _initialScale;
        transform.localScale = new Vector3(newScale, newScale, newScale);
    }

    private void TrySpawnBot()
    {
        Debug.Log($"Trying to spawn bot {_bots}");
        // try to remove a bot
        if (_bots <= 0) return;
        _bots--;
        
        // spawn bot at edge of sphere
        float scale = Mathf.Abs(transform.localScale.x);
        float spawnRadius = scale + botSpawnRadius;
        Vector2 randomCircle = Random.insideUnitCircle;
        Vector2 relSpawnPoint = randomCircle * spawnRadius;
        Vector3 spawnPoint = transform.position + new Vector3(relSpawnPoint.x, relSpawnPoint.y, 0f);
        GameObject newBot = Instantiate(botPrefab, spawnPoint, Quaternion.identity);
        float angle = Mathf.Atan2(randomCircle.y, randomCircle.x) * Mathf.Rad2Deg;
        newBot.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        newBot.GetComponent<Rigidbody>().velocity = _rb.velocity;
    }

    private void CheckOutOfBounds()
    {
        _outOfBoundsTimer += Time.fixedDeltaTime;
        if (_outOfBoundsTimer > OutOfBoundsCheckDelay)
        {
            // if asteroid is out of frame after initial time, flip it then constrain it
            float x = transform.position.x;
            float y = transform.position.y;
            float offset = transform.localScale.x;
            if (y > _camView.GetTop() + offset || y < _camView.GetBot() - offset)
            {
                FlipY();
                _outOfBoundsTimer = 0f;
            }

            if (x > _camView.GetRight() + offset || x < _camView.GetLeft() - offset)
            {
                FlipX();
                _outOfBoundsTimer = 0f;
            }
        }
    }

    /// <summary>
    /// Flips the asteroid's x position about the camera, constraining it to the edge of the camera.
    /// </summary>
    private void FlipX()
    {
        float offset = transform.localScale.x;
        float newX = (_camView.GetX() * 2) - transform.position.x;
        newX = Mathf.Clamp(newX, _camView.GetLeft() - offset, _camView.GetRight() + offset);
        transform.position = new Vector3(newX, transform.position.y);
    }

    /// <summary>
    /// Flips the asteroid's y position about the camera, constraining it to the edge of the camera.
    /// </summary>
    private void FlipY()
    {
        float offset = transform.localScale.x;
        float newY = (_camView.GetY() * 2) - transform.position.y;
        newY = Mathf.Clamp(newY, _camView.GetBot() - offset, _camView.GetTop() + offset);
        transform.position = new Vector3(transform.position.x, newY);
    }
}