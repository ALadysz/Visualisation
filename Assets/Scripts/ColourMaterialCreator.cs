using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class ColourMaterialCreator : MonoBehaviour
{
    public Color objectColour = Color.white;

    //when theres a change in the editor
    void OnValidate()
    {
        //apply new color to object
        ApplyMaterial(objectColour);
    }

    //function that creates a new material and changes its color
    public void ApplyMaterial(Color colour)
    {
        //create a new material with a basic shader (Standard when not using URP)
        Material newMaterial = new Material(Shader.Find("Standard"));

        //set the color of the material
        newMaterial.color = colour;

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
