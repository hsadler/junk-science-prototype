using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScienceElementDiscoveryDisplayScript : MonoBehaviour
{


    public GameObject textDisplayGO;
    private TextMeshProUGUI textDisplay;

    private bool displayIsInUse = false;
    private Queue<string> seTagQueue = new Queue<string>();
    private IDictionary<string, bool> tagToUnlockStatus = new Dictionary<string, bool>();


    // UNITY HOOKS

    void Start() {
        this.textDisplay = textDisplayGO.GetComponent<TextMeshProUGUI>();
        this.textDisplay.text = "";
        LabSceneManager.instance.scienceElementDiscoveredEvent.AddListener(ReceiveElementDiscoveredEvent);
    }

    void Update() {
        CheckAndDisplay();
    }

    // IMPLEMENTATION METHODS

    private void ReceiveElementDiscoveredEvent(string scienceElementTag) {
        if(!this.tagToUnlockStatus.ContainsKey(scienceElementTag)) {
            this.tagToUnlockStatus.Add(scienceElementTag, true);   
            QueueSETag(scienceElementTag);
        }
    }

    private void QueueSETag(string seTag) {
        seTagQueue.Enqueue(seTag);
    }

    private void CheckAndDisplay() {
        if(!this.displayIsInUse && this.seTagQueue.Count > 0) {
            string seTag = this.seTagQueue.Dequeue();
            string seDisplayName = LabSceneManager.instance.scienceElementData.tagToDisplayName[seTag];
            this.textDisplay.text = seDisplayName + " discovered";
            this.displayIsInUse = true;
            StartCoroutine(WaitThenClearDisplay());
        }
    }

    private IEnumerator WaitThenClearDisplay() {
        yield return new WaitForSeconds(Constants.DISCOVERY_DISPLAY_DURATION);
        this.textDisplay.text = "";
        this.displayIsInUse = false;
    }


}
