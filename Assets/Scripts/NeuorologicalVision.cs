using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NeuorologicalVision : MonoBehaviour
{
    public GameObject hallucinationPrefab;
    List<GameObject> hallucinationObjs = new List<GameObject>();
    private GameObject[] hallucinationSpawnPoints;

    public GameObject replacementPrefab;
    public GameObject targetObject;
    public float replacementDistance = 5f;

    private bool isReplaced = false;
    private GameObject replacementInstance;
    private TMP_Text fun_fact;

    void Start()
    {
        GameObject textObj = GameObject.FindGameObjectWithTag("Text");
        fun_fact = textObj.GetComponent<TMP_Text>();
        fun_fact.text = "As the brain interprets electrical signals from the eyes, it can change what we perceive if it mixes those signals up.";
    }

    //spawn hallucinated objects in locations from list + give them a random colour
    public void SimulateHallucinations()
    {
        fun_fact.text = "Visually, hallucinations involve seeing things that aren't there. These can vary from random shapes to people who aren't there";
        hallucinationSpawnPoints = GameObject.FindGameObjectsWithTag("Spawn");
        foreach (GameObject spawnPoint in hallucinationSpawnPoints)
        {
            GameObject hallucinationInstance = Instantiate(hallucinationPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
            hallucinationObjs.Add(hallucinationInstance);

            ColourMaterialCreator colourScript = hallucinationInstance.GetComponent<ColourMaterialCreator>();
            if (colourScript != null)
            {
                colourScript.objectColour = GetRandomColor();
                colourScript.ApplyMaterial(colourScript.objectColour); 
            }
            else
            {
                Debug.LogWarning("ColourMaterialCreator script not found on the hallucination prefab.");
            }
        }
    }

    //initialise agnosia 
    public void SimulateAgnosia()
    {
        fun_fact.text ="Agnosia is the inability to recognize objects usually it only affects one sense, so if the person is able to touch it they can recognise the object.";
        if (targetObject != null && replacementPrefab != null && !isReplaced)
        {
            replacementInstance = Instantiate(replacementPrefab, new Vector3(5.58f, 8.72f, 18.29f), targetObject.transform.rotation);
            targetObject.SetActive(false);
            isReplaced = true;
        }
    }

    //change object when the user gets close to it
    void Update()
    {
        if (isReplaced && replacementInstance != null && targetObject != null)
        {
            float distance = Vector3.Distance(replacementInstance.transform.position, Camera.main.transform.position);
            if (distance <= replacementDistance)
            {
                replacementInstance.SetActive(false);
                targetObject.SetActive(true);
                isReplaced = false;
            }   
        }
    }

    //change things back - kinda
    public void Reset()
    {
        foreach(GameObject obj in hallucinationObjs)
        {
            Destroy(obj);
        }
        Destroy(replacementInstance);

    }

    //make random color
    private Color GetRandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }
}
