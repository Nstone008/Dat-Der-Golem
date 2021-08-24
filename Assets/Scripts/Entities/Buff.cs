using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public float buffLength;
    public BuffType buffType;
    public float startTime;
}

public enum BuffType
{
    Untyped = 0,
    FastProjectile = 1,
    MoreKnockback = 2,
    LargerExlosion = 3,
}
