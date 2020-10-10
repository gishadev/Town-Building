using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region PUBLIC_FIELDS
    public float rotationSpeed;
    public float smoothness;
    #endregion

    #region PRIVATE_FIELDS
    float yRotation;
    #endregion

    void Start()
    {
        yRotation = transform.rotation.eulerAngles.y;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        yRotation += horizontalInput * Time.deltaTime * rotationSpeed;

        Quaternion newRotation = Quaternion.Euler(Vector3.up * yRotation);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * smoothness);
    }
}
