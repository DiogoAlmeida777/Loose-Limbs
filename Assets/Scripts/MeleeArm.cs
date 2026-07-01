using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class MeleeArm : Arm
{
    [SerializeField] private Hitbox hitbox;
    [SerializeField] private AudioSource attackSound;
    public UnityEvent<string,string> playAttackAnim;

    private void OnEnable()
    {
        hitbox.setDamage(stats.meleeDamage);
    }

    public override void Attack()
    {
        playAttackAnim?.Invoke(stats.attackAnimStateName, stats.animLayerName);
        attackSound.Play();
    }

    public void OnEnableHitbox(string attackSideString)
    {
        BodySide attackSide = (BodySide)BodySide.Parse(typeof(BodySide), attackSideString);

        if (attackSide != stats.bodySide) return;

        hitbox.gameObject.SetActive(true);
    }


    public void OnDisableHitbox(string attackSideString)
    {
        BodySide attackSide = (BodySide)BodySide.Parse(typeof(BodySide), attackSideString);

        if (attackSide != stats.bodySide) return;

        hitbox.gameObject.SetActive(false);
    }
}
