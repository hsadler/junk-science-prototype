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
    public float temperature;
    public bool isReceivingHeat;
    public float receivingHeatAmount;

    // mesh per science element
    private IDictionary<string, Mesh> tagToMesh = new Dictionary<string, Mesh>();
    public Mesh sphereMesh;
    public Mesh cubeMesh;

    // collider GOs per science element
    private IDictionary<string, GameObject> tagToColliderGO = new Dictionary<string, GameObject>();
    public GameObject sphereColliderGO;
    public GameObject stickySphereColliderGO;
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
        this.InitializeTemperatureProperties();
        this.meshF = GetComponent<MeshFilter>();
        this.meshR = GetComponent<MeshRenderer>();
        this.constantF = GetComponent<ConstantForce>();
    }

    void Start()
    {
        Transform parent = transform.parent;
        transform.parent = null;
        transform.parent = parent;
        InvokeRepeating("CheckHeatChange", 0f, Constants.SECONDS_PER_HEAT);
    }

    void Update() { }

    void OnEnable()
    {
        this.ProcActivated();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (this.gameObject.tag)
        {
            case Constants.SE_WATER_TAG:
                this.WaterCollisionHandler(collision.gameObject);
                break;
            case Constants.SE_EARTH_TAG:
                this.EarthCollisionHandler(collision.gameObject);
                break;
            case Constants.SE_LAVA_TAG:
                this.LavaCollisionHandler(collision.gameObject);
                break;
            default:
                break;
        }
    }

    // INTERFACE METHODS

    public void InitializeTemperatureProperties()
    {
        this.temperature = 0f;
        this.isReceivingHeat = false;
        this.receivingHeatAmount = 0f;
    }

    public void SetInitialTemperature(float temperature)
    {
        this.temperature = temperature;
    }

    public string GetDisplayInfo()
    {
        int displayTemperature = (int)Mathf.RoundToInt(this.temperature + 60);
        return
            LabSceneManager.instance.scienceElementData.tagToDisplayName[this.gameObject.tag] +
            "\ntemperature: " + displayTemperature;
    }

    // IMPLEMENTATION METHODS

    // collision handlers
    private void WaterCollisionHandler(GameObject collisionOtherGO)
    {
        // water + salt = saline
        if (collisionOtherGO.CompareTag(Constants.SE_SALT_TAG))
        {
            this.ConvertElement(Constants.SE_SALINE_TAG);
            LabSceneManager.instance.GiveScienceElementBackToPool(collisionOtherGO);
        }
    }
    private void EarthCollisionHandler(GameObject collisionOtherGO)
    {
        // water + earth = 70% mud, 20% stone, 10% ore
        // saline + earth = (70% mud, 20% stone, 10% ore) & salt
        if (collisionOtherGO.CompareTag(Constants.SE_WATER_TAG) || collisionOtherGO.CompareTag(Constants.SE_SALINE_TAG))
        {
            float randNum = Random.Range(0f, 1f);
            if (randNum < 0.7f)
            {
                this.ConvertElement(Constants.SE_MUD_TAG);
            }
            else if (randNum < 0.9f)
            {
                this.ConvertElement(Constants.SE_STONE_TAG);
            }
            else
            {
                this.ConvertElement(Constants.SE_ORE_TAG);
            }
            LabSceneManager.instance.GiveScienceElementBackToPool(collisionOtherGO);
            if (collisionOtherGO.CompareTag(Constants.SE_SALINE_TAG))
            {
                this.CreateByProduct(Constants.SE_SALT_TAG, this.transform.position, false, this.temperature);
            }
        }
    }
    private void LavaCollisionHandler(GameObject collisionOtherGO)
    {
        // lava + water = stone & steam
        // lava + saline = stone & steam & salt
        if (collisionOtherGO.CompareTag(Constants.SE_WATER_TAG) || collisionOtherGO.CompareTag(Constants.SE_SALINE_TAG))
        {
            this.ConvertElement(Constants.SE_STONE_TAG);
            LabSceneManager.instance.GiveScienceElementBackToPool(collisionOtherGO);
            this.CreateByProduct(Constants.SE_STEAM_TAG, this.transform.position, true, Constants.WATER_BOILING_POINT);
            if (collisionOtherGO.CompareTag(Constants.SE_SALINE_TAG))
            {
                this.CreateByProduct(Constants.SE_SALT_TAG, this.transform.position, false, this.temperature);
            }
        }
    }

    private void CheckHeatChange()
    {
        // set new temperature
        float newTemp = 0f;
        if (this.isReceivingHeat)
        {
            newTemp = this.temperature + this.receivingHeatAmount;
        }
        else
        {
            float heatToAdd = 0f;
            if (this.temperature > Constants.INITIAL_SCIENCE_ELEMENT_TEMPERATURE)
            {
                heatToAdd = -0.01f;
            }
            else if (this.temperature < Constants.INITIAL_SCIENCE_ELEMENT_TEMPERATURE)
            {
                heatToAdd = 0.01f;
            }
            newTemp = this.temperature + heatToAdd;
        }
        if (newTemp >= Constants.MIN_TEMPERATURE && newTemp <= Constants.MAX_TEMPERATURE)
        {
            float tempChange = newTemp - this.temperature;
            float adjustedTempChange = Random.Range(0, (tempChange * 2));
            this.temperature += adjustedTempChange;
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
            case Constants.SE_EARTH_TAG:
                this.EarthHeatChangeHandler();
                break;
            case Constants.SE_LAVA_TAG:
                this.LavaHeatChangeHandler();
                break;
            case Constants.SE_MUD_TAG:
                this.MudHeatChangeHandler();
                break;
            case Constants.SE_CLAY_TAG:
                this.ClayHeatChangeHandler();
                break;
            case Constants.SE_ORE_TAG:
                this.OreHeatChangeHandler();
                break;
            case Constants.SE_MOLTEN_METAL_TAG:
                this.MoltenMetalHeatChangeHandler();
                break;
            case Constants.SE_METAL_TAG:
                this.MetalHeatChangeHandler();
                break;
            default:
                break;
        }
    }

    // heat change handlers
    private void WaterHeatChangeHandler()
    {
        // boil water = steam
        if (this.temperature > Constants.WATER_BOILING_POINT)
        {
            this.ConvertElement(Constants.SE_STEAM_TAG, true);
        }
    }
    private void SalineHeatChangeHandler()
    {
        // boil saline = steam & salt
        if (this.temperature > Constants.WATER_BOILING_POINT)
        {
            this.ConvertElement(Constants.SE_STEAM_TAG, true);
            this.CreateByProduct(Constants.SE_SALT_TAG, this.transform.position, false, this.temperature);
        }
    }
    private void SteamHeatChangeHandler()
    {
        // cool steam = water
        if (this.temperature <= Constants.WATER_BOILING_POINT)
        {
            this.ConvertElement(Constants.SE_WATER_TAG);
        }
    }
    private void EarthHeatChangeHandler()
    {
        // melt earth = lava
        if (this.temperature > Constants.EARTH_MELTING_POINT)
        {
            this.ConvertElement(Constants.SE_LAVA_TAG);
        }
    }
    private void LavaHeatChangeHandler()
    {
        // cool lava = stone
        if (this.temperature <= Constants.EARTH_MELTING_POINT)
        {
            this.ConvertElement(Constants.SE_STONE_TAG);
        }
    }
    private void MudHeatChangeHandler()
    {
        // cool mud = clay
        if (this.temperature <= Constants.MUD_FREEZING_POINT)
        {
            this.ConvertElement(Constants.SE_CLAY_TAG);
        }
        // heat mud = earth
        if (this.temperature > Constants.MUD_DRYING_POINT)
        {
            this.ConvertElement(Constants.SE_EARTH_TAG);
        }
    }
    private void ClayHeatChangeHandler()
    {
        // heat clay = brick & steam
        if (this.temperature > Constants.CLAY_BOILING_POINT)
        {
            this.ConvertElement(Constants.SE_BRICK_TAG);
            this.CreateByProduct(Constants.SE_STEAM_TAG, this.transform.position, true, this.temperature);
        }
    }
    private void OreHeatChangeHandler()
    {
        // heat ore = slag & molten metal
        if (this.temperature > Constants.METAL_MELTING_POINT)
        {
            this.ConvertElement(Constants.SE_MOLTEN_METAL_TAG);
            this.CreateByProduct(Constants.SE_SLAG_TAG, this.transform.position, false, this.temperature);
        }
    }
    private void MoltenMetalHeatChangeHandler()
    {
        // cool molten metal = metal
        if (this.temperature <= Constants.METAL_MELTING_POINT)
        {
            this.ConvertElement(Constants.SE_METAL_TAG);
        }
    }
    private void MetalHeatChangeHandler()
    {
        // heat metal = molten metal
        if (this.temperature > Constants.METAL_MELTING_POINT)
        {
            this.ConvertElement(Constants.SE_MOLTEN_METAL_TAG);
        }
    }

    private GameObject CreateByProduct(string seTag, Vector3 position, bool isGas = false, float temperature = 0f)
    {
        var byProdGO = LabSceneManager.instance.GetScienceElementFromPool();
        if (byProdGO != null)
        {
            byProdGO.transform.position = new Vector3(
                position.x,
                position.y,
                position.z
            );
            byProdGO.transform.rotation = Quaternion.identity;
            byProdGO.SetActive(true);
            var seScript = byProdGO.GetComponent<ScienceElementScript>();
            seScript.ConvertElement(seTag, isGas);
            seScript.SetInitialTemperature(temperature);
        }
        return byProdGO;
    }

    private void ConvertElement(string seTag, bool isGas = false)
    {
        this.gameObject.tag = seTag;
        float forceUp = isGas ? Mathf.Abs(Physics.gravity.y) / 19f : 0f;
        this.constantF.force = new Vector3(0, forceUp, 0);
        this.ProcActivated();
    }

    private void ProcActivated()
    {
        this.CheckMaterial();
        this.CheckMesh();
        this.CheckCollider();
        this.CheckScale();
        this.ProcDiscovered();
    }

    private void ProcDiscovered()
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
        this.tagToMesh.Add(Constants.SE_SLAG_TAG, cubeMesh);
        this.tagToMesh.Add(Constants.SE_MOLTEN_METAL_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_METAL_TAG, cubeMesh);
        this.tagToMesh.Add(Constants.SE_LAVA_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_CLAY_TAG, sphereMesh);
        this.tagToMesh.Add(Constants.SE_BRICK_TAG, cubeMesh);

        // tag to colliderGO map
        this.tagToColliderGO.Add(Constants.SE_NONE_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_WATER_TAG, stickySphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_SALT_TAG, cubeColliderGO);
        this.tagToColliderGO.Add(Constants.SE_SALINE_TAG, stickySphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_STEAM_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_EARTH_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_MUD_TAG, stickySphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_STONE_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_ORE_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_SLAG_TAG, cubeColliderGO);
        this.tagToColliderGO.Add(Constants.SE_MOLTEN_METAL_TAG, stickySphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_METAL_TAG, cubeColliderGO);
        this.tagToColliderGO.Add(Constants.SE_LAVA_TAG, stickySphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_CLAY_TAG, sphereColliderGO);
        this.tagToColliderGO.Add(Constants.SE_BRICK_TAG, cubeColliderGO);

        // tag to scale map
        this.tagToScale.Add(Constants.SE_NONE_TAG, 1.7f);
        this.tagToScale.Add(Constants.SE_WATER_TAG, 2f);
        this.tagToScale.Add(Constants.SE_SALT_TAG, 1.2f);
        this.tagToScale.Add(Constants.SE_SALINE_TAG, 2.4f);
        this.tagToScale.Add(Constants.SE_STEAM_TAG, 4.4f);
        this.tagToScale.Add(Constants.SE_EARTH_TAG, 2f);
        this.tagToScale.Add(Constants.SE_MUD_TAG, 2.6f);
        this.tagToScale.Add(Constants.SE_STONE_TAG, 2.6f);
        this.tagToScale.Add(Constants.SE_ORE_TAG, 2f);
        this.tagToScale.Add(Constants.SE_SLAG_TAG, 1.4f);
        this.tagToScale.Add(Constants.SE_MOLTEN_METAL_TAG, 2.4f);
        this.tagToScale.Add(Constants.SE_METAL_TAG, 2.4f);
        this.tagToScale.Add(Constants.SE_LAVA_TAG, 2.2f);
        this.tagToScale.Add(Constants.SE_CLAY_TAG, 2f);
        this.tagToScale.Add(Constants.SE_BRICK_TAG, 2f);
    }


}