using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChase : MonoBehaviour
{
    public Vector3 playerTarget;
    public bool chaseState;

    List<Enemy> enemyList;
    Vector3 secureTarget;

    private void Start()
    {
        playerTarget = Vector3.zero;
        secureTarget = Vector3.zero;
        chaseState = false;
        enemyList = new List<Enemy>();
    }

    public bool GetRescueState()
    {
        for (int i = enemyList.Count - 1; i >= 0; i--)
        {
            if (enemyList[i].state == Enemy.States.WALK)
                enemyList.RemoveAt(i);
        }

        if (enemyList.Count == 0)
            return false;

        return true;
    }

    public Vector3 GetRescureTarget()
    {
        if (enemyList.Count > 0)
            return enemyList[enemyList.Count - 1].transform.position;
        else
            return Vector3.zero;
    }

    public void EnemyIsWalking(Enemy e)
    {
        enemyList.Remove(e);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerTarget = other.gameObject.transform.position;
            chaseState = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy stateInfo = other.gameObject.GetComponent<Enemy>();

        if (stateInfo != null)
        {
            if (other.gameObject.tag == "Enemy" && stateInfo.state == Enemy.States.STOP)
            {
                enemyList.Add(stateInfo);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerTarget = Vector3.zero;
            chaseState = false;
        }

        Enemy stateInfo = other.gameObject.GetComponent<Enemy>();
        if (stateInfo != null)
        {
            if (other.gameObject.tag == "Enemy" && stateInfo.state == Enemy.States.STOP)
            {
                enemyList.Remove(stateInfo);
            }
        }
    }
}
