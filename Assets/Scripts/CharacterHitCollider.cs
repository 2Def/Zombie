using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHitCollider : MonoBehaviour
{

    float maxAngel = 90.0f;
    bool inCatch = false;
    float accumulateTime = 0.0f;

    [SerializeField]
    private float hitTime;


    void Update()
    {
        if (inCatch) {
            accumulateTime += Time.deltaTime;

            if (accumulateTime < hitTime )
            {
                this.transform.localEulerAngles = new Vector3(0, -1 * maxAngel * (accumulateTime / hitTime), 0);
            }
            else
            {
                accumulateTime = 0.0f;
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                inCatch = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (inCatch && other.gameObject.tag == "Enemy")
        {
            Enemy hittedEnemy = other.gameObject.GetComponent<Enemy>();
            hittedEnemy.Catched();
        }
    }

    public void TryCatch()
    {
        if(!inCatch)
        {
            inCatch = true;
        }
    }
}
