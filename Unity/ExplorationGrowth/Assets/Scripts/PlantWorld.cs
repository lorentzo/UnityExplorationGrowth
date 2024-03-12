using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantWorld : MonoBehaviour
{
    public GameObject player;
    public GameObject[] plants;
    public List<GameObject> plantInstances = new List<GameObject>();
    float plantGridDist = 1.0f;
    int plantGridPoints = 10;
    private float plantMinScale = 0.5f;
    private float plantMaxScale = 5.0f;
    private float playerMinDistance = 0.1f;
    private float playerMaxDistance = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        initializePlants();
    }

    // Update is called once per frame
    void Update()
    {
        // Compute distance between player and the plant.
        scalePlants();
    }

    // Ini plants.
    void initializePlants()
    {
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

    void scalePlants()
    {
        foreach (GameObject plantInst in plantInstances)
        {
            float dist = Vector3.Distance(player.transform.position, plantInst.transform.position);
            if (dist > playerMaxDistance) continue;
            if (dist < playerMinDistance) dist = playerMinDistance;
            //if (dist > playerMaxDistance) dist = playerMaxDistance;
            float t = dist / playerMaxDistance;
            float scale = Mathf.Lerp(plantMinScale, plantMaxScale, 1.0f - t);
            plantInst.transform.localScale = new Vector3(scale, scale, scale);
        }
    }

    void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            //Debug.Log(hit.collider);
            int plantIdx = (int)Mathf.Floor(Random.Range(0, plants.Length));
            GameObject plant = plants[plantIdx];
            GameObject plantInst = Instantiate(plant, hit.point, plant.transform.rotation);
            float scale = Random.Range(0.3f, 1.0f);
            plantInst.transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
