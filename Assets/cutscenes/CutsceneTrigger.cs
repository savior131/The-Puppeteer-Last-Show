using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector cutscene;
    [SerializeField] private bool disablePlayerDuringCutscene = true;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;

        if (cutscene != null)
        {
            if (disablePlayerDuringCutscene && other.TryGetComponent<Movement>(out var control))
            {
                cutscene.stopped += (_) =>
                {
                    control.EnableControl(true);
                    Destroy(gameObject);
                };

                control.EnableControl(false);
            }
            else
            {
                cutscene.stopped += (_) => Destroy(gameObject);
            }

            cutscene.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
