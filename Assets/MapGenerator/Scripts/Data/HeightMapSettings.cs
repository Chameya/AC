using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Terrain/HeightMapSettings")]
public class HeightMapSettings : UpdatableData
{
    public NoiseSettings noiseSettings;

    public bool useFallof;

    public float heightMultiplier;
    public AnimationCurve heightCurve;

    public float minHeight
    {
        get
        {
            return heightMultiplier * heightCurve.Evaluate(0);
        }

    }
    


    public float maxHeight
    {
        get
        {
            return heightMultiplier * heightCurve.Evaluate(1);
        }
    }



#if UNITY_EDITOR
    protected override void OnValidate()
    {
        noiseSettings.ValidateValues();
        base.OnValidate();
    }
    #endif
}
