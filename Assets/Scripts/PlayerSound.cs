using Unity.VisualScripting;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioSource footstepAudio;
    [SerializeField] private Animator animator;
    private float lastFootstep;

    private void Update()
    {
        float footstep = animator.GetFloat("footstep");
        if (Mathf.Abs(footstep) < .0001f) footstep = 0f;

        if (lastFootstep > 0 && footstep < 0 || lastFootstep < 0 && footstep > 0)
        {
            footstepAudio.Play();
        }

        lastFootstep = footstep;
    }
    public void OnFootstep()
    {
        //audio.Play();
    }
}
