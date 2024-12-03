using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YeetColourAway : MonoBehaviour
{
    [SerializeField] List<GameObject> trees = new List<GameObject>();
    [SerializeField] List<GameObject> stones = new List<GameObject>();
    [SerializeField] List<GameObject> mushrooms = new List<GameObject>();
    
    [SerializeField] bool lessBlue = false;
    [SerializeField] bool noBlue = false;
    [SerializeField] bool lessRed = false;
    [SerializeField] bool noRed = false;
    [SerializeField] bool lessGreen = false;
    [SerializeField] bool noGreen = false;

    Color treeColour;


    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject tree in GameObject.FindGameObjectsWithTag("Tree"))
        {
            trees.Add(tree);
            treeColour = tree.GetComponent<ColourMaterialCreator>().objectColour;
            tree.GetComponent<ColourMaterialCreator>().ApplyMaterial(ColourStealer(treeColour));
        }
        
        foreach (GameObject stone in GameObject.FindGameObjectsWithTag("Stone"))
        {
            stones.Add(stone);
        }
        
        foreach (GameObject mushroom in GameObject.FindGameObjectsWithTag("Mushroom"))
        {
            mushrooms.Add(mushroom);
        }

    }
    
    private Color ColourStealer(Color colour)
    {
        string colourHex = ColorUtility.ToHtmlStringRGB(colour);
        char[] hexArray = colourHex.ToCharArray();
        string R = hexArray[0].ToString() + hexArray[1].ToString();
        string G = hexArray[2].ToString() + hexArray[3].ToString();
        string B = hexArray[4].ToString() + hexArray[5].ToString();
        

        
        if(lessBlue)
        {
            B = "ff";
            if(ColorUtility.TryParseHtmlString(("#" + R + G + B), out colour))
            {
                return colour;
            }
        }
    


        return colour;
    }
    // Update is called once per frame
    void Update()
    {

    }
}
