using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemSpawnSystem : MonoBehaviour
{
    public static ItemSpawnSystem Instance;

    public enum SpotTypes
    {
        Batteries,
        Pills
    }

    [SerializeField] private ItemSpawnParameters _batteriesParameters;
    [SerializeField] private ItemSpawnParameters _pillsParameters;

    private List<ItemSpot> _batetriesPileSpots = new List<ItemSpot>();
    private List<ItemSpot> _pillsPileSpots = new List<ItemSpot>();

    private int _batteriesAmount;
    private int _pillsAmount;

    private float _batteriesInterval;
    private float _pillsInterval;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnBatteries());
        StartCoroutine(SpawnPills());
    }

    private IEnumerator SpawnBatteries()
    {
        _batteriesInterval = _batteriesParameters.timeInterval + Random.Range(-_batteriesParameters.dev, -_batteriesParameters.dev);
        yield return new WaitForSeconds(_batteriesInterval);

        TrySpawn(_batetriesPileSpots);

        StartCoroutine(SpawnBatteries());
    }

    private IEnumerator SpawnPills()
    {
        _pillsInterval = _pillsParameters.timeInterval + Random.Range(-_pillsParameters.dev, -_pillsParameters.dev);
        yield return new WaitForSeconds(_pillsInterval);

        TrySpawn(_pillsPileSpots);

        StartCoroutine(SpawnPills());
    }

    public void RegisterSpot(ItemSpot spot, SpotTypes spotType)
    {
        switch (spotType)
        {
            case SpotTypes.Batteries: _batetriesPileSpots.Add(spot); break;
            case SpotTypes.Pills: _pillsPileSpots.Add(spot); break;
        }
    }

    private void TrySpawn(List<ItemSpot> spots)
    {
        List<ItemSpot> freeSpots = spots.Where(spot => spot.IsFree == true).ToList();
        if (freeSpots.Count() == 0)
        {
            return;
        }
        freeSpots[UnityEngine.Random.Range(0, freeSpots.Count())].Spawn();
    }
}
