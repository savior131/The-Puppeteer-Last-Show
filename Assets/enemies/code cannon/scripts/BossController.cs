
using UnityEngine;
using System.Collections.Generic;

public class BossController : MonoBehaviour
{
    [SerializeField] private GameObject phasesContainer;
    [SerializeField] private Animator animator;
    private List<BossPhase> phases = new List<BossPhase>();
    private int currentPhaseIndex;
    private BossPhase currentPhase;

    private void Awake()
    {
        if (phasesContainer != null)
        {
            phases.AddRange(phasesContainer.GetComponents<BossPhase>());
        }


    }
    void Start()
    {
        if (phases.Count > 0)
        {
            SetPhase(0);
        }
    }

    void Update()
    {
        currentPhase?.UpdatePhase();
    }

    public void NextPhase()
    {
        if (currentPhaseIndex + 1 < phases.Count)
        {
            SetPhase(currentPhaseIndex + 1);
        }
    }
    public int getPhases()
    {
        return phases.Count;
    }
    public int getCurrentPhaseIndex()
    {
        return currentPhaseIndex;
    }

    private void SetPhase(int index)
    {
        if (currentPhase != null)
        {
            currentPhase.EndPhase();
        }
        currentPhaseIndex = index;
        currentPhase = phases[index];
        
        currentPhase.Initialize(this,animator);
        currentPhase.StartPhase();
    }
    public void OnDeath()
    {
        if (currentPhase != null)
        {
            currentPhase.EndPhase();
        }
    }
    public Transform GetBossTransform()
    {
        Transform bossTransform;
        bossTransform = transform.CopyTransform();
        bossTransform.position += Vector3.up * 1.5f;
        return bossTransform;
    }
    public AngleRotation GetAngleRotation()
    {
        return GetComponent<AngleRotation>();
    }
}

