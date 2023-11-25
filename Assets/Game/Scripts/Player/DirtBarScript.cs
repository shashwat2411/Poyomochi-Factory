using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DirtBarScript : MonoBehaviour
{
    private Image DirtBar;
    private Image DarkDirtBar;
    public float CurrentDirtValue;
    public GameObject DarkBar;
    public GameObject BarShadow;
    public GameObject DarkBarShadow;
    private DirtValue DirtValueScriptReference;
    private void Awake()
    {
        DirtValueScriptReference = FindObjectOfType<DirtValue>();
    }

    private void Start()
    {
        DirtBar = GetComponent<Image>();
        DarkDirtBar = DarkBar.GetComponent<Image>();
    }
    private void FixedUpdate()
    {
        if (DirtValueScriptReference.gameObject.GetComponent<PlayerMovement>().inWater == false)
        {
            CurrentDirtValue = DirtValueScriptReference.Dirt;
            DarkDirtBar.fillAmount = CurrentDirtValue / DirtValueScriptReference.MaxDirt;

            if (DarkDirtBar.fillAmount > DirtBar.fillAmount)
            {
                if ((DirtBar.fillAmount + Time.deltaTime * 0.6f) >= DarkDirtBar.fillAmount)
                {
                    DarkDirtBar.fillAmount = DirtBar.fillAmount;
                    DirtValueScriptReference.previousDirtValue = CurrentDirtValue;
                }
                else { DirtBar.fillAmount += Time.deltaTime * 0.6f; }
            }
        }
        else if (DirtValueScriptReference.gameObject.GetComponent<PlayerMovement>().inWater == true)
        {
            CurrentDirtValue = DirtValueScriptReference.Dirt;
            DirtBar.fillAmount = CurrentDirtValue / DirtValueScriptReference.MaxDirt;
            float Difference = DirtValueScriptReference.previousDirtValue - CurrentDirtValue;
            float linear = 2f * Difference / DirtValueScriptReference.MaxDirt;

            if (DirtBar.fillAmount < DarkDirtBar.fillAmount)
            {
                if ((DarkDirtBar.fillAmount - Time.deltaTime * linear) <= DirtBar.fillAmount) 
                {
                    DirtBar.fillAmount = DarkDirtBar.fillAmount;
                    DirtValueScriptReference.previousDirtValue = CurrentDirtValue;
                }
                else { DarkDirtBar.fillAmount -= Time.deltaTime * linear; }
            }

            if (DirtValueScriptReference.Dirt <= 0f)
            {
                DirtBar.fillAmount = 0f;
                DarkDirtBar.fillAmount = 0f;
                DirtValueScriptReference.previousDirtValue = 0f;
            }
        }

        BarShadow.GetComponent<Image>().fillAmount = GetComponent<Image>().fillAmount;
        BarShadow.GetComponent<Image>().fillAmount = DarkBar.GetComponent<Image>().fillAmount;
    }
}
