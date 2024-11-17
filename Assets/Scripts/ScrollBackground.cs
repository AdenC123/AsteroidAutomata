using UnityEngine;

public class ScrollBackground : MonoBehaviour {
    public Transform ship;
    public float scrollSpeed = 0.1f;

    private Material _mat;

    void Start() {
        _mat = GetComponent<MeshRenderer>().material;
    }

    void Update() {
        float xoffset = ship.position.x * scrollSpeed / transform.localScale.x;
        float yoffset = ship.position.y * scrollSpeed / transform.localScale.y;
        _mat.mainTextureOffset = new Vector2(xoffset, yoffset);
    }
}
