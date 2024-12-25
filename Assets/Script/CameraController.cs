using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float panSpeed = 6f;
    //[SerializeField] private float edgeThreshold = 10f;
    [SerializeField] private Vector2 movementBounds = new Vector2(-10f, 10f);
    [SerializeField] private float smoothTime = 0.2f;

    private Vector3 targetPosition; 
    private Vector3 velocity = Vector3.zero; 

    private void Start()
    {
        targetPosition = transform.position; 
    }

    private void Update()
    {
        if (CanMoveCameraTopSide())
        {
            targetPosition.z += panSpeed * Time.deltaTime; // Up
        }
        if (CanMoveCameraDownSide())
        {
            targetPosition.z -= panSpeed * Time.deltaTime; // Down
        }

        targetPosition.z = Mathf.Clamp(targetPosition.z, movementBounds.x, movementBounds.y);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    private bool CanMoveCameraTopSide()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height;
    }

    private bool CanMoveCameraDownSide()
    {
        return Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= 0;
    }
}
