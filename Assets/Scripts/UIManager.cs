using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject eye;
    [SerializeField] private GameObject cell;
    [SerializeField] private GameObject spectrum;
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private List<GameObject> txtObjs;
    private int currentIndex = 0;
    private static bool hasInitialized = false; //so introduction is playable only once


    void Start()
    {
        //initialise starting ui
        if (!hasInitialized)
        {
            hasInitialized = true;
            mainPanel.SetActive(true);
            title.SetActive(true);
            InitializeObjects();
    
        }
    }

    //set active the first text object, otherwise make sure its not active
    private void InitializeObjects()
    {
        for (int i = 0; i < txtObjs.Count; i++)
        {
            txtObjs[i].SetActive(i == currentIndex);
        }
    }

//activate next txt obj - activate or deactivate other things depending on current active
    public void ActivateNext()
    {
        if (txtObjs.Count > currentIndex )
        {
            txtObjs[currentIndex].SetActive(false);
            currentIndex = (currentIndex + 1) % txtObjs.Count;

            txtObjs[currentIndex].SetActive(true);


            switch(currentIndex)
            {
                case 3:
                    title.SetActive(false);
                    eye.SetActive(true);
                    break;
                case 6:
                    eye.SetActive(false);
                    cell.SetActive(true);
                    break;
                case 9:
                    cell.SetActive(false);
                    spectrum.SetActive(true);
                    break;
                case 10:
                    txtObjs[11].SetActive(true);
                    break;
                case 11:
                    mainPanel.SetActive(false);
                    break;
                default:
                    break;
            }
        }
        
    }


    
}