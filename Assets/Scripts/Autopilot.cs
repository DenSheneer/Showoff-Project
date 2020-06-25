using UnityEngine;

public class Autopilot : MonoBehaviour
{
    [SerializeField]
    private GameObject picnicBench;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerManager>().AutoMove(picnicBench.transform.position);
        }
    }
}
