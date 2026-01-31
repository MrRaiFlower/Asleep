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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TimeSystem.OnSanityTick += Tick;
    }

    private void Tick()
    {
        float r = UnityEngine.Random.Range(0f, 100f);

        if (r < _batteriesParameters.spawnChance && _batteriesAmount < _batteriesParameters.maxAmount)
        {
            TrySpawn(_batetriesPileSpots);
        }
        if (r < _pillsParameters.spawnChance && _pillsAmount < _pillsParameters.maxAmount)
        {
            TrySpawn(_pillsPileSpots);
        }
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
