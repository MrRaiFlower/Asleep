using UnityEngine;

public class ItemSpot : MonoBehaviour
{
    [SerializeField] private ItemSpawnSystem.SpotTypes type;

    [SerializeField] private GameObject model;
    [SerializeField] private GameObject prefab;

    [SerializeField] private bool randomizeRotation;

    [HideInInspector] public bool IsFree;

    private void Start()
    {
        ItemSpawnSystem.Instance.RegisterSpot(this, type);
        IsFree = true;
        Destroy(model);
        Spawn();
    }

    public void Spawn()
    {
        IsFree = false;
        Instantiate(prefab, transform.position, GetRotation(), transform);
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
