using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// https://docs.unity3d.com/Packages/com.unity.mathematics@1.2/api/Unity.Mathematics.html
using static Unity.Mathematics.noise;
using static Unity.Mathematics.math;

public class PlantWorld : MonoBehaviour
{
    public GameObject debug;
    public GameObject[] plants;
    public List<GameObject> plantInstances = new List<GameObject>();
    private float plantStartScaleMin = 1.0f;
    private float plantStartScaleMax = 2.4f;
    private Vector3 plantScaleDelta = new Vector3(0.1f, 0.1f, 0.1f);
    private float plantMaxScale = 3.0f;
    private float ScalingDistance = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        //initializePlantsOnGrid();
        initializePlantsOnSphere();
    }

    // Update is called once per frame
    void Update()
    {
        // Compute distance between player and the plant.
        scalePlants();
    }

    void FixedUpdate()
    {
        placePlant();
        scalePlants();
    }

    void initializePlantsOnSphere()
    {
        int nPlants = 500;
        float initialRandomRadius = 100.0f; // take in account size of planet.
        for (int i = 0; i < nPlants; i++)
        {
            Vector3 randomPointPosition = Random.onUnitSphere * initialRandomRadius;
            GameObject debugInst = Instantiate(debug, randomPointPosition, debug.transform.rotation);
            debugInst.transform.parent = transform;
            RaycastHit hit;
            Vector3 direction = Vector3.Normalize(transform.position - randomPointPosition);
            Ray ray = new Ray(randomPointPosition, direction);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.gameObject.name == "Planet") // use mask!
                {
                    instanceRandomPlant(hit.transform, hit.point, hit.normal);
                }
            }
        }
    }

    void placePlant()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.gameObject.name == "Planet") // use mask!
                {
                    instanceRandomPlant(hit.transform, hit.point, hit.normal);
                }
            }
        }
    }

    void instanceRandomPlant(Transform parent, Vector3 position, Vector3 normal)
    {
        // Use noise to instance plants.
        //int plantIdx = (int)Mathf.Floor(Random.Range(0, plants.Length)); // random.
        // https://forum.unity.com/threads/an-overview-of-noise-functions-in-the-unity-mathematics-package.1098193/
        // https://medium.com/@5argon/various-noise-functions-76327e056450
        // https://github.com/Unity-Technologies/Unity.Mathematics
        // https://blog.logrocket.com/making-procedural-noise-unity/
        // https://docs.unity3d.com/Packages/com.unity.mathematics@1.3/api/Unity.Mathematics.html
        Unity.Mathematics.float4 noiseArg = new Unity.Mathematics.float4(position.x, position.y, position.z, 0.0f);
        float noiseVal = (cnoise(noiseArg) + 1.0f) / 2.0f;
        Debug.Log(noiseVal);
        int plantIdx = (int)Mathf.Floor(noiseVal * plants.Length); 
        GameObject plant = plants[plantIdx];
        GameObject plantInst = Instantiate(plant, position, plant.transform.rotation);
        float scale = Random.Range(plantStartScaleMin, plantStartScaleMax);
        plantInst.transform.localScale = new Vector3(scale, scale, scale);
        // Use hit point normal to orient instance.
        // https://docs.unity3d.com/ScriptReference/Quaternion.LookRotation.html
        plantInst.transform.rotation = Quaternion.LookRotation(normal);
        // Make sure to transform all instances with object on which it is spawn.
        // https://docs.unity3d.com/ScriptReference/Transform-parent.html
        plantInst.transform.parent = parent;
    }

    void scalePlants()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Collider[] hitColliders = Physics.OverlapSphere(hit.point, ScalingDistance);
                foreach (Collider hitCollider in hitColliders)
                {
                    if (hitCollider.gameObject.name == "Planet")  // use mask!
                    {
                        continue;
                    }
                    hitCollider.transform.localScale += plantScaleDelta;
                    if (hitCollider.transform.localScale.x > plantMaxScale)
                    {
                        hitCollider.transform.localScale = new Vector3(plantMaxScale, plantMaxScale, plantMaxScale);
                    }
                }
            }
        }
    }

    // Depricated.
    void initializePlantsOnGrid()
    {
        float plantGridDist = 1.0f;
        int plantGridPoints = 10;
        Vector3 currPosition = new Vector3(0.0f, 0.0f, 0.0f);
        for (int i = -plantGridPoints; i < plantGridPoints; i++)
        {
            for (int j = -plantGridPoints; j < plantGridPoints; j++)
            {
                currPosition.x = i * plantGridDist;
                currPosition.z = j * plantGridDist;
                if (Random.value < 0.2)
                {
                    int plantIdx = (int)Mathf.Floor(Random.Range(0, plants.Length));
                    GameObject plant = plants[plantIdx];
                    GameObject plantInst = Instantiate(plant, currPosition, plant.transform.rotation);
                    float scale = Random.Range(0.3f, 1.0f);
                    plantInst.transform.localScale = new Vector3(scale, scale, scale);
                    plantInstances.Add(plantInst);
                }
            }
        }
    }
}
