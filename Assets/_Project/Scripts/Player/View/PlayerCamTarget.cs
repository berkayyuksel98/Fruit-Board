using UnityEngine;

public class PlayerCamTarget : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }
}
