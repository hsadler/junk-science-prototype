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


    // UNITY HOOKS

    void Awake()
    {
        this.tagToMaterial.Add("science-element-none", seNoneMaterial);
        this.tagToMaterial.Add("science-element-water", seWaterMaterial);
        this.tagToMaterial.Add("science-element-salt", seSaltMaterial);
        this.tagToMaterial.Add("science-element-saline", seSaltMaterial);
    }

    void Start() {}

    void Update() {
        CheckMaterial();
    }

    void OnCollisionEnter(Collision collision) {
        if (
            this.gameObject.CompareTag("science-element-water") &&
            collision.gameObject.CompareTag("science-element-salt")
        ) {
            Debug.Log("Water collided with salt!");
            this.gameObject.tag = "science-element-saline";
        }
    }

    // IMPLEMENTATION METHODS

    private void CheckMaterial()
    {
        
    }


}