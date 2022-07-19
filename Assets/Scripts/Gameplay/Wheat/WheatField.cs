using System;
using System.Collections;
using System.Linq;
using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class WheatField : MonoBehaviour
{
    [SerializeField] private Transform _startGrowPosition;
    [SerializeField] private Transform _endGrowPosition;
    [SerializeField] private float _timeGrowingUp;
    [SerializeField] private float _timeFadeOutCutPart;
    [SerializeField] private AnimationCurve _growCurve;
    [SerializeField] private AnimationCurve _fadeOutCurve;

    private void Awake()
    {
        var wheats = GetComponentsInChildren<Wheat>();
        foreach (var wheat in wheats)
        {
            wheat.EventSlice += OnSlice;
        }
    }

    private void OnSlice(Wheat wheat, GameObject cutPart)
    {
        wheat.transform.position = new Vector3(wheat.transform.position.x, _startGrowPosition.position.y,
            wheat.transform.position.z);
        cutPart.GetComponent<Renderer>().material
            .DOFloat(0f, "_Alpha", _timeFadeOutCutPart)
            .SetEase(_fadeOutCurve)
            .OnComplete(() => Destroy(cutPart));
        StartCoroutine(GrowingProcess(wheat));
    }

    private IEnumerator GrowingProcess(Wheat wheat)
    {
        float time = 0;
        var position = wheat.transform.position;
        Vector3 startPoint = new Vector3(position.x, _startGrowPosition.position.y,
            position.z);
        Vector3 endPoint = new Vector3(position.x, _endGrowPosition.position.y,
            position.z);
        
        while (time < _timeGrowingUp)
        {
            wheat.transform.position = Vector3.Lerp(startPoint, endPoint,
                _growCurve.Evaluate(time / _timeGrowingUp));
            time += Time.deltaTime;
            yield return null;
        }

        wheat.EndGrow();
    }
}