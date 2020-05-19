using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempPlayerManager : MonoBehaviour
{
    [SerializeField]
    private int lives = 5;

    public void playerHit(int hitStrenght = 1)
    {
        lives -= hitStrenght;

        Debug.Log("lost a life, you have " + lives + " lives left");

        if (lives <= 0)
        {
            lives = 0;

            Debug.Log("Game over");
            Destroy(this.gameObject);
        }
    }
}
