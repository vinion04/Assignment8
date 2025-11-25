using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using UnityEngine.UI;

public class Player :NetworkBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 _moveInput;
    private Text pointsText;
    private NetworkVariable<int> points = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public override void OnNetworkSpawn() {
        pointsText = GetComponentInChildren<Text>();

        points.OnValueChanged += OnPointsChanged;
    }

    public void OnPointsChanged(int oldValue, int newVal)
    {
        pointsText.text = newVal.ToString();
        Debug.Log($"Points text updated!");
    }

    void Update()
    {
        // Movement
        if (!IsOwner) return;

        Vector3 moveDirection = new Vector3(_moveInput.x, _moveInput.y, 0); //note that we are ignoring z for now
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger entered. IsOwner: {IsOwner}, IsSpawned: {IsSpawned}"); // good debug check
        if (!IsOwner) return;

        if (other.CompareTag("Pickup"))
        {
            Destroy(other.gameObject);
            points.Value += 1;  // note the use of the .Value property to update the network variable
            Debug.Log($"Points incremented to {points.Value}"); // just checking
        }
    }

    private void OnDestroy()
    {
        points.OnValueChanged -= OnPointsChanged;
    }

}
