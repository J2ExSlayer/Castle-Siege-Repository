using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class DeathDetection : MonoBehaviour
{

    /* Teleport Script
public Transform player, destination;
public GameObject playerg;


private void OnTriggerEnter(Collider other)
{
if (other.CompareTag("Player"))
{
playerg.SetActive(false);
player.position = destination.position;
playerg.SetActive(true);
}
}
*/

    public Transform player, destination;
    public GameObject playerg;

    public float deathStartTimer = 2f;
    public float deathTimer = 2f;

    public PlayerMovement playerMove;


    

    private void OnTriggerEnter(Collider other)
    {

        /*
        if(other.CompareTag("Player"))
        {
            StartCoroutine(DeathTimer());
            deathTimer -= 1f;
            if (deathTimer <= 0f)
            {
                playerg.SetActive(false);
                player.position = destination.position;
                playerg.SetActive(true);
            }

            //StartCoroutine(DeathTimer());
            //deathTimer -= (1 * Time.deltaTime);

        }
        else 
        {
            deathTimer = 2f;
        }
        /*
        else 
        {
            deathTimer = deathStartTimer;
        }
        */

        
        if(other.CompareTag("Player"))
        {

            //StartCoroutine(DeathTimer());
            playerg.SetActive(false);
            player.position = destination.position;
            playerg.SetActive(true);

        }
        //else
            //deathTimer = 2f;
            
            
        
        //deathTimer = 2f;
        
    }

    /*
    void DeathTimer()
    {
        if(deathTimer <= 0)
        {
            playerg.SetActive(false);
            player.position = destination.position;
            playerg.SetActive(true);
        }
        deathTimer -= Time.deltaTime;

    }
    */

    
    IEnumerator DetectionTimer()
    {
        yield return new WaitForSeconds(2f);
        
        
        
    }
    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(2f);
       
            
        

        //deathTimer -= 1f;
        //deathTimer--;


        //else
        //    deathTimer = 2f;



    }
    

}
