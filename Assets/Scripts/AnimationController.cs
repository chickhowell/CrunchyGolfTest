using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            animator.SetTrigger("Drive");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            animator.SetTrigger("Chip");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetTrigger("Putt");
        }
    }
}
