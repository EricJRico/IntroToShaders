using UnityEngine;
using UnityEngine.UI;

public class SetAltTintValues : MonoBehaviour
{
    [SerializeField] private string shaderProperty = "_TintBlend";
    [SerializeField] private Slider slider;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    private int _shaderPropertyID;

    private void Awake()
    {
        if (slider == null || skinnedMeshRenderer == null)
            return;

        _shaderPropertyID = Shader.PropertyToID(shaderProperty);
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void OnSliderValueChanged(float sliderValue)
    {
        if (skinnedMeshRenderer == null) 
            return;
        
        skinnedMeshRenderer.sharedMaterial.SetFloat(_shaderPropertyID, sliderValue);
    }

    private void OnDestroy()
    {
        skinnedMeshRenderer.sharedMaterial.SetFloat(_shaderPropertyID, 0);
    }
}
