using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool occupied = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            occupied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            occupied = false;
        }
    }
}
