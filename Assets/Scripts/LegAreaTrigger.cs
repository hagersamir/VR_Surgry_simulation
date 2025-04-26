using UnityEngine;

public class TableColliderTrigger : MonoBehaviour
{
    public HandAnimatorController handAnimator;
    private bool hasActivated = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasActivated && other.CompareTag("Player"))
        {
            hasActivated = true;
            handAnimator.StartSliding();
        }
    }
}