using UnityEngine;

public class FixedRotate : MonoBehaviour
{

    private Quaternion _default;
    
    void Start()
    {
        _default = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = _default;
    }
}
