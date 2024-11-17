using Unity.VisualScripting;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    public GameObject laserPrefab;
    public float shipRotationSpeed = 200f;
    public float shipThrust = 1000f;
    public float shipBrakeDrag = 2f;
    public float laserSpeed = 20f;

    private Rigidbody _rigidbody;
    private float _shipDrag;
    private UIManager _uiManager;
    private bool _destroyed;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
        _shipDrag = _rigidbody.drag;
        _destroyed = false;
    }

    void Update()
    {
        if (_destroyed) return;
        // shoot laser
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Debug.Log("shooting laser");
            GameObject laser = Instantiate(laserPrefab, transform.position, transform.rotation);
            Rigidbody laserRb = laser.GetComponent<Rigidbody>();
            float shipSpeedTowardsFacing = Vector3.Dot(_rigidbody.velocity, transform.up);
            laserRb.velocity = transform.up * (laserSpeed + shipSpeedTowardsFacing);
        }
    }

    void FixedUpdate()
    {
        if (_destroyed) return;
        // rotation
        bool left = Input.GetKey(KeyCode.LeftArrow);
        bool right = Input.GetKey(KeyCode.RightArrow);
        if (left && right)
        {
            // no rotation
        }
        else if (right)
        {
            RotateShip(-shipRotationSpeed * Time.fixedDeltaTime);
        }
        else if (left)
        {
            RotateShip(shipRotationSpeed * Time.fixedDeltaTime);
        }

        // thrust & brake
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _rigidbody.AddForce(transform.up * shipThrust * Time.fixedDeltaTime);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            _rigidbody.drag = shipBrakeDrag;
        }
        else
        {
            _rigidbody.drag = _shipDrag;
        }
    }

    void RotateShip(float angle)
    {
        transform.Rotate(new Vector3(0, 0, angle));
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_destroyed) return;
        // game over when hitting an asteroid
        if (collision.gameObject.layer == Constants.AsteroidLayer)
        {
            _uiManager.GameOver();
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            _destroyed = true;
        }
    }
}