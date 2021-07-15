using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScienceElementInfoScript : MonoBehaviour
{


    public GameObject seInfoTextGO;
    private TextMeshProUGUI seInfoTextDisplay;


    // UNITY HOOKS

    void Start()
    {
        this.seInfoTextDisplay = this.seInfoTextGO.GetComponent<TextMeshProUGUI>();
    }

    void Update() { }

    // INTERFACE METHODS

    public void SetSEInfo(string info)
    {
        this.seInfoTextDisplay.text = info;
    }


}
