using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScienceElementDiscoveryDisplayScript : MonoBehaviour
{


    public GameObject textDisplayGO;
    private TextMeshProUGUI textDisplay;


    // UNITY HOOKS

    void Start() {
        this.textDisplay = textDisplayGO.GetComponent<TextMeshProUGUI>();
        this.textDisplay.text = "";
    }

    void Update() {}


}
