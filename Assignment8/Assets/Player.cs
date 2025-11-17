using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;

public class Player : NetworkBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 _moveInput;

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        if (!IsOwner) return;
        // Movement
        Vector3 moveDirection = new Vector3(_moveInput.x, _moveInput.y, 0); //note that we are ignoring z for now
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
