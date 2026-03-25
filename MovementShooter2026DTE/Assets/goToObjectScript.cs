using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class goToObjectScript : MonoBehaviour
{
    [Header("Settings")]
    public float amount = 0.02f;
    public float maxAmount = 0.06f;
    public float smoothAmount = 6f;

    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        //mooose stuf
        float moveX = -Input.GetAxis("Mouse X") * amount;
        float moveY = -Input.GetAxis("Mouse Y") * amount;

        //keeps it in check
        moveX = Mathf.Clamp(moveX, -maxAmount, maxAmount);
        moveY = Mathf.Clamp(moveY, -maxAmount, maxAmount);

        //m ath
        Vector3 finalPosition = new Vector3(moveX, moveY, 0);

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            finalPosition + initialPosition,
            Time.deltaTime * smoothAmount
        );
    }
}
