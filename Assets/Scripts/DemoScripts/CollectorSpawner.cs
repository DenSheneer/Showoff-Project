using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CollectorSpawner : MonoBehaviour
{
    [SerializeField]
    private CollectableByTongue collectablePrefab;

    [SerializeField]
    private int maxObjects = 10;
    
    [SerializeField]
    private int spawnTimer = 4;
    private float spawnCountDown = 0;

    void Start()
    {

    }
    
    void Update()
    {
        if (spawnCountDown <= 0)
        {
            if (this.transform.childCount < 10)
            {
                SpawnCollectable();
                spawnCountDown = spawnTimer;
            }

        } else
        {
            spawnCountDown -= Time.smoothDeltaTime;
        }
    }


    private void SpawnCollectable()
    {
        Debug.Log("try spawn collectable");

        if (collectablePrefab == null)
            return;

        CollectableByTongue collectable = Instantiate<CollectableByTongue>(collectablePrefab, this.transform);

        float x = Random.Range(0, 0.5f);
        float z = Random.Range(0, 0.5f);

        collectable.transform.localPosition = new Vector3(x, 0, z);
    }

}
