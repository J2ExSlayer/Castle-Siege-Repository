using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EasyAIPatrol : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform currentPoint;

    int currentPatrolPointIndex;

    


    NavMeshAgent nav;

    float currentWaitingTime;
    float maxWaitingTime;

    // Start is called before the first frame update
    void Start()
    {
        
        nav = GetComponent<NavMeshAgent>();

        currentPatrolPointIndex = -1;

        currentWaitingTime = 0;
        maxWaitingTime = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if(nav.remainingDistance < 0.5f)
        {
            if (maxWaitingTime == 0)
                maxWaitingTime = Random.Range(3, 8);

            if (currentWaitingTime >= maxWaitingTime)
            {
                maxWaitingTime = 0;
                currentWaitingTime = 0;
                GoToNextPoint();

                transform.LookAt(currentPoint);
            }
            else
                currentWaitingTime += Time.deltaTime;
        }
    }


    void GoToNextPoint()
    {
        if(patrolPoints.Length != 0)
        {
            currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
            nav.SetDestination(patrolPoints[currentPatrolPointIndex].position);
        }
    }

}
