using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    public bool inRotate;
    public Animator enemyAnimations;

    float accumulateCurrentTime;
    float currentTime;
    float direction;
    Vector3 target;
    bool playerDetected, enemyDetected;
    float wideRunAngel; /* do skomentowania */
    bool [] fallControl;
    bool inCollision;

    [SerializeField]
    private Vector2 timeIntervalBetweenNewDirection;
    [SerializeField]
    private float probabilityOfRotationPerSecond;
    [SerializeField]
    private float movement;
    [SerializeField]
    private float rotationSensitivity;
    [SerializeField]
    Vector2 targetRotateInterval;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimations = this.GetComponent<Animator>() as Animator;
        accumulateCurrentTime = 0.0f;
        direction = 180.0f;
        currentTime = Random.Range(timeIntervalBetweenNewDirection.x, timeIntervalBetweenNewDirection.y);
        target = Vector3.zero;
        inRotate = false;
        playerDetected = false;
        wideRunAngel = 180.0f;
        fallControl = new bool[] { false, false };
        this.transform.eulerAngles = new Vector3(0, Random.Range(0f, 360.0f), 0);
        inCollision = false;
    }

    public void SetChaserTarget(Vector3 newTarget)
    {
        target = newTarget;
        playerDetected = true;
    }

    public void ResetChaserTarget()
    {
        if (playerDetected)
        {
            target = Vector3.zero;
            playerDetected = false;
        }
    }

    public void SetRescueTarget(Vector3 newTarget)
    {
        target = newTarget;
        enemyDetected = true;
    }

    public void ResetRescueTarget()
    {
        if (enemyDetected)
        {
            target = Vector3.zero;
            enemyDetected = false;
        }
    }

    public void Walk()
    {
        for(int i = 0; i < fallControl.Length; i++)
        {
            if (FallControler()[i] && !fallControl[i])
                fallControlSwitcher(i, 180.0f);
            else if (!FallControler()[i] && fallControl[i])
                fallControl[i] = false;
            else if(FallControler()[i] && fallControl[i])
            {
                if(accumulateCurrentTime > 0.4f)
                {
                    transform.LookAt(new Vector3((int)Game.terrainSize.x / 2,0, (int)Game.terrainSize.z / 2) /* Jak sie przez dwie sekundy nie wydostanie idz na srodek*/);
                    accumulateCurrentTime = 0.0f;
                }
            }
        }

        enemyAnimations.SetBool("isWalking", true);
        accumulateCurrentTime += Time.deltaTime;
        transform.position += transform.forward * movement * Time.deltaTime;

        if ((accumulateCurrentTime > currentTime || playerDetected || enemyDetected || inCollision) && !fallControl[0] && !fallControl[1])
        {
            GenerateNewDirection();
        }
    }

    public void InRotate()
    {
        float rotateBy = 0;

        if (this.transform.eulerAngles.y > direction)
            rotateBy = -1 * (rotationSensitivity * Time.deltaTime);

        else if (this.transform.eulerAngles.y < direction)
            rotateBy = (rotationSensitivity * Time.deltaTime);

        this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y + rotateBy, 0);

        if (Mathf.Abs(this.transform.eulerAngles.y - direction) < 0.5f)
        {
            inRotate = false;
            this.transform.eulerAngles = new Vector3(0, direction, 0 );
        }
    }

    void GenerateNewDirection()
    {
        if (playerDetected)
        {
            if (accumulateCurrentTime > Random.Range(targetRotateInterval.x, targetRotateInterval.y))
            {
                Vector3 _direction = (target - transform.position).normalized;
                Quaternion _lookRotation = Quaternion.LookRotation(_direction);

                direction = (_lookRotation.eulerAngles.y + (270 - Random.Range(0f, wideRunAngel))) % 360;
                accumulateCurrentTime = 0f;
            } 
        }else if(enemyDetected)
        {
            Vector3 _direction = (target - transform.position).normalized;
            Quaternion _lookRotation = Quaternion.LookRotation(_direction);

            direction = _lookRotation.eulerAngles.y;
            accumulateCurrentTime = 0f;
            currentTime = 0f;
        }
        else
        {
            inCollision = false;
            accumulateCurrentTime = 0f;
            direction = Random.Range(0f, 360f);
        }

        currentTime = Random.Range(timeIntervalBetweenNewDirection.x, timeIntervalBetweenNewDirection.y);
        inRotate = true;
    }

    bool [] FallControler()
    {
        Vector3 _position = transform.position;
        bool[] result = new bool[] { false, false };

        if (_position.x < 2 || _position.x > Game.terrainSize.x - 2)
            result[0] = true;
        if (_position.z < 2 || _position.z > Game.terrainSize.z - 2)
            result[1] = true;

        return result;

    }

    void fallControlSwitcher(int index, float angle )
    {
        if (index >= fallControl.Length)
            return;

        fallControl[index] = true;
        inRotate = true;
        direction = (direction + angle) % 360.0f;
        accumulateCurrentTime = 0.0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        inCollision = true;
    }

}
