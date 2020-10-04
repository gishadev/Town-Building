using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region PUBLIC_FIELDS
    public float rotationSpeed;
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

        transform.rotation = Quaternion.Euler(Vector3.up * yRotation);
    }
}
