// using System.Collections;
// using System.Collections.Generic;
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
        LabSceneManager.instance.scienceElementDiscoveredEvent.AddListener(ReceiveElementDiscoveredEvent);
    }

    void Update() {}

    // IMPLEMENTATION METHODS

    private void ReceiveElementDiscoveredEvent(string scienceElementTag) {
        Debug.Log("Science element discovered with tag: " + scienceElementTag);
    }


}
