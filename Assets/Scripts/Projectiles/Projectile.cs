using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Entity sourceEntity;
    public float speedOfProjectile;
    public enum ProjectileType {Basic, Grenade, Mine, Orbiter, Chaotic, MineWExplosion, Buff}
    public ProjectileType currentType = ProjectileType.Basic;
    public BuffType buffType;

}
