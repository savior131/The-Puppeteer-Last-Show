using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public abstract class BossPhase : MonoBehaviour
{
    protected BossController boss;
    protected AngleRotation angleRotation;
    protected Animator animator;

    private new Transform transform;
    public void Initialize(BossController bossController,Animator animator)
    {
        boss = bossController;
        angleRotation=boss.GetAngleRotation();
        transform = boss.GetBossTransform();
       
        this.animator = animator;
    }

    public abstract void StartPhase();
    public abstract void UpdatePhase();
    public abstract void EndPhase();
    public void fire(GameObject bulletPrefab,Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        animator.SetTrigger("fire");
        bullet.GetComponent<Projectile>().setReturnTarget(transform);
       
    }


}
