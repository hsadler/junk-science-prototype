using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceElementScript : MonoBehaviour
{

    
    // component caches
    private MeshFilter meshF;
    private MeshRenderer meshR;
    private ConstantForce constantF;

    // meshes
    public Mesh sphereMesh;
    public Mesh cubeMesh;
    private IDictionary<string, Mesh> tagToMesh = new Dictionary<string, Mesh>();

    // collider GOs
    public GameObject sphereColliderGO;
    public GameObject cubeColliderGO;
    private GameObject currColliderGO;
    private IDictionary<string, GameObject> tagToColliderGO = new Dictionary<string, GameObject>();

    // element scale
    private IDictionary<string, float> tagToScale = new Dictionary<string, float>();

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
    private IDictionary<string, Material> tagToMaterial = new Dictionary<string, Material>();

    // last temperature
    private float lastTemperature;
    // consts
    private const float GAS_THRESHOLD = 100f;
 

    // UNITY HOOKS

    void Awake() {
        // tag to material map
        this.tagToMaterial.Add("science-element-none", seNoneMaterial);
        this.tagToMaterial.Add("science-element-water", seWaterMaterial);
        this.tagToMaterial.Add("science-element-salt", seSaltMaterial);
        this.tagToMaterial.Add("science-element-saline", seSalineMaterial);
        this.tagToMaterial.Add("science-element-steam", seSteamMaterial);
        // tag to mesh map
        this.tagToMesh.Add("science-element-none", sphereMesh);
        this.tagToMesh.Add("science-element-water", sphereMesh);
        this.tagToMesh.Add("science-element-salt", cubeMesh);
        this.tagToMesh.Add("science-element-saline", sphereMesh);
        this.tagToMesh.Add("science-element-steam", sphereMesh);
        // tag to colliderGO map
        this.tagToColliderGO.Add("science-element-none", sphereColliderGO);
        this.tagToColliderGO.Add("science-element-water", sphereColliderGO);
        this.tagToColliderGO.Add("science-element-salt", cubeColliderGO);
        this.tagToColliderGO.Add("science-element-saline", sphereColliderGO);
        this.tagToColliderGO.Add("science-element-steam", sphereColliderGO);
        // tag to scale map
        this.tagToScale.Add("science-element-none", 1.5f);
        this.tagToScale.Add("science-element-water", 1.5f);
        this.tagToScale.Add("science-element-salt", 0.8f);
        this.tagToScale.Add("science-element-saline", 1.5f);
        this.tagToScale.Add("science-element-steam", 3f);
        this.lastTemperature = this.temperature;
    }

    void Start() {
        this.meshF = GetComponent<MeshFilter>();
        this.meshR = GetComponent<MeshRenderer>();
        this.constantF = GetComponent<ConstantForce>();
        Transform parent = transform.parent;
        transform.parent = null;
        transform.parent = parent;
        InvokeRepeating("CheckHeatChange", 0f, this.secondsPerHeat);
    }

    void Update() {
        CheckMaterial();
        CheckMesh();
        CheckCollider();
        CheckScale();
    }

    void OnCollisionEnter(Collision collision) {
        if (
            this.gameObject.CompareTag("science-element-water") &&
            collision.gameObject.CompareTag("science-element-salt")
        ) {
            // turn water to saline and make salt disappear
            this.ConvertToSaline();
            collision.gameObject.SetActive(false);
            LabSceneManager.instance.GiveScienceElementBackToPool(collision.gameObject);
        }
    }

    // IMPLEMENTATION METHODS

    private void CheckMaterial() {
        // TODO: optimize if needed 
        Material applyMat = tagToMaterial[this.gameObject.tag];
        this.meshR.material = applyMat;
    }

    private void CheckMesh() {
        // TODO: optimize if needed 
        Mesh applyMesh = tagToMesh[this.gameObject.tag];
        this.meshF.mesh = applyMesh;
    }

    private void CheckCollider() {
        GameObject colliderGO = tagToColliderGO[this.gameObject.tag];
        if(currColliderGO != colliderGO) {
            if(currColliderGO != null) {
                currColliderGO.SetActive(false);
            }
            colliderGO.SetActive(true);
            currColliderGO = colliderGO;
        }
    }

    private void CheckScale() {
        // TODO: optimize if needed 
        float scale = tagToScale[this.gameObject.tag];
        transform.localScale = Vector3.one * scale;
    }

    private void CheckHeatChange() {
        if (this.receivingHeat)
        {
            this.temperature += this.receivingHeatAmount;
        }
        // do water type boil changes
        if (this.lastTemperature < GAS_THRESHOLD && this.temperature >= GAS_THRESHOLD) {
            // handle water and saline differently
            if (this.gameObject.CompareTag("science-element-water"))
            {
                this.ConvertToSteam();
            }
            else if (this.gameObject.CompareTag("science-element-saline"))
            {
                // convert current element to steam
                this.ConvertToSteam();
                // also create a salt as a by-product
                var saltGO = LabSceneManager.instance.GetScienceElementFromPool();
                if (saltGO != null)
                {
                    saltGO.transform.position = new Vector3(
                        this.transform.position.x,
                        this.transform.position.y,
                        this.transform.position.z
                    );
                    saltGO.transform.rotation = Quaternion.identity;
                    saltGO.SetActive(true);
                    var seScript = saltGO.GetComponent<ScienceElementScript>();
                    seScript.ConvertToSalt();
                }
            }
        } else if(this.lastTemperature <= GAS_THRESHOLD && this.temperature < GAS_THRESHOLD) {
            if (this.gameObject.CompareTag("science-element-steam"))
            {
                this.ConvertToWater();
            }
        }
        this.lastTemperature = this.temperature;
    }

    private void ConvertToWater()
    {
        // Debug.Log("converting to water...");
        this.gameObject.tag = "science-element-water";
        this.constantF.force = new Vector3(0, 0, 0);
    }

    private void ConvertToSaline()
    {
        //Debug.Log("converting to saline...");
        this.gameObject.tag = "science-element-saline";
        this.constantF.force = new Vector3(0, 0, 0);
    }

    private void ConvertToSteam()
    {
        // Debug.Log("converting to steam...");
        this.gameObject.tag = "science-element-steam";
        float forceUp = Mathf.Abs(Physics.gravity.y) / 19f;
        this.constantF.force = new Vector3(0, forceUp, 0);
    }

    private void ConvertToSalt()
    {
        //Debug.Log("converting to salt...");
        this.gameObject.tag = "science-element-salt";
        this.constantF.force = new Vector3(0, 0, 0);
    }


}