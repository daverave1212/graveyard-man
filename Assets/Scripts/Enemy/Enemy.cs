using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject hint;

    [SerializeField] private SphereCollider triggerCollider;

    [SerializeField] private CapsuleCollider damageAreaCollider;

    [SerializeField] private float timeToRevive = 15.0f;

    [SerializeField] private Collider projectileHitbox;

    private Collider[] _colliders;
    private Rigidbody[] _rbs;

    public GameObject draggablePart;

    public bool knocked = false;

    public bool buried = false;

    private float originalHeight;

    private AudioSource _hitSound;

    private void Start()
    {
        _colliders = transform.GetChild(0).gameObject.GetComponentsInChildren<Collider>();
        _rbs = transform.GetChild(0).gameObject.GetComponentsInChildren<Rigidbody>();

        originalHeight = draggablePart.transform.position.y;

        _hitSound = GetComponent<AudioSource>();

        // Make trigger sphere and projectile collider to ignore inner colliders...
        foreach (var childCollider in _colliders)
        {
            Physics.IgnoreCollision(triggerCollider, childCollider, true);
            Physics.IgnoreCollision(damageAreaCollider, childCollider, true);
            Physics.IgnoreCollision(projectileHitbox, childCollider, true);
        }

        SetRagdollState(false);
        projectileHitbox.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("ProjectileShovel"))
        {
            Rigidbody otherRb = collision.gameObject.GetComponent<Rigidbody>();
            otherRb.freezeRotation = false;
            float initialVelocityMagnitude = otherRb.velocity.sqrMagnitude;

            otherRb.AddForce(otherRb.velocity * -1.1f, ForceMode.Impulse);

            if (Mathf.Abs(initialVelocityMagnitude) > 8.0f)
            {
                EnemyManager.Instance.DestroyEnemy(gameObject);
                TriggerDeathAnim();
            }
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player") && knocked && !buried)
        {
            hint.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            hint.SetActive(false);
        }
    }

    public void SetRagdollState(bool state)
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = state;
        }

        foreach (var rb in _rbs)
        {
            rb.isKinematic = !state;
        }
    }

    public void TriggerDeathAnim()
    {
        knocked = true;
        GetComponentInChildren<Animator>().enabled = false;
        SetRagdollState(true);
        _hitSound.Play();
        // StartCoroutine(Revive()); We don't do dis no more, too wonky.
    }

    IEnumerator Revive()
    {
        yield return new WaitForSeconds(5.0f);
        if (buried)
        {
            yield break;
        }
        SetRagdollState(false);
        draggablePart.transform.rotation = Quaternion.identity;
        draggablePart.transform.position = new Vector3(draggablePart.transform.position.x, originalHeight, draggablePart.transform.position.z);
        GetComponentInChildren<Animator>().enabled = true;
        EnemyManager.Instance.ReAddEnemy(gameObject);
        GravekeeperController.Instance.ResetCollisionForEnemy(gameObject);
        knocked = false;
    }
}