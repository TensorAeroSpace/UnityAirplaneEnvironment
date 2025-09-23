using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class RipMaps : MonoBehaviour
{
    // Going to try and grab the mesh and texture for each panel in the map

    public void Rip()
    {
        Debug.Log("Starting rip");
        GameObject rootObject = new GameObject("Ripped Map");

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject;
            if (go != null)
            {
                MeshRenderer meshRenderer = go.GetComponent<MeshRenderer>();
                MeshFilter meshFilter = go.GetComponent<MeshFilter>();
                if (meshRenderer != null && meshFilter != null)
                {
                    // Create a copy of the object
                    GameObject clonePanel = new GameObject(i.ToString());

                    // Copy the position
                    clonePanel.transform.position = go.transform.position;

                    // Copy the mesh
                    MeshFilter cloneFilter = clonePanel.AddComponent<MeshFilter>();
                    MeshRenderer cloneRenderer = clonePanel.AddComponent<MeshRenderer>();
                    cloneFilter.sharedMesh = meshFilter.sharedMesh;
#if UNITY_EDITOR
                    AssetDatabase.CreateAsset(cloneFilter.sharedMesh, "Assets/Ripped Map/" + "mesh" + i.ToString() + ".mesh");
#endif

                    // Copy the texture
                    Texture2D texture = Instantiate(GetTexture(meshRenderer));
                    // Encode texture into PNG
                    byte[] bytes = ImageConversion.EncodeToPNG(texture);
                    File.WriteAllBytes("Assets/Ripped Map/" + "tex" + i.ToString() + ".png", bytes);
#if UNITY_EDITOR
                    AssetDatabase.Refresh();
#endif

                    // Copy the material
                    Material cloneMaterial = new Material(Shader.Find("Standard"));
                    cloneMaterial.CopyPropertiesFromMaterial(meshRenderer.sharedMaterial);

#if UNITY_EDITOR
                    cloneMaterial.SetTexture("_MainTex", (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/Ripped Map/" + "tex" + i.ToString() + ".png", typeof(Texture2D)));
#endif
                    cloneRenderer.material = cloneMaterial;
#if UNITY_EDITOR
                    AssetDatabase.CreateAsset(cloneMaterial, "Assets/Ripped Map/" + "mat" + i.ToString() + ".mat");
#endif

                    clonePanel.transform.SetParent(rootObject.transform, true);
                }
            }
        }
        Debug.Log("Done ripping");
    }

    private Texture2D GetTexture(MeshRenderer meshRenderer)
    {
        return (Texture2D)meshRenderer.sharedMaterial.mainTexture;
    }

}
