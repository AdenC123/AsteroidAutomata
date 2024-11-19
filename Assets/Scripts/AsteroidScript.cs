using UnityEngine;

public class AsteroidScript : MonoBehaviour
{
    // @formatter:off
    [SerializeField] private GameObject botPrefab;
    [SerializeField] private Material infectedMaterial;
    [SerializeField] private bool infected;
    [SerializeField] private int initialMass;
    [SerializeField] private int initialBots;  // only for initial asteroid
    [SerializeField] private float botSpawnTime;
    [SerializeField] private float botSpawnRadius;
    // @formatter:on

    private const float OutOfBoundsCheckDelay = 0.5f;

    private CameraView _camView;
    private MeshRenderer _meshRenderer;
    private float _outOfBoundsTimer;
    private float _botSpawnTimer;
    private int _mass;
    private int _bots;

    private void Awake()
    {
        _camView = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraView>();
        _meshRenderer = GetComponent<MeshRenderer>();
        _mass = initialMass;
        if (infected)
        {
            _bots = initialBots;
            _meshRenderer.material = infectedMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
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
        // TODO
        // convert mass into bots
        // chance to spawn a bot outside the asteroid (based on number of bots)
        _botSpawnTimer += Time.fixedDeltaTime;
        if (_botSpawnTimer >= botSpawnTime)
        {
            _botSpawnTimer = 0;
            SpawnBot();
        }
        // update asteroid size based on mass
    }

    private void SpawnBot()
    {
        float spawnRadius = transform.localScale.x + botSpawnRadius;
        Vector2 randomCircle = Random.insideUnitCircle;
        Vector2 relSpawnPoint = randomCircle * spawnRadius;
        Vector3 spawnPoint = transform.position + new Vector3(relSpawnPoint.x, relSpawnPoint.y, 0f);
        GameObject newBot = Instantiate(botPrefab, spawnPoint, Quaternion.identity); 
        // Vector3 orientation = randomCircle * Vector3.up;
        // newBot.transform.rotation = Quaternion.Euler(orientation);
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