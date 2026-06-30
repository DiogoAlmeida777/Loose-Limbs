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

    private LimbHealth currentLeftArmHealth;
    private LimbHealth currentRightArmHealth;
    private LimbHealth currentLeftLegHealth;
    private LimbHealth currentRightLegHealth;

    private void OnEnable()
    {
        if (bodyHealth != null)
        {
            bodyHealth.OnHealthChanged.AddListener(OnBodyHealthChanged);
        }

        if (limbsManager != null)
            limbsManager.OnLimbChanged.AddListener(OnLimbChanged);
    }

    private void Start()
    {
        if (bodyHealth != null)
            OnBodyHealthChanged(bodyHealth.currentHealth, bodyHealth.MaxHealth);
    }

    private void OnDisable()
    {
        if (bodyHealth != null)
            bodyHealth.OnHealthChanged.RemoveListener(OnBodyHealthChanged);

        if (limbsManager != null)
            limbsManager.OnLimbChanged.RemoveListener(OnLimbChanged);

        UnsubscribeLimb(ref currentLeftArmHealth);
        UnsubscribeLimb(ref currentRightArmHealth);
        UnsubscribeLimb(ref currentLeftLegHealth);
        UnsubscribeLimb(ref currentRightLegHealth);
    }

    private void OnBodyHealthChanged(float current, float max)
    {
        float fillAmount = max > 0 ? current / max : 0f;
        healthBarFill.fillAmount = Mathf.Clamp01(fillAmount);
        healthText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";

        UpdateDiagramPiece(headImage, current, max);
        UpdateDiagramPiece(torsoImage, current, max);
    }

    private void OnLimbChanged(BodySide side, bool isArm, GameObject newLimbObject)
    {
        if (isArm)
        {
            if (side == BodySide.Left)
                SwapLimbSubscription(ref currentLeftArmHealth, newLimbObject, leftArmImage);
            else
                SwapLimbSubscription(ref currentRightArmHealth, newLimbObject, rightArmImage);
        }
        else
        {
            if (side == BodySide.Left)
                SwapLimbSubscription(ref currentLeftLegHealth, newLimbObject, leftLegImage);
            else
                SwapLimbSubscription(ref currentRightLegHealth, newLimbObject, rightLegImage);
        }
    }

    private void SwapLimbSubscription(ref LimbHealth cachedHealth, GameObject newLimbObject, Image limbImage)
    {
        UnsubscribeLimb(ref cachedHealth);

        if (newLimbObject == null)
        {
            if (limbImage != null)
                limbImage.color = destroyedColor;
            return;
        }

        LimbHealth newHealth = newLimbObject.GetComponent<LimbHealth>();
        if (newHealth == null)
        {
            if (limbImage != null)
                limbImage.color = destroyedColor;
            return;
        }

        cachedHealth = newHealth;

        cachedHealth.OnHealthChanged.AddListener((current, max) => UpdateDiagramPiece(limbImage, current, max));

        UpdateDiagramPiece(limbImage, newHealth.currentHealth, newHealth.MaxHealth);
    }

    private void UnsubscribeLimb(ref LimbHealth cachedHealth)
    {
        if (cachedHealth != null)
        {
            cachedHealth.OnHealthChanged.RemoveAllListeners();
            cachedHealth = null;
        }
    }

    private Color GetHealthColor(float t)
    {
        t = Mathf.Clamp01(t);
        const float midpoint = 0.5f;

        if (t >= midpoint)
        {
            float localT = (t - midpoint) / (1f - midpoint);
            return Color.Lerp(damagedColor, healthyColor, localT);
        }
        else
        {
            float localT = t / midpoint;
            return Color.Lerp(destroyedColor, damagedColor, localT);
        }
    }

    private void UpdateDiagramPiece(Image image, float current, float max)
    {
        if (image == null) return;
        float t = max > 0 ? current / max : 0f;
        image.color = GetHealthColor(t);
    }
}