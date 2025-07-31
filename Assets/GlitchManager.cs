using UnityEngine;

public class GlitchManager : MonoBehaviour
{
    public Material mat;

    [Header("Glitch Settings")]
    public float activeNoiseAmount = 1f;
    public float activeGlitchStrength = 1f;
    public float activeScanLineStrength = 1f;
    public float effectDuration = 3f;

    private bool isGlitching = false;
    private float timer = 0f;

    void Start()
    {
        // ª‘¥‡Õø‡ø°µÏµÕπ‡√‘Ë¡‡°¡
        mat.SetFloat("_NoiseAmont", 0f);
        mat.SetFloat("_GlitchStrength", 0f);
        mat.SetFloat("_ScanLimeStrength", 0f);
    }

    void Update()
    {
        if (isGlitching)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                StopGlitch();
            }
        }
    }

    public void StartGlitch()
    {
        isGlitching = true;
        timer = effectDuration;

        mat.SetFloat("_NoiseAmont", activeNoiseAmount);
        mat.SetFloat("_GlitchStrength", activeGlitchStrength);
        mat.SetFloat("_ScanLimeStrength", activeScanLineStrength);
    }

    public void StopGlitch()
    {
        isGlitching = false;

        // ª‘¥‡Õø‡ø°µÏ
        mat.SetFloat("_NoiseAmont", 0f);
        mat.SetFloat("_GlitchStrength", 0f);
        mat.SetFloat("_ScanLimeStrength", 0f);
    }
}