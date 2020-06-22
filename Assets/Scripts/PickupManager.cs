using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using UnityEditor;

public class PickupManager : MonoBehaviour
{
    [SerializeField]
    List<TapAble> tapAbles = null;

    Lantern[] lanterns;

    TrashSpawner ts;

    PlayerManager playerManager = null;
    UI_Manager debugUI = null;

    int tapMask;
    int playerMask;
    float idleTime = 0.0f;

    void Awake()
    {
        GetAllTapables();

        LeanTouch.OnFingerTap += HandleFingerTap;

        tapMask = LayerMask.GetMask("TapLayer");
        playerMask = LayerMask.GetMask("Player");

        playerManager = FindObjectOfType<PlayerManager>();
        playerManager.onTapableChange += updateLevelItems;

        debugUI = FindObjectOfType<UI_Manager>();
        playerManager.OnFireflyChange += debugUI.UpdateFireflyComponent;

        ts = FindObjectOfType<TrashSpawner>();

        List<Lantern> tempLanternList = new List<Lantern>();
        foreach (TapAble tapAble in tapAbles)
        {
            tapAble.ExitEvent += playerManager.TapAbleOutOfReach;

            if (tapAble is Lantern)
            {
                tempLanternList.Add(tapAble as Lantern);
                (tapAble as Lantern).SubscribeToBeetleSpawnEvent(updateLevelItems);
                (tapAble as Lantern).onLitEvent += updateLevelItems;
            }
        }
        lanterns = tempLanternList.ToArray();
    }

    public void AddTapble(TapAble pTapAble)
    {
        if (pTapAble != null && (!tapAbles.Contains(pTapAble)))
        {
            tapAbles.Add(pTapAble);
        }
    }


    void HandleFingerTap(LeanFinger finger)
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, tapMask))
        {
            if (hit.collider != null)
            {
                TapAble tapAble = hit.collider.gameObject.GetComponent<TapAble>();
                if (tapAble != null)
                {
                    tapAble.Tab();
                    playerManager.HandleTapAble(tapAble);
                }
            }
        }
    }

    void updateLevelItems(TapAble tapAble)
    {
        if (tapAbles.Contains(tapAble))
        {
            tapAbles.Remove(tapAble);
        }
        else if (!tapAbles.Contains(tapAble))
        {
            tapAbles.Add(tapAble);
        }

    }

    private void FixedUpdate()
    {
        foreach (TapAble tapAble in tapAbles)
        {
            if (playerManager.CheckInReach(tapAble))
            {
                tapAble.InRange();
                playerManager.HandleInReachTapAble(tapAble);
            }
            else
            {
                tapAble.OutOfRange();
            }
        }
    }

    private void Update()
    {
        if (LeanTouch.Fingers.Count < 1)
        {
            if (timerUntilReset(30.0f))
                SceneLoader.LoadScene(SceneLoader.StartScreenSceneName);
        }
        else
            idleTime = 0.0f;

        ts.gameObject.SetActive(!checkAllLanternRadii());
    }

    public void GetAllTapables()
    {
        GameObject[] allGameObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject gameObject in allGameObjects)
        {
            TapAble tapAble = gameObject.GetComponent<TapAble>();

            if (tapAble != null)
                AddTapble(tapAble);
        }
    }

    bool checkAllLanternRadii()     //  If the player is inside the radius of ANY lantern, return true. Else, return false.
    {
        foreach (Lantern lantern in lanterns)
            if (lantern.IsLit)
                if (lantern.InRadiusCheck(playerManager.Position, playerMask))
                    return true;

        return false;
    }

    bool timerUntilReset(float time)
    {
        idleTime += Time.deltaTime;
        if (idleTime >= time)
            return true;

        return false;
    }
}
