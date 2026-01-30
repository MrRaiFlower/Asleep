using UnityEngine;

public class ItemSpot : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private GameObject prefab;

    [SerializeField] private bool randomizeRotation;
    [SerializeField] private bool spawnAtStart;

    private void Start()
    {
        model.SetActive(false);
        
        if (spawnAtStart)
        {
            SpawnPile();
        }
    }

    private void SpawnPile()
    {
        Instantiate(prefab, transform.position, GetRotation());
        
    }

    private Quaternion GetRotation()
    {
        if (randomizeRotation)
        {
            return Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        }
        else
        {
            return Quaternion.Euler(Vector3.zero);
        }
    }
}
