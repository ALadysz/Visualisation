using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//prettty much the same as animal vision,
//uses bools instead of enum as theres not too many options

public class YeetColourAway : MonoBehaviour
{
    private List<GameObject> trees = new List<GameObject>();
    private List<GameObject> otherTrees = new List<GameObject>();
    private List<GameObject> stones = new List<GameObject>();
    private List<GameObject> mushrooms = new List<GameObject>();
    private List<GameObject> cubes = new List<GameObject>();
    [SerializeField] private List<GameObject> otherObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> fires_PS = new List<GameObject>();
    [SerializeField] private List<GameObject> fires_LIGHT = new List<GameObject>();
    
    private bool protanopia = false;
    private bool tritanopia = false;
    private bool monochromatism = false;

    GameObject[] objects;
    private TMP_Text fun_fact;


    void Start()
    {
        GameObject textObj = GameObject.FindGameObjectWithTag("Text");
        fun_fact = textObj.GetComponent<TMP_Text>();
        fun_fact.text = "1 in 12 men and 1 in 200 women are colour blind!";
    }

    public void SetBool(string colourBool)
    {
        switch (colourBool)
        {
            case "Pro":
                protanopia = true;
                fun_fact.text = "Protanopia is a type of red-green color blindness where red cones are missing, making it hard to distinguish between red and green hues.";
                break;
            case "Tri":
                tritanopia = true;
                fun_fact.text = "Tritanopia is a rare color blindness where blue cones are absent, making it difficult to differentiate between blue and yellow.";
                break;
            case "Mono":
                monochromatism = true;
                fun_fact.text = "Monochromatism, or total color blindness, means seeing the world in shades of gray due to the absence of functioning photoreceptor cones.";
                break;
            default:
                protanopia = false;
                tritanopia = false;
                monochromatism = false;
                break;
        }
    }

    public void SetColours()
    {
        ColourReplacer(true, "Tree", trees);
        ColourReplacer(true, "OtherTree", otherTrees);
        ColourReplacer(true, "Stone", stones);
        ColourReplacer(true, "Mushroom", mushrooms);
        ColourReplacer(true, "Cube", cubes);
        ColourReplacer(false, "Null", otherObjects);
        HandleFires();
        protanopia = false;
        tritanopia = false;
        monochromatism = false;
    }

    void ColourReplacer(bool findTag, string tag, List<GameObject> objectList)
    {
        if(findTag)
        {
            objects = GameObject.FindGameObjectsWithTag(tag); 
            foreach (GameObject obj in objects)
            {
                objectList.Add(obj);
                ColourMaterialCreator colourComponent = obj.GetComponent<ColourMaterialCreator>();
                if (colourComponent != null)
                {
                    Color originalColour = colourComponent.objectColour;
                    colourComponent.ApplyMaterial(InitializeConverter(originalColour));
                }
                else
                {
                    Debug.LogWarning($"{obj.name} does not have a ColourMaterialCreator component.");
                }
            }
        }
        else
        {
            foreach (GameObject obj in objectList)
            {
                ColourMaterialCreator colourComponent = obj.GetComponent<ColourMaterialCreator>();
                if (colourComponent != null)
                {
                    Color originalColour = colourComponent.objectColour;
                    colourComponent.ApplyMaterial(InitializeConverter(originalColour));
                }
                else
                {
                    Debug.LogWarning($"{obj.name} does not have a ColourMaterialCreator component.");
                }
            }
        }
    }

    private void HandleFires()
    {
        foreach(GameObject fire in fires_PS)
        {
            ParticleSystem firePS = fire.GetComponent<ParticleSystem>();
            var main = firePS.main;
            main.startColor = InitializeConverter(main.startColor.color);
        }

        foreach(GameObject fire in fires_LIGHT)
        {
            Light fireLIGHT = fire.GetComponent<Light>();
            fireLIGHT.color = InitializeConverter(fireLIGHT.color);
        }
    }

    private Color InitializeConverter(Color color)
    {
        if (monochromatism)
        {
            return ConvertColours(color, new float[,]
            {
                { 0.299f, 0.587f, 0.114f },
                { 0.299f, 0.587f, 0.114f },
                { 0.299f, 0.587f, 0.114f }
            });
        }
        else if (protanopia)
        {
            return ConvertColours(color, new float[,]
            {
                { 0.567f, 0.433f, 0.0f },
                { 0.558f, 0.442f, 0.0f },
                { 0.0f, 0.242f, 0.758f }
            });
        }
        else if (tritanopia)
        {
            return ConvertColours(color, new float[,]
            {
                { 0.95f, 0.05f, 0.0f },
                { 0.0f, 0.433f, 0.567f },
                { 0.0f, 0.475f, 0.525f }
            });
        }
        return color;
    }

    public Color ConvertColours(Color color, float[,] matrix)
    {
        float R = color.r;
        float G = color.g;
        float B = color.b;

        float newR = R * matrix[0, 0] + G * matrix[0, 1] + B * matrix[0, 2];
        float newG = R * matrix[1, 0] + G * matrix[1, 1] + B * matrix[1, 2];
        float newB = R * matrix[2, 0] + G * matrix[2, 1] + B * matrix[2, 2];

        return new Color(newR, newG, newB, color.a);
    }


}
