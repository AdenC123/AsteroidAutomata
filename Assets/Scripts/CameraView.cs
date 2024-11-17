using UnityEngine;

public class CameraView : MonoBehaviour
{
    private float _screenWidth;
    private float _screenHeight;
    private float _camSize;

    private void Start()
    {
        _camSize = GetComponent<Camera>().orthographicSize;
    }

    private void UpdateWidthAndHeight()
    {
        float angle = Mathf.Abs(transform.eulerAngles.x) * Mathf.Deg2Rad;
        _screenHeight = _camSize / Mathf.Cos(angle);
        float screenAspect = (float)Screen.width / Screen.height;
        _screenWidth = _camSize * screenAspect;
    }

    public float GetTop()
    {
        UpdateWidthAndHeight();
        return transform.position.y + _screenHeight;
    }

    public float GetBot()
    {
        UpdateWidthAndHeight();
        return transform.position.y - _screenHeight;
    }

    public float GetLeft()
    {
        UpdateWidthAndHeight();
        return transform.position.x - _screenWidth;
    }

    public float GetRight()
    {
        UpdateWidthAndHeight();
        return transform.position.x + _screenWidth;
    }

    public float GetX()
    {
        return transform.position.x;
    }

    public float GetY()
    {
        return transform.position.y;
    }

    public float GetScreenHeight()
    {
        return _screenHeight;
    }

    public float GetScreenWidth()
    {
        return _screenWidth;
    }
}