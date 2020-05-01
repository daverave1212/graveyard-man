using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelPickable : MonoBehaviour
{
   [SerializeField] private GameObject hint;

   [SerializeField] private bool isProjectile = false;

   private void TriggerHint(bool state)
   {
      hint.SetActive(state);
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.gameObject.CompareTag("Player"))
      {
         if (isProjectile && GetComponent<Rigidbody>().velocity.sqrMagnitude > 7.0f)
         {
            return;
         }
         
         TriggerHint(true);
      }
   }

   private void OnTriggerExit(Collider other)
   {
      TriggerHint(false);
   }
}
