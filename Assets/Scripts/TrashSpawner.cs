using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TrashSpawner : MonoBehaviour
{
    private BoxCollider boxCollider;
    [SerializeField]
    private List<GameObject> prefabList = new List<GameObject>();

    private float timer = 2f;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            timer = 2.0f;
            SpawnTrash();
        }
    }

    private void SpawnTrash()
    {
        // we have no prefabs to choose from
        if (prefabList.Count <= 0)
            return;

        float x = Random.Range(-boxCollider.bounds.extents.x + transform.position.x + boxCollider.center.x, boxCollider.bounds.extents.x + transform.position.x + boxCollider.center.x);
        float y = Random.Range(-boxCollider.bounds.extents.y + transform.position.y + boxCollider.center.y, boxCollider.bounds.extents.y + transform.position.y + boxCollider.center.y);
        float z = Random.Range(-boxCollider.bounds.extents.z + transform.position.z + boxCollider.center.z, boxCollider.bounds.extents.z + transform.position.z + boxCollider.center.z);

        float scale = Random.Range(0.4f, 1.6f);

        Vector3 randomPos = new Vector3(x, y, z);

        int randomIndex = Random.Range(0, prefabList.Count);

        GameObject trash = Instantiate(prefabList[randomIndex],new Vector3(0,0,0),Quaternion.identity,null);
        trash.transform.localPosition = randomPos;
        trash.transform.localScale = new Vector3(scale, scale, scale);
    }
}
