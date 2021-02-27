using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceElementScript : MonoBehaviour
{


    public Material seNoneMaterial;
    public Material seWaterMaterial;
    public Material seSaltMaterial;
    public Material seSalineMaterial;

    private IDictionary<string, Material> tagToMaterial =
        new Dictionary<string, Material>();
    private MeshRenderer meshR;
 

    // UNITY HOOKS

    void Awake()
    {
        this.tagToMaterial.Add("science-element-none", seNoneMaterial);
        this.tagToMaterial.Add("science-element-water", seWaterMaterial);
        this.tagToMaterial.Add("science-element-salt", seSaltMaterial);
        this.tagToMaterial.Add("science-element-saline", seSalineMaterial);
    }

    void Start() {
        this.meshR = GetComponent<MeshRenderer>();
    }

    void Update() {
        CheckMaterial();
    }

    void OnCollisionEnter(Collision collision) {
        if (
            this.gameObject.CompareTag("science-element-water") &&
            collision.gameObject.CompareTag("science-element-salt")
        ) {
            // turn water to saline and make salt disappear
            // Debug.Log("Water collided with salt!");
            this.gameObject.tag = "science-element-saline";
            collision.gameObject.SetActive(false);
        }
    }

    // IMPLEMENTATION METHODS

    private void CheckMaterial()
    {
        Material applyMat = tagToMaterial[this.gameObject.tag];
        this.meshR.material = applyMat;
    }


}