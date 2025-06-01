using UnityEngine;

public class WaveManager : MonoSingleton<WaveManager>
{
    public float amplitude = 1f;
    public float length = 2f;
    public float speed = 1f;
    public float offset = 0f;

    private void Update()
    {
        offset += Time.deltaTime * speed;
    }

    public float GetWaveHeight(float x)
    {
        return amplitude * Mathf.PerlinNoise(x * 0.05f + offset, 0f);
    }
}
