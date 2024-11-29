using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float panSpeed = 6f;
    private void Update()
    {
        Vector2 panPosition = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        transform.position += new Vector3(panPosition.x, 0, panPosition.y) * (panSpeed * Time.deltaTime);
    }
}
