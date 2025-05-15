using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlamethrowerManager : MonoBehaviour
{
    [SerializeField] private float activeDuration = 2f;
    [SerializeField] private float inactiveDuration = 2f;

    [SerializeField] private List<Animator> groupA;
    [SerializeField] private List<Animator> groupB;

    private static readonly int PlayFlame = Animator.StringToHash("open fire");

    private void Start()
    {
        StartCoroutine(FlameRoutine());
    }

    private IEnumerator FlameRoutine()
    {
        while (true)
        {
            ToggleGroup(groupA, true);
            ToggleGroup(groupB, false);
            yield return new WaitForSeconds(activeDuration);

            ToggleGroup(groupA, false);
            ToggleGroup(groupB, true);
            yield return new WaitForSeconds(activeDuration);
        }
    }

    private void ToggleGroup(List<Animator> group, bool on)
    {
        foreach (var anim in group)
        {
            anim.SetBool(PlayFlame, on);
        }
    }
}
