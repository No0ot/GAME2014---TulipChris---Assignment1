//      Author          : Chris Tulip
//      StudentID       : 100818050
//      Date Modified   : October 24, 2021
//      File            : CameraScript.cs
//      Description     : This controls the camera, parts of this script were taken from https://catlikecoding.com/unity/tutorials/hex-map/
//      History         :   v0.5 - Camera Controls added
//                          v1.0 - Implemented Clamping
//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        if (IsPointerOverUIObject())
            return;
        HandleInput();
    }
    /// <summary>
    /// Handles all input. Desktop Input along with calling TouchInput() function
    /// </summary>
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
    /// <summary>
    /// zooms the camera out, only works on desktop.
    /// </summary>
    /// <param name="delta"></param>
    void AdjustZoom(float delta)
    {
        zoom = Mathf.Clamp01(zoom + delta);
        float distance = Mathf.Lerp(minZoom, maxZoom, zoom);

        camera.orthographicSize = distance;
        //transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, distance);
    }
    /// <summary>
    /// Moves the camera around
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    void AdjustPosition(float x, float y)
    {
        Vector3 direction = new Vector3(x, y).normalized;
        Vector3 position = transform.localPosition;

        position += new Vector3(direction.x * panSpeed, direction.y * panSpeed, 0f);
        transform.localPosition = ClampPosition(position);
    }
    /// <summary>
    /// Clamps the camera so it cannot move outside the bounds of the grid.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    Vector3 ClampPosition(Vector3 position)
    {
        float xMax = (grid.chunkCountX * Config.chunkSize - 0.5f) * Config.tileSize;
        position.x = Mathf.Clamp(position.x, 0f, xMax);

        float yMax = (grid.chunkCountY * Config.chunkSize - 1f) * Config.tileSize;
        position.y = Mathf.Clamp(position.y, 0f, yMax);

        return position;
    }
    /// <summary>
    /// Touch Input
    /// </summary>
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
    /// <summary>
    /// EventSystem.current.IsPointerOverGameObject() was not working properly so i did some research and found this function : https://stackoverflow.com/questions/57010713/unity-ispointerovergameobject-issue
    /// </summary>
    /// <returns></returns>
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
