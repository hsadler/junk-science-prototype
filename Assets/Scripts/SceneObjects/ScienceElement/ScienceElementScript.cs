using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienceElementScript : MonoBehaviour
{


    // component caches
    private MeshFilter meshF;
    private MeshRenderer meshR;
    private ConstantForce constantF;

    // science element temperature
    public float temperature = 0f;
    private float lastTemperature;
    public bool receivingHeat = false;
    public float receivingHeatAmount = 0f;
    public float secondsPerHeat = 1f;

    // mesh per science element
    private IDictionary<string, Mesh> tagToMesh = new Dictionary<string, Mesh>();
    public Mesh sphereMesh;
    public Mesh cubeMesh;

    // collider GOs per science element
    private IDictionary<string, GameObject> tagToColliderGO = new Dictionary<string, GameObject>();
    public GameObject sphereColliderGO;
    public GameObject cubeColliderGO;
    private GameObject currColliderGO;

    // size scale per science element
    private IDictionary<string, float> tagToScale = new Dictionary<string, float>();

    // materials per science element
    private IDictionary<string, Material> tagToMaterial = new Dictionary<string, Material>();
    public Material seNoneMaterial;
    public Material seWaterMaterial;
    public Material seSaltMaterial;
    public Material seSalineMaterial;
    public Material seSteamMaterial;
    public Material seEarthMaterial;
    public Material seMudMaterial;
    public Material seStoneMaterial;
    public Material seOreMaterial;
    public Material seSlagMaterial;
    public Material seMoltenMetalMaterial;
    public Material seMetalMaterial;
    public Material seLavaMaterial;
    public Material seClayMaterial;
    public Material seBrickMaterial;


    // UNITY HOOKS

    void Awake()
    {
        this.SetScienceElementMappings();
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
    void OnEnable()
    {
        this.procDiscovered();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (this.gameObject.tag)
        {
            case Constants.SE_WATER_TAG:
                this.WaterCollisionHandler(collision.gameObject);
                break;
            default:
                break;
        }
    }

    // IMPLEMENTATION METHODS

    // collision handlers
    private void WaterCollisionHandler(GameObject collisionOtherGO)
    {
        if (collisionOtherGO.CompareTag(Constants.SE_SALT_TAG))
        {
            // turn water to saline and make salt disappear
            this.ConvertElement(Constants.SE_SALINE_TAG);
            collisionOtherGO.SetActive(false);
            LabSceneManager.instance.GiveScienceElementBackToPool(collisionOtherGO);
        }
    }

    private void CheckHeatChange()
    {
        // set new temperature
        if (this.receivingHeat)
        {
            this.temperature += this.receivingHeatAmount;
        }
        // run heat change handler
        switch (this.gameObject.tag)
        {
            case Constants.SE_WATER_TAG:
                this.WaterHeatChangeHandler();
                break;
            case Constants.SE_SALINE_TAG:
                this.SalineHeatChangeHandler();
                break;
            case Constants.SE_STEAM_TAG:
                this.SteamHeatChangeHandler();
                break;
            default:
                break;
        }
        // set last temperature for comparison checks
        this.lastTemperature = this.temperature;
    }

    // heat change handlers
    private void WaterHeatChangeHandler()
    {
        if (this.lastTemperature < Constants.GAS_THRESHOLD && this.temperature >= Constants.GAS_THRESHOLD)
        {
            this.ConvertElement(Constants.SE_STEAM_TAG, true);
        }
    }
    private void SalineHeatChangeHandler()
    {
        if (this.lastTemperature < Constants.GAS_THRESHOLD && this.temperature >= Constants.GAS_THRESHOLD)
        {
            // convert current element to steam
            this.ConvertElement(Constants.SE_STEAM_TAG, true);
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
                seScript.ConvertElement(Constants.SE_SALT_TAG);
            }
        }
    }
    private void SteamHeatChangeHandler()
    {
        if (this.lastTemperature >= Constants.GAS_THRESHOLD && this.temperature < Constants.GAS_THRESHOLD)
        {
            this.ConvertElement(Constants.SE_WATER_TAG);
        }
    }

    private void ConvertElement(string seTag, bool isGas = false)
    {
        this.gameObject.tag = seTag;
        float forceUp = isGas ? Mathf.Abs(Physics.gravity.y) / 19f : 0f;
        this.constantF.force = new Vector3(0, forceUp, 0);
        this.procDiscovered();
    }

    private void procDiscovered()
    {
        if (this.gameObject.tag != Constants.SE_NONE_TAG)
        {
            LabSceneManager.instance.scienceElementDiscoveredEvent.Invoke(this.gameObject.tag);
        }
    }

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

    private void SetScienceElementMappings()
    {
        // tag to material map
        this.tagToMaterial.Add(Constants.SE_NONE_TAG, seNoneMaterial);
        this.tagToMaterial.Add(Constants.SE_WATER_TAG, seWaterMaterial);
        this.tagToMaterial.Add(Constants.SE_SALT_TAG, seSaltMaterial);
        this.tagToMaterial.Add(Constants.SE_SALINE_TAG, seSalineMaterial);
        this.tagToMaterial.Add(Constants.SE_STEAM_TAG, seSteamMaterial);
        this.tagToMaterial.Add(Constants.SE_EARTH_TAG, seEarthMaterial);
        this.tagToMaterial.Add(Constants.SE_MUD_TAG, seMudMaterial);
        this.tagToMaterial.Add(Constants.SE_STONE_TAG, seStoneMaterial);
        this.tagToMaterial.Add(Constants.SE_ORE_TAG, seOreMaterial);
        this.tagToMaterial.Add(Constants.SE_SLAG_TAG, seSlagMaterial);
        this.tagToMaterial.Add(Constants.SE_MOLTEN_METAL_TAG, seMoltenMetalMaterial);
        this.tagToMaterial.Add(Constants.SE_METAL_TAG, seMetalMaterial);
        this.tagToMaterial.Add(Constants.SE_LAVA_TAG, seLavaMaterial);
        this.tagToMaterial.Add(Constants.SE_CLAY_TAG, seClayMaterial);
        this.tagToMaterial.Add(Constants.SE_BRICK_TAG, seBrickMaterial);

        // tag to mesh map
        this.tagToMesh.Add(Constants.SE_NONE_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_WATER_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_SALT_TAG, cubeMesh);
        this.tagToMesh.Add(Constants.SE_SALINE_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_STEAM_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_EARTH_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_MUD_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_STONE_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_ORE_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_SLAG_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_MOLTEN_METAL_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_METAL_TAG, cubeMesh);
        this.tagToMesh.Add(Constants.SE_LAVA_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_CLAY_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_BRICK_TAG, cubeMesh);

        // tag to colliderGO map
        this.tagToColliderGO.Add(Constants.SE_NONE_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_WATER_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_SALT_TAG, cubeColliderGO);
        this.tagToColliderGO.Add(Constants.SE_SALINE_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_STEAM_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_EARTH_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_MUD_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_STONE_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_ORE_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_SLAG_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_MOLTEN_METAL_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_METAL_TAG, cubeColliderGO);
        this.tagToColliderGO.Add(Constants.SE_LAVA_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_CLAY_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_BRICK_TAG, cubeColliderGO);

        // tag to scale map
        this.tagToScale.Add(Constants.SE_NONE_TAG, 1.7f);
        this.tagToScale.Add(Constants.SE_WATER_TAG, 1.7f);
        this.tagToScale.Add(Constants.SE_SALT_TAG, 1f);
        this.tagToScale.Add(Constants.SE_SALINE_TAG, 1.7f);
        this.tagToScale.Add(Constants.SE_STEAM_TAG, 3f);
        this.tagToScale.Add(Constants.SE_EARTH_TAG, 1.7f);
        this.tagToScale.Add(Constants.SE_MUD_TAG, 2f);
        this.tagToScale.Add(Constants.SE_STONE_TAG, 1.5f);
        this.tagToScale.Add(Constants.SE_ORE_TAG, 2f);
        this.tagToScale.Add(Constants.SE_SLAG_TAG, 1.5f);
        this.tagToScale.Add(Constants.SE_MOLTEN_METAL_TAG, 1.7f);
        this.tagToScale.Add(Constants.SE_METAL_TAG, 1.7f);
        this.tagToScale.Add(Constants.SE_LAVA_TAG, 1.7f);
        this.tagToScale.Add(Constants.SE_CLAY_TAG, 2f);
        this.tagToScale.Add(Constants.SE_BRICK_TAG, 2f);
    }


}