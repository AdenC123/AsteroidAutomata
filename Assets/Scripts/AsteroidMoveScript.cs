using System;
using UnityEngine;

public class AsteroidMoveScript : MonoBehaviour {
    public float respawnTime = 0.5f;
    public GameObject mediumAsteroid;
    public GameObject smallAsteroid;

    private CameraView _cam;
    private float _respawnTimer;
    private Rigidbody _rigidBody;
    private UIManager _uiManager;
    
    void Start() {
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraView>();
        _uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        _rigidBody = GetComponent<Rigidbody>();
        _respawnTimer = respawnTime;
    }

    void Update() {
        // check timer
        if (_respawnTimer > 0) {
            _respawnTimer -= Time.deltaTime;
            return;
        }
        // if asteroid is out of frame after initial time, flip it then constrain it
        float x = transform.position.x;
        float y = transform.position.y;
        float offset = transform.localScale.x;
        if (y > _cam.GetTop() + offset   || y < _cam.GetBot() - offset) {
            FlipY();
            _respawnTimer = respawnTime;
        }
        if (x > _cam.GetRight() + offset || x < _cam.GetLeft() - offset) {
            FlipX();
            _respawnTimer = respawnTime;
        }
    }

    void OnTriggerEnter(Collider other) {
        // destroy asteroid when it hits a laser
        if (other.CompareTag("Laser")) {
            _uiManager.IncreaseScore(1);
            if (CompareTag("LargeAsteroid")) {
                SplitAsteroid(mediumAsteroid);
            } else if (CompareTag("MediumAsteroid")) {
                SplitAsteroid(smallAsteroid);
            }
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Spawn two new asteroids off of the current one
    /// </summary>
    /// <param name="newAsteroid">The new asteroid object to spawn</param>
    void SplitAsteroid(GameObject newAsteroid) {
        float offset = newAsteroid.transform.localScale.x / 2;
        Vector3 velocityOff90 = Quaternion.AngleAxis(90, Vector3.forward) * Vector3.Normalize(_rigidBody.velocity);
        Vector3 pos1 = transform.position + velocityOff90 * offset;
        Vector3 pos2 = transform.position - velocityOff90 * offset;
        Vector3 vel1 = Quaternion.AngleAxis(30, Vector3.forward) * _rigidBody.velocity;
        Vector3 vel2 = Quaternion.AngleAxis(-30, Vector3.forward) * _rigidBody.velocity;

        GameObject asteroid1 = Instantiate(newAsteroid, pos1, new Quaternion());
        asteroid1.GetComponent<Rigidbody>().velocity = vel1;
        GameObject asteroid2 = Instantiate(newAsteroid, pos2, new Quaternion());
        asteroid2.GetComponent<Rigidbody>().velocity = vel2;
    }

    /// <summary>
    /// Flips the asteroids's x position about the camera, constraining it to the edge of the camera.
    /// </summary>
    private void FlipX() {
        float offset = transform.localScale.x;
        float newX = (_cam.GetX() * 2) - transform.position.x;
        newX = Mathf.Clamp(newX, _cam.GetLeft() - offset, _cam.GetRight() + offset);
        transform.position = new Vector3(newX, transform.position.y);
    }

    /// <summary>
    /// Flips the asteroids's y position about the camera, constraining it to the edge of the camera.
    /// </summary>
    private void FlipY() {
        float offset = transform.localScale.x;
        float newY = (_cam.GetY() * 2) - transform.position.y;
        newY = Mathf.Clamp(newY, _cam.GetBot() - offset, _cam.GetTop() + offset);
        transform.position = new Vector3(transform.position.x, newY);
    }
}
