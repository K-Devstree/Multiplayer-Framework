using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CombinedCharacter", menuName = "APPack/CreateCombinedCharacter", order = 1)]
public class CombinedCharacter : ScriptableObject
{
    
    public List<Mesh> combinedMeshes = new List<Mesh>();

    public List<Material> materials = new List<Material>();

    [HideInInspector]
    public Material bodyMat;
    [HideInInspector]
    public RuntimeAnimatorController animator;
    [HideInInspector]
    public Avatar avatar;
    [HideInInspector]
    public Dictionary<string, float> blendshapes = new Dictionary<string, float>();
    [HideInInspector]
    public Vector3 hipsScale;
    [HideInInspector]
    public Vector3 headScale;

    public GameObject empty_rig;

    public Object prefab;

    public void ClearData()
    {
#if UNITY_EDITOR
        for (var i = 0; i < combinedMeshes.Count; i++) {
            var path = AssetDatabase.GetAssetPath(combinedMeshes[i]);
            AssetDatabase.DeleteAsset(path);
        }       
        combinedMeshes.Clear();
        if(prefab != null) 
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(prefab));

        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(bodyMat));
#endif
    }

    public void CreatePrefab()
    {
#if UNITY_EDITOR
        var path = AssetDatabase.GetAssetPath(this);
        path = path.Substring(0, path.LastIndexOf("/"));

        GameObject go = new GameObject(string.Format("{0}_prefab", this.name));

        
        float[][] lods_p = new float[][] {
            new float[] {0.5f, 0.2f, 0.05f, 0f },
            new float[] {0.4f, 0.1f, 0f, 0f },
            new float[] {0.3f,   0f,   0f, 0f },
            new float[] {0f,   0f,   0f, 0f }
        };

        var lod_group = go.AddComponent<LODGroup>();

        LOD[] lods = new LOD[combinedMeshes.Count];

        for (var i = 0; i < combinedMeshes.Count; i++)
        {
            var rig = Instantiate(empty_rig, go.transform);           
            var skinnedMesh = rig.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
            var hips = rig.transform.GetChild(1).transform;
            hips.localScale = hipsScale;

            var head = hips.GetComponentsInChildren<Transform>().First(f => f.name == "Head");
            head.localScale = headScale;

            skinnedMesh.sharedMesh = combinedMeshes[i];
            skinnedMesh.materials = materials.ToArray();
            skinnedMesh.updateWhenOffscreen = true;

            for(var blendIndex = 0; blendIndex < skinnedMesh.sharedMesh.blendShapeCount; blendIndex++)
            {
                if(blendshapes.TryGetValue(skinnedMesh.sharedMesh.GetBlendShapeName(blendIndex), out float weight))              
                    skinnedMesh.SetBlendShapeWeight(blendIndex, weight);
            }

            rig.name = string.Format("bake_lod{0}", i);

            var newAnimator = rig.AddComponent<Animator>();

            newAnimator.runtimeAnimatorController = animator;
            newAnimator.avatar = avatar;
            newAnimator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            newAnimator.applyRootMotion = true;

            var renderer = new List<SkinnedMeshRenderer>();
            renderer.Add(skinnedMesh);

            lods[i] = new LOD(lods_p[4 - (combinedMeshes.Count)][i], renderer.ToArray());
        }

        lod_group.SetLODs(lods);
        lod_group.RecalculateBounds();

        var prefab = PrefabUtility.SaveAsPrefabAsset(go, string.Format("{0}/Prefabs/{1}_prefab.prefab", path, this.name));
        this.prefab = prefab;

        EditorUtility.SetDirty(this);

        DestroyImmediate(go);
#endif
    }
}
