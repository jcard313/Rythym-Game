// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class test : MonoBehaviour {

//     public bool canBePressed;

//     public KeyCode keyToPress;

//     public GameObject normaleffect, goodefect, perfectmate, bruh;

//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(Input.GetKeyUp(keyToPress))
//         {
//             if(canBePressed)
//             {
//                 gameObject.SetActive(false);

//                 //managelegame.instance.NoteHit();

//                 if(Mathf.Abs( transform.position.y) > 0.25)
//                 {
//                     Debug.Log("beuh");
//                     managelegame.instance.NormalHit();
//                     Instantiate(normaleffect, transform.position, normaleffect.transform.rotation);
//                 } else if(Mathf.Abs(transform.position.y) > 0.05)
//                 {
//                     Debug.Log("good");
//                     managelegame.instance.GoodHit();
//                     Instantiate(goodefect, transform.position, goodefect.transform.rotation);
//                 } else
//                 {
//                     Debug.Log("perfect");
//                     managelegame.instance.PerfectHit();
//                     Instantiate(perfectmate, transform.position, perfectmate.transform.rotation);
//                 }
//             }
//         }
//     }

//     private void OnTriggerEnter2D(Collider2D other)
//     {
//         if (other.tag == "Activator")
//         {
//             canBePressed = true;
//         }

//     }

//     private void OnTriggerExit2D(Collider2D other)
//     {
//         if (other.tag == "Activator" && gameObject.activeSelf)
//         {
//             canBePressed = false;
    
//             managelegame.instance.NoteMissed();
//             Instantiate(bruh, transform.position, bruh.transform.rotation);
//         }
//     }
// }