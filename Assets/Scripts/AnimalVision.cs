using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimalVision : MonoBehaviour
{
    //objects to change the colour of
    private List<GameObject> trees = new List<GameObject>();
    private List<GameObject> otherTrees = new List<GameObject>();
    private List<GameObject> stones = new List<GameObject>();
    private List<GameObject> mushrooms = new List<GameObject>();
    private List<GameObject> cubes = new List<GameObject>();
    [SerializeField] private List<GameObject> otherObjects = new List<GameObject>();
    [SerializeField] private List<GameObject> fires_PS = new List<GameObject>();
    [SerializeField] private List<GameObject> fires_LIGHT = new List<GameObject>();
    
    //enum so the function can select an animal
    public enum AnimalType { Nothing, Bee, Bird, Snake, Shark, Cat, Dog }
    public AnimalType selectedAnimal = AnimalType.Nothing;
    
    //other variables
    private GameObject[] objects;
    private bool isSnake = false; //necessary to make fires seem infrared
    private TMP_Text fun_fact;

    private void Start()
    {
        //set beginning text
        GameObject textObj = GameObject.FindGameObjectWithTag("Text");
        fun_fact = textObj.GetComponent<TMP_Text>();
        fun_fact.text = "Animals see the world very differently from humans, interact with the buttons to see how they would see this scene!";        
    }

    //set which animal to make the vision seem like + set text
    public void SetEnun(string animal)
    {   
        switch (animal)
        {
            case "Cat":
                selectedAnimal = AnimalType.Cat;
                fun_fact.text = "Cats see well in low light and detect motion easily but perceive fewer colors than humans.";
                break;
            case "Dog":
                selectedAnimal = AnimalType.Dog;
                fun_fact.text = "Dogs primarily see shades of blue and yellow due to their dichromatic vision.";
                break;
            case "Bee":
                selectedAnimal = AnimalType.Bee;
                fun_fact.text = "Bees see ultraviolet light patterns on flowers, helping them locate nectar.";
                break;
            case "Bird":
                selectedAnimal = AnimalType.Bird;
                fun_fact.text = "Birds have tetrachromatic vision, allowing them to see ultraviolet light and a wider color spectrum than humans.";
                break;
            case "Snake":
                selectedAnimal = AnimalType.Snake;
                fun_fact.text = "Snakes detect infrared radiation to locate warm-blooded prey, even in darkness.";
                break;
            case "Shark":
                selectedAnimal = AnimalType.Shark;
                fun_fact.text = "Sharks have excellent contrast sensitivity but see in black and white.";
                break;
            default:
                selectedAnimal = AnimalType.Nothing;
                break;
        }
    }

    //go through each list changing its colour
    private void SetColours()
    {
        ColourReplacer(true, "Tree", trees);
        ColourReplacer(true, "OtherTree", otherTrees);
        ColourReplacer(true, "Stone", stones);
        ColourReplacer(true, "Mushroom", mushrooms);
        ColourReplacer(true, "Cube", cubes);
        ColourReplacer(false, "Null", otherObjects);
        HandleFires();
        isSnake = false;
    }

    //replace colours using ColourMaterialCreator as each object has it
    private void ColourReplacer(bool findTag, string tag, List<GameObject> objectList)
    {
        //check for tag bool as some lists are already set and dont need to find a tag
        if(findTag)
        {
            objects = GameObject.FindGameObjectsWithTag(tag); 
            foreach (GameObject obj in objects)
            {
                objectList.Add(obj);
                ColourMaterialCreator colourComponent = obj.GetComponent<ColourMaterialCreator>();
                if (colourComponent != null)
                {

                    colourComponent.ApplyMaterial(InitializeConverter(colourComponent.objectColour));
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

    //change the colours of fire
    private void HandleFires()
    {
        //if its a snake, make the fires + lights red/orange to simulate infrared
        if(isSnake)
        {
            foreach(GameObject fire in fires_PS)
            {
                ParticleSystem firePS = fire.GetComponent<ParticleSystem>();
                var main = firePS.main;
                float r = Mathf.Clamp01(main.startColor.color.r * 1.5f);
                float g = Mathf.Clamp01(main.startColor.color.g * 0.5f);
                float b = Mathf.Clamp01(main.startColor.color.b * 0.1f);
                main.startColor = new Color(r, g, b, 255.0f);
            }

            foreach(GameObject fire in fires_LIGHT)
            {
                Light fireLIGHT = fire.GetComponent<Light>();
                float r = Mathf.Clamp01(fireLIGHT.color.r * 1.5f);
                float g = Mathf.Clamp01(fireLIGHT.color.g * 0.5f);
                float b = Mathf.Clamp01(fireLIGHT.color.b * 0.1f);
                fireLIGHT.color = new Color(r, g, b, 255.0f);
            }
        }
        //otherwise use initialize converter to find the colour of the fire + light
        else
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

    }

    //depending on enum, call relevant function to convert colour
    private Color InitializeConverter(Color color)
    {        
        Color convertedColor = color;

        switch (selectedAnimal)
        {
            case AnimalType.Bee:
                convertedColor = SimulateBeeVision(color);
                break;
            case AnimalType.Bird:
                convertedColor = SimulateBirdVision(color);
                break;
            case AnimalType.Snake:
                convertedColor = SimulateSnakeVision(color);
                isSnake = true;
                break;
            case AnimalType.Shark:
                convertedColor = SimulateSharkVision(color);
                break;
            case AnimalType.Cat:
                convertedColor = SimulateCatVision(color);
                break;
            case AnimalType.Dog:
                convertedColor = SimulateDogVision(color);
                break;
        }

        return convertedColor;
    }


    // functions to store matrixes and calculate updated colour using them
    private Color SimulateBeeVision(Color color)
    {
        float[,] beeMatrix = {
            { 0.5f, 0.5f, 0.0f },    //enhanced blue and green, no red
            { 0.0f, 0.8f, 0.2f },    //stronger green sensitivity
            { 0.3f, 0.3f, 0.4f }     //simulates UV sensitivity
        };
        return ConvertColours(color, beeMatrix);
    }

    private Color SimulateBirdVision(Color color)
    {
        float[,] birdMatrix = {
            { 0.9f, 0.1f, 0.0f },    //red and UV-sensitive
            { 0.0f, 0.8f, 0.2f },    //balanced green perception
            { 0.1f, 0.3f, 0.6f }     //enhanced blue sensitivity
        };
        return ConvertColours(color, birdMatrix);
    }

    private Color SimulateSnakeVision(Color color)
    {
        float[,] snakeMatrix = {
            { 0.3f, 0.3f, 0.2f },    //limited red sensitivity
            { 0.5f, 0.5f, 0.0f },    //moderate green sensitivity
            { 0.2f, 0.4f, 0.4f }     //infrared-like perception
        };
        return ConvertColours(color, snakeMatrix);
    }

    private Color SimulateSharkVision(Color color)
    {
        float[,] sharkMatrix = {
            { 0.3f, 0.7f, 0.0f },    //monochromatic vision
            { 0.3f, 0.7f, 0.0f },
            { 0.3f, 0.7f, 0.0f }
        };
        return ConvertColours(color, sharkMatrix);
    }

    private Color SimulateCatVision(Color color)
    {
        float[,] catMatrix = {
            { 0.6f, 0.4f, 0.0f },    //red is perceived weakly
            { 0.0f, 0.6f, 0.4f },    //green perception
            { 0.2f, 0.8f, 0.0f }     //blue perception
        };
        return ConvertColours(color, catMatrix);
    }

    private Color SimulateDogVision(Color color)
    {
        float[,] dogMatrix = {
            { 0.5f, 0.5f, 0.0f },    //reduced red sensitivity
            { 0.3f, 0.7f, 0.0f },    //green perception
            { 0.2f, 0.6f, 0.2f }     //blue perception
        };
        return ConvertColours(color, dogMatrix);
    }

    //apply matrix to colour
    private Color ConvertColours(Color color, float[,] matrix)
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
