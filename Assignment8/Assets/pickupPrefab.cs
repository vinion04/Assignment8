using UnityEngine;
using Unity.Netcode;

public class PickupSpawner : NetworkBehaviour
{
    public GameObject pickupPrefab;

    public override void OnNetworkSpawn()
    {
        if (IsServer) // so only server creates the prefabs
        {
            SpawnPickupAt(new Vector3(2, 2, 0));
            SpawnPickupAt(new Vector3(2, 4, 0));
            SpawnPickupAt(new Vector3(-2, -2, 0));
            SpawnPickupAt(new Vector3(-2, -4, 0));
        }
    }

    public void SpawnPickupAt(Vector3 position)
    {
        GameObject pickup = Instantiate(pickupPrefab, position, Quaternion.identity);
        pickup.GetComponent<NetworkObject>().Spawn();  // note netcode function for spawning the object
    }
}
