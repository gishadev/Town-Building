using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region PUBLIC_FIELDS
    [Header("Rotation")]
    public float rotationSpeed;
    public float rotationSmoothness;
    [Header("Movement")]
    public float movementSpeed;
    public float movementSmoothness;
    [Header("Zoom")]
    public float zoomSmoothness;
    public float zoomSteps;
    public float minZoomSize;
    #endregion

    #region PRIVATE_FIELDS
    // Values //
    float yRotation;
    float newZoom;
    Vector3 newPos;

    // Inputs //
    Vector3 movementInput;
    float rotationInput;

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
        yRotation = transform.rotation.eulerAngles.y;

        newPos = transform.position;

        maxZoomSize = cam.orthographicSize;
        newZoom = cam.orthographicSize;
        zoomStep = (maxZoomSize - minZoomSize) / zoomSteps;

        top = GridTransform.FromCoordsToVector3(WorldBuilder.Instance.Grid.lastNode.coords);
        bottom = GridTransform.FromCoordsToVector3(WorldBuilder.Instance.Grid.firstNode.coords);
    }

    void Update()
    {
        GetInput();

        Rotation();
        Movement();

        Zoom();
    }

    void Rotation()
    {
        yRotation += rotationInput * rotationSpeed * Time.deltaTime;
        Quaternion newRotation = Quaternion.Euler(Vector3.up * yRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * rotationSmoothness);
    }

    void Movement()
    {
        Vector3 f = movementInput.z * transform.forward * movementSpeed * Time.deltaTime;
        Vector3 h = movementInput.x * transform.right * movementSpeed * Time.deltaTime;

        newPos += h + f;
        newPos.x = Mathf.Clamp(newPos.x, bottom.x, top.x);
        newPos.z = Mathf.Clamp(newPos.z, bottom.z, top.z);

        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * movementSmoothness);
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

    void GetInput()
    {
        movementInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (Input.GetKey(KeyCode.Q))
            rotationInput = 1f;
        else if (Input.GetKey(KeyCode.E))
            rotationInput = -1f;
        else
            rotationInput = 0f;
    }
}
