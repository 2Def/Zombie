using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public enum States { WALK, STOP, TARGET };
    public States state;

    EnemyWalk enemyWalk;
    EnemyChase enemyChase;
    AudioSource audioSource;


    [SerializeField]
    private AudioClip zombieMoan;

    // Start is called before the first frame update
    void Start()
    {
        state = States.WALK;
        enemyWalk = GetComponentInChildren<EnemyWalk>();
        enemyChase = GetComponentInChildren<EnemyChase>();
        audioSource = this.gameObject.AddComponent<AudioSource>();

        Audio();
    }

    // Update is called once per frame
    public void UpdateState(Game.GAME stopAnimation = Game.GAME.PLAY)
    {
        switch (state)
        {
            case States.WALK:
                {
                    if (stopAnimation == Game.GAME.PAUSE)
                    {
                        enemyWalk.enemyAnimations.SetBool("isWalking", false);
                        return;
                    }

                    if (enemyChase.chaseState)
                        enemyWalk.SetChaserTarget(enemyChase.playerTarget);
                    else
                    {
                        enemyWalk.ResetChaserTarget();

                        if (enemyChase.GetRescueState())
                            enemyWalk.SetRescueTarget(enemyChase.GetRescureTarget());
                        else
                            enemyWalk.ResetRescueTarget();
                    }
                    enemyWalk.Walk();
                }
                break;
        }

        if (enemyWalk.inRotate)
            enemyWalk.InRotate();
    }

    void Audio()
    {
        audioSource.loop = true;
        audioSource.spatialBlend = 1.0f;
        audioSource.clip = zombieMoan;
        audioSource.maxDistance = 5;
        audioSource.volume = 0.2f;
        audioSource.Play();
    }

    public void Catched()
    {
        state = States.STOP;
        enemyWalk.enemyAnimations.SetBool("isWalking", false);
        Game.CheckWinGame();
    }

    private void OnCollisionEnter(Collision collision)
    {
        Enemy currentCollided = collision.gameObject.GetComponent<Enemy>();
        if(state == States.WALK && currentCollided != null )
        {
            if (collision.gameObject.tag == "Enemy" && currentCollided.state == States.STOP)
            {
                currentCollided.state = States.WALK;
                enemyChase.EnemyIsWalking(currentCollided);
            }
        }
        
    }

}
