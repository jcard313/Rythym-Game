using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{

    public GameObject hitEffect, goodEffect, perfectEffect, missEffect;

    public bool canBePressed;
    public KeyCode keyToPress;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyToPress)) {

             if(canBePressed) {

                gameObject.SetActive(false);
                if (Math.Abs(transform.position.y)>0.25) {
                    Debug.Log("Normal");
                    GameManager.instance.NormalHit();
                    Instantiate(hitEffect, transform.position, hitEffect.transform.rotation);
                } else if (Math.Abs(transform.position.y)>0.2f) {
                    Debug.Log("Good");
                    GameManager.instance.GoodHit();
                    Instantiate(goodEffect, transform.position, goodEffect.transform.rotation);
                } else {
                    Debug.Log("Perfect");
                    GameManager.instance.PerfectHit();
                    Instantiate(perfectEffect, transform.position, perfectEffect.transform.rotation);
                }
             }
        }
        
    }

    private void OnTriggerEnter2D(Collider2D other) {

        Debug.Log("Note threshold in button " + GameManager.instance.noteThreshold);

        // accounting for the change in note threshold due to pressure
        if (other.tag=="Activator" && Math.Abs(transform.position.y)<=GameManager.instance.noteThreshold) {
            canBePressed = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag=="Activator" && gameObject.activeSelf) {
            canBePressed = false;
            GameManager.instance.NoteMissed();
            Instantiate(missEffect, transform.position, missEffect.transform.rotation);
        }
    }
}