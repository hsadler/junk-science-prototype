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

    void Awake()
    {
        // tag to material map
        this.tagToMaterial.Add(Constants.SE_NONE_TAG, seNoneMaterial);
        this.tagToMaterial.Add(Constants.SE_WATER_TAG, seWaterMaterial);
        this.tagToMaterial.Add(Constants.SE_SALT_TAG, seSaltMaterial);
        this.tagToMaterial.Add(Constants.SE_SALINE_TAG, seSalineMaterial);
        this.tagToMaterial.Add(Constants.SE_STEAM_TAG, seSteamMaterial);
        // tag to mesh map
        this.tagToMesh.Add(Constants.SE_NONE_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_WATER_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_SALT_TAG, cubeMesh);
        this.tagToMesh.Add(Constants.SE_SALINE_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_STEAM_TAG, sphereMesh);
        // tag to colliderGO map
        this.tagToColliderGO.Add(Constants.SE_NONE_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_WATER_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_SALT_TAG, cubeColliderGO);
        this.tagToColliderGO.Add(Constants.SE_SALINE_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_STEAM_TAG, sphereColliderGO);
        // tag to scale map
        this.tagToScale.Add(Constants.SE_NONE_TAG, 1.7f);
        this.tagToScale.Add(Constants.SE_WATER_TAG, 1.7f);
        this.tagToScale.Add(Constants.SE_SALT_TAG, 1f);
        this.tagToScale.Add(Constants.SE_SALINE_TAG, 1.7f);
        this.tagToScale.Add(Constants.SE_STEAM_TAG, 3f);

        this.lastTemperature = this.temperature;
    }

    void Start()
    {
        this.meshF = GetComponent<MeshFilter>();
        this.meshR = GetComponent<MeshRenderer>();
        this.constantF = GetComponent<ConstantForce>();
        Transform parent = transform.parent;
        transform.parent = null;
        transform.parent = parent;
        InvokeRepeating("CheckHeatChange", 0f, this.secondsPerHeat);
    }

    void Update()
    {
        CheckMaterial();
        CheckMesh();
        CheckCollider();
        CheckScale();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (
            this.gameObject.CompareTag(Constants.SE_WATER_TAG) &&
            collision.gameObject.CompareTag(Constants.SE_SALT_TAG)
        )
        {
            // turn water to saline and make salt disappear
            this.ConvertToSaline();
            collision.gameObject.SetActive(false);
            LabSceneManager.instance.GiveScienceElementBackToPool(collision.gameObject);
        }
    }

    void OnEnable()
    {
        this.procDiscovered();
    }

    // IMPLEMENTATION METHODS

    private void CheckMaterial()
    {
        Material applyMat = tagToMaterial[this.gameObject.tag];
        if (this.meshR != applyMat)
        {
            this.meshR.material = applyMat;
        }
    }

    private void CheckMesh()
    {
        Mesh applyMesh = tagToMesh[this.gameObject.tag];
        if (this.meshF != applyMesh)
        {
            this.meshF.mesh = applyMesh;
        }
    }

    private void CheckCollider()
    {
        GameObject colliderGO = tagToColliderGO[this.gameObject.tag];
        if (currColliderGO != colliderGO)
        {
            if (currColliderGO != null)
            {
                currColliderGO.SetActive(false);
            }
            colliderGO.SetActive(true);
            currColliderGO = colliderGO;
        }
    }

    private void CheckScale()
    {
        float scale = tagToScale[this.gameObject.tag];
        Vector3 newScale = Vector3.one * scale;
        if (transform.localScale != newScale)
        {
            transform.localScale = newScale;
        }
    }

    private void CheckHeatChange()
    {
        if (this.receivingHeat)
        {
            this.temperature += this.receivingHeatAmount;
        }
        // do water type boil changes
        if (this.lastTemperature < GAS_THRESHOLD && this.temperature >= GAS_THRESHOLD)
        {
            // handle water and saline differently
            if (this.gameObject.CompareTag(Constants.SE_WATER_TAG))
            {
                this.ConvertToSteam();
            }
            else if (this.gameObject.CompareTag(Constants.SE_SALINE_TAG))
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
        }
        else if (this.lastTemperature <= GAS_THRESHOLD && this.temperature < GAS_THRESHOLD)
        {
            if (this.gameObject.CompareTag(Constants.SE_STEAM_TAG))
            {
                this.ConvertToWater();
            }
        }
        this.lastTemperature = this.temperature;
    }

    private void ConvertToWater()
    {
        // Debug.Log("converting to water...");
        this.gameObject.tag = Constants.SE_WATER_TAG;
        this.constantF.force = new Vector3(0, 0, 0);
        procDiscovered();
    }

    private void ConvertToSalt()
    {
        //Debug.Log("converting to salt...");
        this.gameObject.tag = Constants.SE_SALT_TAG;
        this.constantF.force = new Vector3(0, 0, 0);
        procDiscovered();
    }

    private void ConvertToSaline()
    {
        //Debug.Log("converting to saline...");
        this.gameObject.tag = Constants.SE_SALINE_TAG;
        this.constantF.force = new Vector3(0, 0, 0);
        procDiscovered();
    }

    private void ConvertToSteam()
    {
        // Debug.Log("converting to steam...");
        this.gameObject.tag = Constants.SE_STEAM_TAG;
        float forceUp = Mathf.Abs(Physics.gravity.y) / 19f;
        this.constantF.force = new Vector3(0, forceUp, 0);
        procDiscovered();
    }

    private void procDiscovered()
    {
        if (this.gameObject.tag != Constants.SE_NONE_TAG)
        {
            LabSceneManager.instance.scienceElementDiscoveredEvent.Invoke(this.gameObject.tag);
        }
    }


}