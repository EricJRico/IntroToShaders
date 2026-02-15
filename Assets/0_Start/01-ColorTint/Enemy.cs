using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Flash Settings")]
    [SerializeField] private float damageFadeDuration = 0.12f;

    private static readonly int TintStrengthID = Shader.PropertyToID("_TintStrength");
    private Coroutine _flashRoutine;
    private Material _currentMat;
    private Color _originalColor;
    
    private void Start()
    {
        var currentRenderer = GetComponentInChildren<Renderer>();
        _currentMat = new Material(currentRenderer.sharedMaterial);
        currentRenderer.sharedMaterial = _currentMat;
    }

    public void Damage()
    {
        // Restart flash if already running
        if (_flashRoutine != null)
            StopCoroutine(_flashRoutine);

        _flashRoutine = StartCoroutine(FlashRoutine());
    }
    
    IEnumerator FlashRoutine()
    {
        float t = 0f;
        _currentMat.SetFloat(TintStrengthID, 1f); // instant damage flash

        while (t < damageFadeDuration)
        {
            t += Time.deltaTime;
            float value = Mathf.Lerp(1f, 0f, t / damageFadeDuration);
            _currentMat.SetFloat(TintStrengthID, value);
            yield return null;
        }

        _currentMat.SetFloat(TintStrengthID, 0f);
        _flashRoutine = null;
    }
}
