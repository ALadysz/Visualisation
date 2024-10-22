using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ColourMaterialCreator : MonoBehaviour
{
    [SerializeField] private Color objectColor = Color.white;

    //when theres a change in the editor
    void OnValidate()
    {
        //apply new color to object
        ApplyMaterial();
    }

    //function that creates a new material and changes its color
    void ApplyMaterial()
    {
        //create a new material with a basic shader (Standard when not using URP)
        Material newMaterial = new Material(Shader.Find("Universal Render Pipeline/Lit"));

        //set the color of the material
        newMaterial.color = objectColor;

        // Apply the material to the object's renderer
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            //set the material in the editor, marking the scene as dirty for saving
            renderer.sharedMaterial = newMaterial;
            EditorUtility.SetDirty(renderer);
        }
        else
        {
            Debug.LogError("Renderer component not found on the object.");
        }
    }
}
