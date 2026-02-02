using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntruderB : MonoBehaviour, Intruder
{
    [SerializeField] private List<GameObject> _bodies = new List<GameObject>();
    [Space(16)]
    [SerializeField] private float _timeLimit;
    [Space(16)]
    [SerializeField] private int _minTimes;
    [SerializeField] private int _maxTimes;
    [Space(16)]
    [SerializeField] private float _minSanityDrain;
    [SerializeField] private float _maxSanityDrain;
    [Space(16)]
    [SerializeField] private float _minFlashlightDrain;
    [SerializeField] private float _maxFlashlightDrain;

    private LookDetectionBody _body;
    private int _previousBodyIndex;
    private int _bodyIndex;

    private bool _wasLit;

    private void Start()
    {
        foreach (GameObject body in _bodies)
        {
            body.SetActive(false);
        }

        SanitySystem.Instance.intruders.Add(this);
    }

    public void Activate()
    {
        Fade.Instance.DoFade(() => StartCoroutine(PhaseOne()));
    }

    private IEnumerator PhaseOne()
    {
        _previousBodyIndex = -1;
        ActivateNewBody();
        yield return new WaitUntil(() => _body.IsSeen());

        StartCoroutine(PhaseTwo());
    }

    private IEnumerator PhaseTwo()
    {
        int times = Random.Range(_minTimes, _maxTimes + 1);

        for (int i = 0; i < times; i++)
        {
            _wasLit = false;
            StartCoroutine(Wait());
            yield return new WaitUntil(() => _wasLit);
        }

        Fade.Instance.DoFade(() =>
        {
            _body.gameObject.SetActive(false);
            SanitySystem.Instance.isIntruderCurrentlyActive = false;
            SanitySystem.Instance.ChangeSanity(-Random.Range(_minSanityDrain, _maxSanityDrain));
        });
    }

    private void ActivateNewBody()
    {
        while (true)
        {
            _bodyIndex = Random.Range(0, _bodies.Count);
            if (_bodyIndex != _previousBodyIndex)
            {
                break;
            }
        }

        _previousBodyIndex = _bodyIndex;
        _body = _bodies[_bodyIndex].GetComponent<LookDetectionBody>();
        _body.gameObject.SetActive(true);
    }

    private IEnumerator Wait()
    {
        float t = _timeLimit;

        while (true)
        {
            if (_body.IsSeen() && FlashlightControl.Instance.isOn)
            {
                break;
            }
            else
            {
                t -= Time.deltaTime;
                if (t <= 0f)
                {
                    SanitySystem.Instance.GameOver();
                }
                yield return null;
            }
        }

        yield return new WaitForSeconds(1f);
        _wasLit = true;
        FlashlightControl.Instance.charge -= Random.Range(_minFlashlightDrain, _maxFlashlightDrain);
        _body.gameObject.SetActive(false);
        Fade.Instance.DoFade(ActivateNewBody);
    }
}
