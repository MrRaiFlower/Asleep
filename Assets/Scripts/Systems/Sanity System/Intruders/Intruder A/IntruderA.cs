using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntruderA : MonoBehaviour, Intruder
{
    [SerializeField] private List<IntruderALocation> _locations = new List<IntruderALocation>();
    [Space(16)]
    [SerializeField] private float _phaseTwoTime;
    [SerializeField] private float _phaseThreeTime;
    [Space(16)]
    [SerializeField] private float _minSanityDrain;
    [SerializeField] private float _maxSanityDrain;

    private IntruderALocation _location;

    private void Start()
    {
        foreach (IntruderALocation location in _locations)
        {
            location.roomBody.SetActive(false);
            location.hidingBody.SetActive(false);
        }

        SanitySystem.Instance.intruders.Add(this);
    }

    public void Activate()
    {
        Fade.Instance.DoFade(() => StartCoroutine(PhaseOne()));
    }

    private IEnumerator PhaseOne()
    {
        _location = _locations[Random.Range(0, _locations.Count)];
        SpawnBody();
        LookDetectionBody body = _location.roomBody.GetComponent<LookDetectionBody>();

        yield return new WaitUntil(() => body.IsSeen());

        StartCoroutine(PhaseTwo());
    }

    private IEnumerator PhaseTwo()
    {
        yield return new WaitForSeconds(_phaseTwoTime);

        if (IsPlayerHidden())
        {
            Fade.Instance.DoFade(() => StartCoroutine(PhaseThree()));
        }
        else
        {
            SanitySystem.Instance.GameOver();
        }
    }

    private IEnumerator PhaseThree()
    {
        MoveBody();

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

        Fade.Instance.DoFade(Leave);
    }

    private void Leave()
    {
        RemoveBody();
        SanitySystem.Instance.isIntruderCurrentlyActive = false;
        SanitySystem.Instance.ChangeSanity(-Random.Range(_minSanityDrain, _maxSanityDrain));
    }

    private bool IsPlayerHidden()
    {
        return _location.hidingSpot.GetComponent<HidingSpot>().HidesPlayer();
    }

    private void SpawnBody()
    {
        _location.roomBody.SetActive(true);
    }

    private void MoveBody()
    {
        _location.roomBody.SetActive(false);
        _location.hidingBody.SetActive(true);
    }

    private void RemoveBody()
    {
        _location.hidingBody.SetActive(false);
    }
}
