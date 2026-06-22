using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("Body Health")]
    [SerializeField] private BodyHealth bodyHealth;
    [SerializeField] private Image healthBarFill;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Limbs")]
    [SerializeField] private LimbsManager limbsManager;

    [Header("Limb Diagram Images")]
    [SerializeField] private Image headImage;
    [SerializeField] private Image torsoImage;
    [SerializeField] private Image leftArmImage;
    [SerializeField] private Image rightArmImage;
    [SerializeField] private Image leftLegImage;
    [SerializeField] private Image rightLegImage;

    [Header("Diagram Colors")]
    [SerializeField] private Color healthyColor = Color.cyan;
    [SerializeField] private Color damagedColor = Color.red;
    [SerializeField] private Color destroyedColor = Color.gray;

    void Update()
    {
        if (bodyHealth == null) return;

        float current = bodyHealth.currentHealth;
        float max = bodyHealth.MaxHealth;
        UpdateHealthBar(current, max);

        // Head and torso share the same HP pool as the main health bar
        UpdateDiagramPiece(headImage, current, max);
        UpdateDiagramPiece(torsoImage, current, max);

        // Each swappable limb has its own LimbHealth, fetched live via LimbsManager
        UpdateLimbPiece(leftArmImage, limbsManager.currentLeftArm);
        UpdateLimbPiece(rightArmImage, limbsManager.currentRightArm);
        UpdateLimbPiece(leftLegImage, limbsManager.currentLeftLeg);
        UpdateLimbPiece(rightLegImage, limbsManager.currentRightLeg);
    }

    void UpdateHealthBar(float current, float max)
    {
        float fillAmount = max > 0 ? current / max : 0f;
        healthBarFill.fillAmount = Mathf.Clamp01(fillAmount);
        healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }

    // Blends healthyColor -> damagedColor -> destroyedColor as t goes from 1 to 0.
    // Above the midpoint: healthy to damaged. Below it: damaged to destroyed (gray at exactly 0).
    Color GetHealthColor(float t)
    {
        t = Mathf.Clamp01(t);
        const float midpoint = 0.5f;

        if (t >= midpoint)
        {
            float localT = (t - midpoint) / (1f - midpoint); // 0..1 across the upper half
            return Color.Lerp(damagedColor, healthyColor, localT);
        }
        else
        {
            float localT = t / midpoint; // 0..1 across the lower half
            return Color.Lerp(destroyedColor, damagedColor, localT);
        }
    }

    // Used for head/torso, which always exist and share bodyHealth's values directly
    void UpdateDiagramPiece(Image image, float current, float max)
    {
        if (image == null) return;

        float t = max > 0 ? current / max : 0f;
        image.color = GetHealthColor(t);
    }

    // Used for swappable limbs, which can be null (destroyed/unequipped)
    void UpdateLimbPiece(Image image, GameObject limbObject)
    {
        if (image == null) return;

        if (limbObject == null)
        {
            image.color = destroyedColor;
            return;
        }

        LimbHealth limbHealth = limbObject.GetComponent<LimbHealth>();
        if (limbHealth == null)
        {
            image.color = destroyedColor;
            return;
        }

        float t = limbHealth.MaxHealth > 0
            ? limbHealth.currentHealth / limbHealth.MaxHealth
            : 0f;
        image.color = GetHealthColor(t);
    }
}
