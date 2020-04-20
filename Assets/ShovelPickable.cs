using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelPickable : MonoBehaviour
{
   [SerializeField] private GameObject hint;

   private void TriggerHint(bool state)
   {
      hint.SetActive(state);
   }

   private void OnTriggerEnter(Collider other)
   {
      Debug.Log("Shovel");
      if (other.gameObject.CompareTag("Player"))
      {
         TriggerHint(true);
      }
   }

   private void OnTriggerExit(Collider other)
   {
      TriggerHint(false);
   }
}
