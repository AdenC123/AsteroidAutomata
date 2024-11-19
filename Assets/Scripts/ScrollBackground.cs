using UnityEngine;

public class ScrollBackground : MonoBehaviour {

    [SerializeField] private float scrollSpeed = 0.1f;

    private Material _mat;
    private Transform _ship;

    void Awake() {
        _mat = GetComponent<MeshRenderer>().material;
        _ship = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        float xoffset = _ship.position.x * scrollSpeed / transform.localScale.x;
        float yoffset = _ship.position.y * scrollSpeed / transform.localScale.y;
        _mat.mainTextureOffset = new Vector2(xoffset, yoffset);
    }
}
