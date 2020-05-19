using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public abstract class TapAble : MonoBehaviour
{
    public abstract void Tab();
    public abstract void InRange();
    public abstract void OutOfRange();
}
