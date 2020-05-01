using System;
using UnityEngine;

public class GravekeeperController : MonoBehaviour
{
    private Camera _mainCamera;

    [SerializeField] private GameObject inHandShovel;

    [SerializeField] private GameObject handReference;

    [SerializeField] private float characterSpeed = 4;

    [SerializeField] private Collider terrainCollider;

    [SerializeField] private GameObject shovelProjectile;

    [SerializeField] private GameObject weaponHandPosition;

    [SerializeField] private GameObject holePrefab;

    [SerializeField] private PlaneTriangleGetter planeTriangler;

    private Vector3 _cameraOffset;

    private RaycastHit _hit;

    private Ray _ray;

    private Animator _animator;

    private GameObject _itemInRange;

    private GameObject _heldItem;

    private bool _heldCorpse;

    private GameObject _corpseInRange;

    private bool _digging = false;

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
        if (terrainCollider != null && terrainCollider.Raycast(_ray, out _hit, 100))
        {
            transform.LookAt(_hit.point, Vector3.up);
        }


        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetBool("Run", true);
        }

        // If we press left click run.
        if (Input.GetMouseButton(0) && !_digging)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _hit.point, characterSpeed * Time.smoothDeltaTime);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _animator.SetBool("Run", false);
        }

        if (Input.GetKey(KeyCode.E))
        {
            if (_itemInRange != null && _heldItem == null)
            {
                _heldItem = Instantiate(inHandShovel, handReference.transform.position,
                    handReference.transform.rotation, handReference.transform);
                Destroy(_itemInRange);
            }
        }

        if (Input.GetKey(KeyCode.Q))
        {
            if (_corpseInRange != null && _heldItem == null)
            {
                _heldItem = _corpseInRange.GetComponent<Enemy>().draggablePart;
                _heldCorpse = true;

                // Assure we ignore collision between player and ragdoll...
                Collider[] ragdollColliders = _corpseInRange.GetComponentsInChildren<Collider>();
                Collider ownCollider = GetComponent<Collider>();

                foreach (var ragdollCollider in ragdollColliders)
                {
                    Physics.IgnoreCollision(ownCollider, ragdollCollider, true);
                }

                // _corpseInRange.transform.parent = weaponHandPosition.transform;
                // _corpseInRange.transform.localPosition = Vector3.zero;
            }
        }

        if (_heldItem && _heldCorpse)
        {
            _heldItem.transform.position = weaponHandPosition.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_digging && _heldItem != null)
            {
                _animator.SetBool("Dig", true);
                _digging = true;
                Instantiate(holePrefab, transform.position + transform.forward * 2.0f, Quaternion.identity);
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (_digging)
            {
                _animator.SetBool("Dig", false);
                _digging = false;
            }
        }

        // Primitive camera zooming
        _mainCamera.fieldOfView -= Input.mouseScrollDelta.y * 2;
        _mainCamera.fieldOfView = Math.Min(Math.Max(10, _mainCamera.fieldOfView), 60);

        // Update camera position
        _mainCamera.transform.position = transform.position + _cameraOffset;


        // Debug
        if (Input.GetMouseButtonDown(1) && _heldItem != null)
        {
            GameObject projectile =
                Instantiate(shovelProjectile, weaponHandPosition.transform.position, transform.rotation);
            projectile.GetComponentInChildren<Rigidbody>().AddForce(transform.forward * 30f, ForceMode.Impulse);
            Destroy(_heldItem);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickableShovel"))
        {
            _itemInRange = other.gameObject;
        }
        else if (other.gameObject.CompareTag("ProjectileShovel"))
        {
            if (other.gameObject.GetComponent<Rigidbody>().velocity.sqrMagnitude < 7.0f)
            {
                _itemInRange = other.gameObject;
            }
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            _corpseInRange = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Temp
        _itemInRange = null;
    }
}