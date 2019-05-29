
#pragma warning disable 0618

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class Fragile : MonoBehaviour {
    public float CrackOverVelocity = 1f;
    public int PointsForCracking;
	public Transform fragments;  
	public float waitForRemoveCollider = 1.0f;      //Delay before removing collider (negative/zero = never)
	public float waitForRemoveRigid = 0.0f;        //Delay before removing rigidbody (negative/zero = never)
	public float waitForDestroy = 0.0f;             //Delay before removing objects (negative/zero = never)
	public float explosiveForce = 20.0f;           //How much random force applied to each fragment
	public float durability = 5.0f;                 //How much physical force the object can handle before it breaks
	public ParticleSystem breakParticles;
	Transform fragmentd;                            //Stores the fragmented object after break
	bool broken;                                    //Determines if the object has been broken or not 
	Transform frags;

    private GameState gameState;

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("collision relative speed: " + collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude > CrackOverVelocity)
        {
            Crash();
        }
    }

    private void Crash()
    {

		triggerBreak();
		Destroy(this.gameObject);
        gameState.Score += PointsForCracking;
    }

	public void triggerBreak() {
		Destroy(transform.Find("object").gameObject);
		Destroy(transform.GetComponent<Collider>());
		Destroy(transform.GetComponent<Rigidbody>());
		StartCoroutine(breakObject());
	}

	// breaks object
	public IEnumerator breakObject() {
		if (!broken) {
			if (this.GetComponent<AudioSource>() != null) {
				GetComponent<AudioSource>().Play();
			}
			broken = true;
			if (breakParticles != null) {
				// adds particle system to stage
				ParticleSystem ps = (ParticleSystem)Instantiate(breakParticles,transform.position, transform.rotation);
				// destroys particle system after duration of particle system
				Destroy(ps.gameObject, ps.duration);
			}
			// adds fragments to stage (!memo:consider adding as disabled on start for improved performance > mem)
			fragmentd = (Transform)Instantiate(fragments, transform.position, transform.rotation);
			// set size of fragments
			fragmentd.localScale = transform.localScale;
			frags = fragmentd.Find("fragments");
			foreach (Transform child in frags) {
				Rigidbody cr = child.GetComponent<Rigidbody>();
				cr.AddForce(UnityEngine.Random.Range(-explosiveForce, explosiveForce), UnityEngine.Random.Range(-explosiveForce, explosiveForce), UnityEngine.Random.Range(-explosiveForce, explosiveForce));
				cr.AddTorque(UnityEngine.Random.Range(-explosiveForce, explosiveForce), UnityEngine.Random.Range(-explosiveForce, explosiveForce), UnityEngine.Random.Range(-explosiveForce, explosiveForce));
			}
			StartCoroutine(removeColliders());
			StartCoroutine(removeRigids());
			// destroys fragments after "waitForDestroy" delay
			if (waitForDestroy > 0) {
				foreach (Transform child in transform) {
					child.gameObject.SetActive(false);
				}
				yield return new WaitForSeconds(waitForDestroy);
				GameObject.Destroy(fragmentd.gameObject);
				GameObject.Destroy(transform.gameObject);
				// destroys gameobject
			} else if (waitForDestroy <= 0) {
				foreach (Transform child in transform) {
					child.gameObject.SetActive(false);
				}
				yield return new WaitForSeconds(1.0f);
				GameObject.Destroy(transform.gameObject);
			}
		}
	}

	// removes rigidbodies from fragments after "waitForRemoveRigid" delay
	public IEnumerator removeRigids() {
		if (waitForRemoveRigid > 0 && waitForRemoveRigid != waitForDestroy) {
			yield return new WaitForSeconds(waitForRemoveRigid);
			foreach (Transform child in frags) {
				child.GetComponent<Rigidbody>().isKinematic = true;
			}
		}
	}

	// removes colliders from fragments "waitForRemoveCollider" delay
	public IEnumerator removeColliders() {
		if (waitForRemoveCollider > 0) {
			yield return new WaitForSeconds(waitForRemoveCollider);
			foreach (Transform child in frags) {
				child.GetComponent<Collider>().enabled = false;
			}
		}
	}
}
