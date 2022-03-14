using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        //triggers new part setting
        if (other.CompareTag("Player"))
        {
            StartCoroutine(BodyController.instance.PlaceNewPart(gameObject)); 
            GetComponent<Collider>().enabled = false;
            
        }
    }
}
