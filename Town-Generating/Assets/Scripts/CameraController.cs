using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region PUBLIC_FIELDS
    [Header("Rotation")]
    public float rotationSpeed;
    public float rotationMouseSens;
    public float rotationSmoothness;
    [Header("Movement")]
    public float movementSpeed;
    public float movementMouseSens;
    public float movementSmoothness;
    [Header("Zoom")]
    public float zoomSmoothness;
    public float zoomSteps;
    public float minZoomSize;
    #endregion

    #region PRIVATE_FIELDS
    // Values //
    float yDeltaRotation;
    float newZoom;
    Vector3 newPos;
    Quaternion newRotation;

    Vector3 dragStartPos;
    Vector3 dragCurrentPos;

    Vector3 rotateStartPos;
    Vector3 rotateCurrentPos;

    // Inputs //
    // Vector3 movementInput;
    // float rotationInput;

    float maxZoomSize;
    float zoomStep;

    // Bounds //
    Vector3 top;
    Vector3 bottom;
    #endregion

    #region COMPONENTS
    Camera cam;
    #endregion

    void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        yDeltaRotation = transform.rotation.eulerAngles.y;

        newPos = transform.position;
        newRotation = transform.rotation;

        maxZoomSize = cam.orthographicSize;
        newZoom = cam.orthographicSize;
        zoomStep = (maxZoomSize - minZoomSize) / zoomSteps;

        top = GridTransform.FromCoordsToVector3(WorldBuilder.Instance.Grid.lastNode.coords);
        bottom = GridTransform.FromCoordsToVector3(WorldBuilder.Instance.Grid.firstNode.coords);
    }

    void Update()
    {
        MouseRotation();
        MouseMovement();

        KeyboardRotation();
        KeyboardMovement();

        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * movementSmoothness);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSmoothness);

        Zoom();
    }

    #region Keyboard
    void KeyboardRotation()
    {
        float rotationInput;
        if (Input.GetKey(KeyCode.Q))
            rotationInput = 1f;
        else if (Input.GetKey(KeyCode.E))
            rotationInput = -1f;
        else
            return;

        yDeltaRotation = rotationInput * rotationSpeed * Time.deltaTime;
        newRotation *= Quaternion.Euler(Vector3.up * yDeltaRotation);
    }

    void KeyboardMovement()
    {
        Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        Vector3 f = movementInput.z * transform.forward * movementSpeed * Time.deltaTime;
        Vector3 h = movementInput.x * transform.right * movementSpeed * Time.deltaTime;

        newPos += h + f;
        newPos.x = Mathf.Clamp(newPos.x, bottom.x, top.x);
        newPos.z = Mathf.Clamp(newPos.z, bottom.z, top.z);


    }
    #endregion

    #region Mouse
    void MouseMovement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            float entry;

            if (plane.Raycast(ray, out entry))
                dragStartPos = ray.GetPoint(entry);
        }

        if (Input.GetMouseButton(1))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            float entry;

            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPos = ray.GetPoint(entry);

                newPos = newPos + (dragStartPos - dragCurrentPos) * movementMouseSens;
                newPos.x = Mathf.Clamp(newPos.x, bottom.x, top.x);
                newPos.z = Mathf.Clamp(newPos.z, bottom.z, top.z);

                transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * movementSmoothness);
            }
        }
    }

    void MouseRotation()
    {
        if (Input.GetMouseButtonDown(2))
            rotateStartPos = Input.mousePosition;
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPos = Input.mousePosition;

            float diff = rotateStartPos.x - rotateCurrentPos.x;
            rotateStartPos = rotateCurrentPos;

            yDeltaRotation = diff * rotationMouseSens;
            newRotation *= Quaternion.Euler(-Vector3.up * yDeltaRotation);
        }

    }

    void Zoom()
    {
        float zoom;
        if (Input.mouseScrollDelta.y > 0f)
            zoom = -zoomStep;
        else if (Input.mouseScrollDelta.y < 0f)
            zoom = zoomStep;
        else
            zoom = 0f;

        newZoom = Mathf.Clamp(zoom + newZoom, minZoomSize, maxZoomSize);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, newZoom, Time.deltaTime * zoomSmoothness);
    }
    #endregion
}
