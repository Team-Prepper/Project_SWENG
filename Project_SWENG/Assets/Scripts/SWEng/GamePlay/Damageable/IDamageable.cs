using UnityEngine;
using SWEng.GamePlay;

public interface IDamageable
{
    public Transform transform { get; }
    public IStatus Status { get; }
    
}