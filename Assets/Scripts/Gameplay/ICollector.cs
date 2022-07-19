using UnityEngine;

public interface ICollector
{
    Transform TransformCollector { get; }
    
    bool TryCollect();
}