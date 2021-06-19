using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScienceElementDiscoveryDisplayScript : MonoBehaviour
{


    public GameObject textDisplayGO;
    private TextMeshProUGUI textDisplay;

    private IDictionary<string, bool> tagToUnlockStatus = new Dictionary<string, bool>();
    private IEnumerable waitThenClearDisplay;


    // UNITY HOOKS

    void Start() {
        this.textDisplay = textDisplayGO.GetComponent<TextMeshProUGUI>();
        this.textDisplay.text = "";
        LabSceneManager.instance.scienceElementDiscoveredEvent.AddListener(ReceiveElementDiscoveredEvent);
    }

    void Update() {}

    // IMPLEMENTATION METHODS

    private void ReceiveElementDiscoveredEvent(string scienceElementTag) {
        if(!this.tagToUnlockStatus.ContainsKey(scienceElementTag)) {
            string seDisplayName = LabSceneManager.instance.scienceElementData.tagToDisplayName[scienceElementTag];
            this.textDisplay.text = seDisplayName + " discovered";
            this.tagToUnlockStatus.Add(scienceElementTag, true);
            StartCoroutine(WaitThenClearDisplay());
        }
    }

    private IEnumerator WaitThenClearDisplay() {
        yield return new WaitForSeconds(Constants.DISCOVERY_DISPLAY_DURATION);
        this.textDisplay.text = "";
    }


}
