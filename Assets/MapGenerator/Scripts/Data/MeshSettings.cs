using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Terrain/MeshSettings")]
public class MeshSettings : UpdatableData
{
    public const int numSupportedLODs = 5;
    public const int numSupportedChunkSizes = 9;
    public const int numSupportedFlatshadedChunkSizes = 3;
    public static readonly int[] supportedChunkSizes = { 48, 72, 96, 120, 144, 168, 192, 216, 240 };

    public float meshScale = 2f;
    public bool useFlatShading;

    [Range(0f, numSupportedChunkSizes - 1)]
    public int chunkSizeIndex;
    [Range(0f, numSupportedFlatshadedChunkSizes - 1)]
    public int flatshadedChunkSizeIndex;

    //num vertices per line of a mesh rendered at LOD = 0 "highest".Includes the 2 extra vertices that are excluded from final mesh, but used for calculating normals
    public int numVertsPerLine
    {
        get
        {
            return supportedChunkSizes[(useFlatShading) ? flatshadedChunkSizeIndex : chunkSizeIndex] + 5;
        }
    }

    public float meshWorldSize
    {
        get
        {
            return (numVertsPerLine-3)*meshScale; 
        }
    }
}
