using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GravekeeperController : MonoBehaviour
{
    public static GravekeeperController Instance { get; set; }

    private Camera _mainCamera;

    [SerializeField] private GameObject inHandShovel;

    [SerializeField] private GameObject handReference;

    [SerializeField] private GameObject headReference;

    [SerializeField] private float characterSpeed = 4;

    [SerializeField] private Collider terrainCollider;

    [SerializeField] private GameObject shovelProjectile;

    [SerializeField] private GameObject weaponHandPosition;

    [SerializeField] private GameObject holePrefab;

    [SerializeField] private GameObject hpBar;

    [SerializeField] private TextMeshProUGUI hpLabel;

    [SerializeField] private GameObject hurtEffectPanel;


    private RectTransform _hpBarRectTransform;

    public float health = 100.0f;

    private Vector3 _cameraOffset;

    private RaycastHit _hit;

    private Ray _ray;

    private Animator _animator;

    // Flags

    private GameObject _corpseInRange;

    private GameObject _itemInRange;

    private GameObject _holeInRange;

    private GameObject _heldItem;

    private bool _digging;

    private bool _holdingCorpse;

    private bool _holdingShovel;

    private float _damageDelay = 0.0f;


    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _cameraOffset = _mainCamera.transform.position;
        _animator = gameObject.GetComponent<Animator>();
        _hpBarRectTransform = hpBar.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Perform look at mouse continously
        _ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        if (terrainCollider != null && terrainCollider.Raycast(_ray, out _hit, 100))
        {
            Vector3 newPoint = _hit.point;
            newPoint.y = transform.position.y; // Correct wrong rotation on X/Y axis.
            transform.LookAt(newPoint, Vector3.up);
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }


        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetBool("Run", true);
            SoundManager.Instance.PlaySound("Footsteps");
        }

        // If we press left click run.
        if (Input.GetMouseButton(0) && !_digging && (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") ||
                                                     _animator.GetCurrentAnimatorStateInfo(0).IsName("Run")))
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _hit.point, characterSpeed * Time.smoothDeltaTime);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _animator.SetBool("Run", false);
            SoundManager.Instance.StopSound("Footsteps");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            PerformPickupAnimationThenExecute(() =>
            {
                if (_itemInRange != null && _heldItem == null &&
                    (_itemInRange.CompareTag("PickableShovel") || _itemInRange.CompareTag("ProjectileShovel")))
                {
                    _holdingShovel = true;
                    _heldItem = Instantiate(inHandShovel, handReference.transform.position,
                        handReference.transform.rotation, handReference.transform);
                    Destroy(_itemInRange);
                }
            });
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_corpseInRange != null && _heldItem == null && _corpseInRange.CompareTag("Enemy") && !_holdingCorpse && !_corpseInRange.GetComponentInParent<Enemy>().buried)
            {
                PerformPickupAnimationThenExecute(() =>
                {
                    _heldItem = _corpseInRange.GetComponent<Enemy>().draggablePart;
                    _holdingCorpse = true;

                    SetCollisionsWithGameObject(_corpseInRange, false);
                });
            }
            else if (_holeInRange != null && _holdingCorpse && !_holeInRange.GetComponent<HoleScript>().HasCorpse())
            {
                PerformPickupAnimationThenExecute(() =>
                {
                    _corpseInRange.GetComponentInParent<Enemy>().buried = true;
                    _holdingCorpse = false;
                    SetCollisionsWithGameObject(_heldItem.GetComponentInParent<Enemy>().gameObject, true);
                    _holeInRange.GetComponentInChildren<HoleScript>()
                        .SetCorpse(_heldItem.GetComponentInParent<Enemy>().gameObject);
                    _heldItem.transform.position = _holeInRange.transform.position;
                    _heldItem = null;
                });
            }
            else if (_holdingCorpse)
            {
                SetCollisionsWithGameObject(_heldItem.GetComponentInParent<Enemy>().gameObject, true);

                _heldItem.GetComponentInParent<Enemy>().gameObject.transform.position =
                    transform.position + transform.forward * 2.0f + Vector3.up;
                _heldItem.transform.position = _heldItem.GetComponentInParent<Enemy>().gameObject.transform.position;
                _holdingCorpse = false;
                _heldItem = null;
            }
        }

        if (_heldItem != null && _holdingCorpse)
        {
            _heldItem.transform.position = weaponHandPosition.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!_digging && _holdingShovel)
            {
                _digging = true;
                _animator.SetBool("Dig", true);

                if (!_holeInRange)
                {
                    _holeInRange = Instantiate(holePrefab, transform.position + transform.forward * 2.0f,
                        Quaternion.identity);
                }
                else
                {
                    _holeInRange.GetComponent<HoleScript>().ResumeDigging();
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            StopDigging();
        }

        // Primitive camera zooming
        _mainCamera.fieldOfView -= Input.mouseScrollDelta.y * 2;
        _mainCamera.fieldOfView = Math.Min(Math.Max(10, _mainCamera.fieldOfView), 60);

        // Update camera position
        _mainCamera.transform.position = transform.position + _cameraOffset;


        // Debug
        if (Input.GetMouseButtonDown(1) && _heldItem != null && _holdingShovel)
        {
            SpawnProjectile();
        }

        _damageDelay += Time.deltaTime;
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
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponentInParent<Enemy>().knocked)
            {
                _corpseInRange = other.gameObject.GetComponentInParent<Enemy>().gameObject;
            }
        }
        else if (other.gameObject.CompareTag("Hole"))
        {
            _holeInRange = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PickableShovel"))
        {
            _itemInRange = null;
        }
        else if (other.gameObject.CompareTag("ProjectileShovel"))
        {
            _itemInRange = null;
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            if (other.gameObject.Equals(_corpseInRange))
            {
                _corpseInRange = null;
            }
        }
        else if (other.gameObject.CompareTag("Hole"))
        {
            _holeInRange = null;
        }
    }


    private void SetCollisionsWithGameObject(GameObject otherGO, bool state)
    {
        Collider[] ragdollColliders = otherGO.GetComponentsInChildren<Collider>();
        Collider ownCollider = GetComponent<Collider>();

        foreach (var ragdollCollider in ragdollColliders)
        {
            Physics.IgnoreCollision(ownCollider, ragdollCollider, !state);
        }
    }

    public void StopDigging()
    {
        if (_digging)
        {
            _animator.SetBool("Dig", false);
            _digging = false;
            if (_holeInRange != null)
            {
                _holeInRange.GetComponent<HoleScript>().PauseDigging();
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("DamageArea"))
        {
            if (_damageDelay / 2.0f > 1.0f)
            {
                _damageDelay = 0.0f;
                if (!other.gameObject.GetComponentInParent<Enemy>().knocked)
                {
                    DealDamage();
                }
            }
        }
    }

    private void SpawnProjectile()
    {
        _animator.SetTrigger("Throw");
        StartCoroutine(ActuallySpawnProjectile());
    }

    private void PerformPickupAnimationThenExecute(Action anyMethod)
    {
        _animator.SetTrigger("Pickup");
        StartCoroutine(PerformActionAfterAnimation(0.5f, anyMethod));
    }


    IEnumerator PerformActionAfterAnimation(float time, Action method)
    {
        yield return new WaitForSeconds(time);
        method();
    }

    IEnumerator ActuallySpawnProjectile()
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 projectileStartPos = transform.position + transform.forward * 1.1f;
        projectileStartPos.y = headReference.transform.position.y;
        GameObject projectile =
            Instantiate(shovelProjectile, handReference.transform.position, transform.rotation);
        Rigidbody projectileRB = projectile.GetComponentInChildren<Rigidbody>();
        projectileRB.AddForce(transform.forward * 30f, ForceMode.VelocityChange);
        Destroy(_heldItem);
        _holdingShovel = false;

        SoundManager.Instance.PlaySound("Throw");
    }

    public void ResetCollisionForEnemy(GameObject enemy)
    {
        SetCollisionsWithGameObject(enemy, true);
    }

    private bool _hurtEffectPlaying;

    private void DealDamage()
    {
        health -= 5.0f;
        StopCoroutine(PlayHurtEffect());
        StartCoroutine(PlayHurtEffect());
        _hpBarRectTransform.transform.localScale = new Vector3(health / 100.0f, 1.0f, 1.0f);
        hpLabel.text = health.ToString() + "%";
        SoundManager.Instance.PlaySound("Hit");
        if (health <= 0.0f)
        {
            SceneManager.LoadScene("GameOverScene", LoadSceneMode.Single);
        }
    }

    IEnumerator PlayHurtEffect()
    {
        Image imageRef = hurtEffectPanel.GetComponent<Image>();
        Color color = imageRef.color;

        float _duration = 0.2f;
        float _currentTime = 0.0f;

        color.a = 0.0f;
        while (_currentTime < _duration)
        {
            color.a = _currentTime / _duration * 0.4f;
            imageRef.color = color;
            _currentTime += Time.deltaTime;
            yield return null;
        }

        _currentTime = 0.0f;
        while (_currentTime < _duration)
        {
            color.a = (1 - (_currentTime / _duration)) * 0.4f;
            imageRef.color = color;
            _currentTime += Time.deltaTime;
            yield return null;
        }
    }
}