using System;
using UnityEngine;

public class HummerDamage : MonoBehaviour,IDamagingObject
{
    [SerializeField] float damage;
    public float Damage => damage;
}
