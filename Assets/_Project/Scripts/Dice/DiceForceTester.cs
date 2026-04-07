using UnityEngine;

public class DiceForceTester : MonoBehaviour
{
    [SerializeField] private Vector3 forceVector;
    [SerializeField] private float forceValue;
    [Range(0f, 2f)]
    [SerializeField] private float torqueMultiplier = 0.5f;
    [Tooltip("Tork yönü. Boş bırakılırsa rastgele seçilir.")]
    [SerializeField] private Vector3 torqueAxis;
    [SerializeField] private Rigidbody rb;
    private Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }
    [ContextMenu("Throw Dice")]
    private void Throw()
    {
        if (rb != null)
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.AddForce(forceVector * forceValue, ForceMode.Impulse);
            Vector3 axis = torqueAxis.sqrMagnitude > 0.0001f ? torqueAxis.normalized : Random.insideUnitSphere;
            rb.AddTorque(axis * torqueMultiplier, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("No Rigidbody found on the object.");
        }
    }

    [ContextMenu("Reset Position")]
    private void ResetPosition()
    {
        transform.position = initialPosition;
        transform.rotation = Quaternion.identity;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.useGravity = false;
            rb.isKinematic = true;
        }
        else
        {
            Debug.LogError("No Rigidbody found on the object.");
        }

    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + forceVector * forceValue);
    }
}
