using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceElementScript : MonoBehaviour
{


    // element temperature
    public float temperature = 0f;
    public bool receivingHeat = false;
    public float receivingHeatAmount = 0f;
    public float secondsPerHeat = 1f;

    // materials
    public Material seNoneMaterial;
    public Material seWaterMaterial;
    public Material seSaltMaterial;
    public Material seSalineMaterial;
    public Material seSteamMaterial;
    private IDictionary<string, Material> tagToMaterial =
        new Dictionary<string, Material>();
    private MeshRenderer meshR;

    // consts
    private const float BOIL_THRESHOLD = 100f;
 

    // UNITY HOOKS

    void Awake()
    {
        this.tagToMaterial.Add("science-element-none", seNoneMaterial);
        this.tagToMaterial.Add("science-element-water", seWaterMaterial);
        this.tagToMaterial.Add("science-element-salt", seSaltMaterial);
        this.tagToMaterial.Add("science-element-saline", seSalineMaterial);
        this.tagToMaterial.Add("science-element-steam", seSteamMaterial);
    }

    void Start() {
        this.meshR = GetComponent<MeshRenderer>();
        InvokeRepeating("CheckHeatChange", 0f, this.secondsPerHeat);
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
            //Debug.Log("Chemical reaction!!!!!!!!");
            this.ConvertToSaline();
            collision.gameObject.SetActive(false);
        }
    }

    // IMPLEMENTATION METHODS

    private void CheckMaterial() {
        Material applyMat = tagToMaterial[this.gameObject.tag];
        this.meshR.material = applyMat;
    }

    private void CheckHeatChange() {
        if (this.receivingHeat)
        {
            this.temperature += this.receivingHeatAmount;
        }
        // do water type boil changes
        if (this.temperature >= BOIL_THRESHOLD) {
            // handle water and saline differently
            if (this.gameObject.CompareTag("science-element-water"))
            {
                this.ConvertToSteam();
            }
            else if (this.gameObject.CompareTag("science-element-saline"))
            {
                this.ConvertToSteam();
                var saltGO = Instantiate(
                    this.gameObject,
                    new Vector3(
                        this.transform.position.x,
                        this.transform.position.y,
                        this.transform.position.z
                    ),
                    Quaternion.identity
                );
                saltGO.GetComponent<ScienceElementScript>().ConvertToSalt();
            }
        }
    }

    private void ConvertToWater()
    {
        //Debug.Log("converting to water...");
        this.gameObject.tag = "science-element-water";
        GetComponent<ConstantForce>().force = new Vector3(0, 0, 0);
    }

    private void ConvertToSaline()
    {
        //Debug.Log("converting to saline...");
        this.gameObject.tag = "science-element-saline";
        GetComponent<ConstantForce>().force = new Vector3(0, 0, 0);
    }

    private void ConvertToSteam()
    {
        //Debug.Log("converting to steam...");
        this.gameObject.tag = "science-element-steam";
        GetComponent<ConstantForce>().force = new Vector3(0, 1.7f, 0);
    }

    private void ConvertToSalt()
    {
        //Debug.Log("converting to salt...");
        this.gameObject.tag = "science-element-salt";
        GetComponent<ConstantForce>().force = new Vector3(0, 0, 0);
    }


}