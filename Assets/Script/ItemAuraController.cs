using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ItemAuraController : MonoBehaviour
{
    [Header("Aura")]
    public Color auraColor = new Color(0.4f, 0.8f, 1f, 1f);
    public float auraIntensity = 2f;
    public float auraRadius = 3f;

    [Header("Pulse")]
    public bool pulse = true;
    public float pulseSpeed = 2f;
    public float pulseAmount = 0.3f;

    private Light2D auraLight;
    private float baseIntensity;

    void Start()
    {
        auraLight = GetComponent<Light2D>();
        if (auraLight == null)
            auraLight = gameObject.AddComponent<Light2D>();

        auraLight.lightType = Light2D.LightType.Point;
        auraLight.color = auraColor;
        auraLight.intensity = auraIntensity;
        auraLight.pointLightOuterRadius = auraRadius;
        auraLight.pointLightInnerRadius = auraRadius * 0.4f;
        auraLight.enabled = false;

        baseIntensity = auraIntensity;
    }

    void Update()
    {
        if (auraLight == null || !auraLight.enabled || !pulse) return;
        auraLight.intensity = baseIntensity
            + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
    }

    public void EnableAura()
    {
        if (auraLight != null) auraLight.enabled = true;
    }

    public void DisableAura()
    {
        if (auraLight != null) auraLight.enabled = false;
    }
}