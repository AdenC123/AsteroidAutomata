using UnityEngine;

public class AsteroidSpawnScript : MonoBehaviour
{
    public GameObject largeAsteroid;
    public float spawnRate = 2f;
    public float spawnVelocity = 2f;

    private float _spawnTimer;
    private float _screenSpawnOffset;
    private CameraView _cam;

    void Start() {
        _spawnTimer = 0;
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraView>();
        // spawn offset is asteroid radius, use x
        _screenSpawnOffset = largeAsteroid.transform.localScale.x;
    }

    void Update() {
        // update timer
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0) {
            SpawnLargeAsteroid();
            _spawnTimer = spawnRate;
        }
    }

    /// <summary>
    /// Spawn a large asteroid directly outside the field of view, heading towards the player.
    /// </summary>
    [ContextMenu("Spawn Large Asteroid")]
    public void SpawnLargeAsteroid() {
        // get spawn locations
        float spawnTop = _cam.GetTop() + _screenSpawnOffset;
        float spawnBot = _cam.GetBot() - _screenSpawnOffset;
        float spawnRight = _cam.GetRight() + _screenSpawnOffset;
        float spawnLeft = _cam.GetLeft() - _screenSpawnOffset;

        // choose random position and direction
        float side = Random.Range(0, 4);
        float x;
        float y;
        float xVel;
        float yVel;
        if (side < 1) {
            // top
            y = spawnTop;
            x = Random.Range(spawnLeft, spawnRight);
            xVel = Random.Range(-1, 1);
            yVel = -1;
        } else if (side < 2) {
            // bot
            y = spawnBot;
            x = Random.Range(spawnLeft, spawnRight);
            xVel = Random.Range(-1, 1);
            yVel = 1;
        } else if (side < 3) {
            // right
            x = spawnRight;
            y = Random.Range(spawnBot, spawnTop);
            xVel = -1;
            yVel = Random.Range(-1, 1);
        } else {
            // left
            x = spawnLeft;
            y = Random.Range(spawnBot, spawnTop);
            xVel = 1;
            yVel = Random.Range(-1, 1);
        }

        GameObject asteroid = Instantiate(largeAsteroid, new Vector3(x, y), new Quaternion());
        Vector3 asteroidVel = new Vector3(xVel, yVel).normalized * spawnVelocity;
        asteroid.GetComponent<Rigidbody>().velocity = asteroidVel;
    }
}
