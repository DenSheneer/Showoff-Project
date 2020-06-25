using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DemoCameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform followTarget = null;

    private Vector3 offset = new Vector3();

    private void Awake()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.1f;
        audioSource.loop = true;

        if (Debug.isDebugBuild)
            audioSource.volume = 0;

    }

    void Start()
    {
        offset = this.transform.position - followTarget.position;
    }

    void Update()
    {
        this.transform.position = followTarget.transform.position + offset;
    }
}
