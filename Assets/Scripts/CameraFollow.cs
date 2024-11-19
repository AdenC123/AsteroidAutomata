using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform _target;

    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate() {
        transform.position = _target.position;
    }
}
