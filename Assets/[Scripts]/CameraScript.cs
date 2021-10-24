//********GAME2014 - MOBILE GAME DEV ASSIGNMENT 1*****************
// CHRIS TULIP 100 818 050
//
// A script to handle movement of the camera. Currently does not work as it is using desktop controls and the build mode is currently set to android.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float panSpeed = 1f;
    float zoom = 1f;
    public float minZoom, maxZoom;
    public TDGrid grid;

    Camera camera;

    private void Awake()
    {
        camera = gameObject.GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        HandleInput();
        
    }

    void HandleInput()
    {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        if (zoomDelta != 0f)
            AdjustZoom(zoomDelta);

        float xDelta = Input.GetAxis("Horizontal");
        float yDelta = Input.GetAxis("Vertical");
        if (xDelta != 0f || yDelta != 0f)
            AdjustPosition(xDelta, yDelta);

        TouchInput();
    }

    void AdjustZoom(float delta)
    {
        zoom = Mathf.Clamp01(zoom + delta);
        float distance = Mathf.Lerp(minZoom, maxZoom, zoom);

        camera.orthographicSize = distance;
        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, distance);
    }

    void AdjustPosition(float x, float y)
    {
        Vector3 direction = new Vector3(x, y).normalized;
        Vector3 position = transform.localPosition;

        position += new Vector3(direction.x * panSpeed, direction.y * panSpeed, 0f);
        transform.localPosition = ClampPosition(position);
    }

    Vector3 ClampPosition(Vector3 position)
    {
        float xMax = (grid.chunkCountX * Config.chunkSize - 0.5f) * Config.tileSize;
        position.x = Mathf.Clamp(position.x, 0f, xMax);

        float yMax = (grid.chunkCountY * Config.chunkSize - 1f) * Config.tileSize;
        position.y = Mathf.Clamp(position.y, 0f, yMax);

        return position;
    }

    void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch temp = Input.GetTouch(0);

            if(temp.phase == TouchPhase.Moved)
            {
                AdjustPosition(-temp.deltaPosition.x, -temp.deltaPosition.y);
            }
        }
    }
}
