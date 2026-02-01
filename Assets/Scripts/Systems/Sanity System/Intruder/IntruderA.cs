using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntruderA : MonoBehaviour, Intruder
{
    [SerializeField] private int _locations;
    [Space(16)]
    [SerializeField] private List<GameObject> _roomBodies;
    [SerializeField] private List<GameObject> _hidingBodies;
    [SerializeField] private List<HidingSpot> _hidingSpots;
    [Space(16)]
    [SerializeField] private float _phaseTwoTime;
    [SerializeField] private float _phaseThreeTime;
    [Space(16)]
    [SerializeField] private float _minSanityDrain;
    [SerializeField] private float _maxSanityDrain;

    private int _location;

    private void Start()
    {
        for (int i = 0; i < _locations; i++)
        {
            _roomBodies[i].SetActive(false);
            _hidingBodies[i].SetActive(false);
        }

        // Activate();
    }

    public void Activate()
    {
        IntruderController.Instance.isActive = true;

        _location = Random.Range(0, _roomBodies.Count);
        SpawnBody();

        StartCoroutine(PhaseOne());
    }

    private IEnumerator PhaseOne()
    {
        IntruderBody body = _roomBodies[_location].GetComponent<IntruderBody>();

        yield return new WaitUntil(() => body.IsSeen()); Debug.Log("Hide!");

        StartCoroutine(PhaseTwo());
    }

    private IEnumerator PhaseTwo()
    {
        yield return new WaitForSeconds(_phaseTwoTime);

        if (IsPlayerHidden())
        {
            StartCoroutine(PhaseThree()); Debug.Log("Silent!");
        }
        else
        {
            SanitySystem.Instance.GameOver();
        }
    }

    private IEnumerator PhaseThree()
    {
        Fade.Instance.DoFade(MoveBody);

        float t = 0;
        while (t < _phaseThreeTime)
        {
            t += Time.deltaTime;

            if (!IsPlayerHidden())
            {
                SanitySystem.Instance.GameOver();
            }
            
            yield return null;
        }

        Fade.Instance.DoFade(Leave); Debug.Log("Safe!");
    }

    private void Leave()
    {
        RemoveBody();
        SanitySystem.Instance.ReduceSanity(UnityEngine.Random.Range(_minSanityDrain, _maxSanityDrain));
        IntruderController.Instance.isActive = false;
    }

    private bool IsPlayerHidden()
    {
        return _hidingSpots[_location].HidesPlayer();
    }

    private void SpawnBody()
    {
        _roomBodies[_location].SetActive(true);
    }

    private void MoveBody()
    {
        _roomBodies[_location].SetActive(false);
        _hidingBodies[_location].SetActive(true);
    }

    private void RemoveBody()
    {
        _hidingBodies[_location].SetActive(false);
    }
}
