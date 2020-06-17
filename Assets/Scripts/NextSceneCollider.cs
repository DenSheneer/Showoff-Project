using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider),typeof(Rigidbody))]
public class NextSceneCollider : MonoBehaviour
{
    private static readonly string nextSceneName = "EndScreen";

    private void Start()
    {
        GetComponent<Collider>().isTrigger = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            PlayerManager pm = other.GetComponent<PlayerManager>();
            if (pm != null)
            {
                PlayerInfo.Score = pm.Score;
            }

            NextScene(nextSceneName);
        }
    }

    public void NextScene(string nextSceneName)
    {
        if (Application.CanStreamedLevelBeLoaded(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.Log("scene does not exist or is not in build");
    }
}
