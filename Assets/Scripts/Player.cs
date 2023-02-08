using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 6.0f;
    [SerializeField] private float rotateSpeed = 10.0f;
    private bool _isWalking;

    private void Update()
    {
        var inputVector = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y += 1;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y -= 1;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x -= 1;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x += 1;
        }

        inputVector = inputVector.normalized;
        var moveDirection = new Vector3(inputVector.x, 0.0f, inputVector.y);
        _isWalking = moveDirection != Vector3.zero;
        transform.position += moveDirection * (moveSpeed * Time.deltaTime);
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    public bool IsWalking()
    {
        return _isWalking;
    }
}