using UnityEngine;

public abstract class BossPhase : MonoBehaviour
{
    protected BossController boss;
    protected AngleRotation angleRotation;
    private new Transform transform;
    public void Initialize(BossController bossController,Transform bossTransfrom)
    {
        boss = bossController;
        angleRotation=boss.GetAngleRotation();
        transform = bossTransfrom;
    }

    public abstract void StartPhase();
    public abstract void UpdatePhase();
    public abstract void EndPhase();
    public void fire(GameObject bulletPrefab,Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Projectile>().setReturnTarget(transform);
    }

}
