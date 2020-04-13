using System;
using UnityEngine;

public class GravekeeperController : MonoBehaviour
{

    private Camera _mainCamera;

    [SerializeField] private float characterSpeed = 4;

    [SerializeField]
    private Collider terrainCollider;

    private Vector3 _cameraOffset;

    private RaycastHit _hit;

    private Ray _ray;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _cameraOffset = _mainCamera.transform.position;
        _animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

      
        
        
        // Perform look at mouse continously
        _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (terrainCollider.Raycast(_ray, out _hit, 100))
        {
            transform.LookAt(_hit.point, Vector3.up);
        }


        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetBool("Run", true);
        }
        
        // If we press left click run.
        if (Input.GetMouseButton(0))
        {
            transform.position = Vector3.MoveTowards(transform.position, _hit.point, characterSpeed * Time.smoothDeltaTime);
        } else if (Input.GetMouseButtonUp(0))
        {
            _animator.SetBool("Run", false);
        }

        
        // Primitive camera zooming
        _mainCamera.fieldOfView -= Input.mouseScrollDelta.y * 2;
        _mainCamera.fieldOfView = Math.Min(Math.Max(10, _mainCamera.fieldOfView), 60);
        
        // Update camera position
        _mainCamera.transform.position = transform.position + _cameraOffset;
    }
}
