using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using TMPro;

//use post processing to simulate blurriness
public class YeetDetail : MonoBehaviour
{
    private PostProcessVolume ppv;
    private DepthOfField dof;
    private TMP_Text fun_fact;
    
    private void Start()
    {
        GameObject textObj = GameObject.FindGameObjectWithTag("Text");
        fun_fact = textObj.GetComponent<TMP_Text>();
        fun_fact.text = "Blurred vision is pretty common! It's the reason so many people wear glasses or contacts.";
    }

    //nearsightedness
    public void SetMyopia()
    {
        fun_fact.text = "Also known as near-sightedness, Myopia causes distant objects to appear blurry due to the eyeball being too long.";
        ppv = GetComponent<PostProcessVolume>();
        ppv.profile.TryGetSettings(out dof);
        dof.focusDistance.value = 2f;
        dof.aperture.value = 2.8f;
        dof.focalLength.value = 50f;
    }

    //farsightedness
    public void SetHyperopia()
    {
        fun_fact.text = "Also known as far-sightedness, Hyperopia makes nearby objects appear blurry because the eyeball is too short.";
        ppv = GetComponent<PostProcessVolume>();
        ppv.profile.TryGetSettings(out dof);
        dof.focusDistance.value = 15f;
        dof.aperture.value = 2.8f;
        dof.focalLength.value = 50f;
    }

    public void SetCompleteBlurryVision()
    {
        fun_fact.text = "The most common cause for blurred vision is the shape of the eye preventing light from focusing on the retina.";
        ppv = GetComponent<PostProcessVolume>();
        ppv.profile.TryGetSettings(out dof);
        dof.focusDistance.value = 0.1f;
        dof.aperture.value = 1.2f;
        dof.focalLength.value = 50f;
    }

    public void Reset()
    {
        fun_fact.text = "Blurred vision is pretty common! It's the reason so many people wear glasses or contacts.";
        ppv = GetComponent<PostProcessVolume>();
        ppv.profile.TryGetSettings(out dof);
        dof.focusDistance.value = 0.1f;
        dof.aperture.value = 0.1f;
        dof.focalLength.value = 1.0f;
    }


}
