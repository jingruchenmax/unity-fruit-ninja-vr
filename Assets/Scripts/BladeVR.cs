using UnityEngine;

public class BladeVR : MonoBehaviour
{
    public float sliceForce = 5f;
    public float minSliceVelocity = 0.01f;

    private Collider sliceCollider;

    private Vector3 direction;
    public Vector3 Direction => direction;

    private bool slicing;
    public bool Slicing => slicing;

    Vector3 oldPosition;
    private void Awake()
    {
        sliceCollider = GetComponent<Collider>();
        StartSlice();
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        // StopSlice();
    }

    private void Update()
    {
        ContinueSlice();
    }

    private void StartSlice()
    {
        sliceCollider.enabled = true;
        oldPosition = transform.position;
    }

    private void StopSlice()
    {
        //slicing = false;
        //sliceCollider.enabled = false;
        //sliceTrail.enabled = false;
    }

    private void ContinueSlice()
    {
        direction = transform.position-oldPosition;
        oldPosition = transform.position;
        Debug.Log(Direction);
    }

}
