using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TongueCollider : MonoBehaviour
{
    [SerializeField]
    private TongueController controller = null;

    void OnTriggerStay(Collider collider)
    {
        // We only care about TongueCollectables for now
        if (collider.tag == "TongueCollectable" && controller.Collecting != true)
        {
            CollectableByTongue collectable = collider.GetComponent<CollectableByTongue>();

            if (collectable != null) {
                if (collectable.CollectingWeight > controller.CurrectCollectStrenght)
                    controller.FailCollect(collectable);
                else
                    controller.Collect(collectable);
            }
        } 
    }
}
