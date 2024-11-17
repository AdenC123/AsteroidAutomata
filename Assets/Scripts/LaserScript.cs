using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class LaserScript : MonoBehaviour {
    
    private CameraView _cam;

    void Start() {
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraView>();
    }

    void Update() {
        // delete lasers when they leave double the screen
        float maxX = _cam.GetRight() + _cam.GetScreenWidth();
        float minX = _cam.GetLeft() - _cam.GetScreenWidth();
        float maxY = _cam.GetTop() + _cam.GetScreenHeight();
        float minY = _cam.GetBot() - _cam.GetScreenWidth();

        float x = transform.position.x;
        float y = transform.position.y;

        if (x > maxX || x < minX || y > maxY || y < minY) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other) {
        // destroy laser when it hits any asteroid
        if (other.gameObject.layer == Constants.AsteroidLayer) {
            Destroy(gameObject);
        }
    }
}
