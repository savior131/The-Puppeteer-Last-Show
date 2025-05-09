using UnityEngine;

public class CodeDefendAttack : MonoBehaviour,IParriable,IDamagingObject
{
    [SerializeField] float rageTimer = 15;
    [SerializeField] float attackCoolDown = 5;
    [SerializeField] float damage; 
    public float Damage=>damage;


    public void OnParried()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
