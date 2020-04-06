using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public float width, deepnes;
    public Game.ENVIRONMENT type;

    private Game.DIRECTION dir;


    private void Start()
    {
        dir = Game.DIRECTION.BACK;
        SetDirection(dir);
    }

    public void SetDirection(Game.DIRECTION _dir)
    {
        dir = _dir;

        switch(_dir)
        {
            case Game.DIRECTION.FORWARD:
                {
                    transform.position = new Vector3(width / 2, transform.position.y, deepnes / 2);
                    transform.eulerAngles = new Vector3(0,0,0);
                }
                break;
            case Game.DIRECTION.BACK:
                {
                    transform.position = new Vector3(width / 2, transform.position.y, Game.terrainSize.z - ( deepnes / 2 ) );
                    transform.eulerAngles = new Vector3(0, 180, 0);
                }
                break;
            case Game.DIRECTION.LEFT:
                {
                    transform.position = new Vector3(deepnes / 2, transform.position.y, width / 2);
                    transform.eulerAngles = new Vector3(0, 270, 0);
                }
                break;
            case Game.DIRECTION.RIGHT:
                {
                    transform.position = new Vector3(Game.terrainSize.x - ( deepnes / 2 ), transform.position.y, width / 2);
                    transform.eulerAngles = new Vector3(0, 90, 0);
                }
                break;
        }
    }
}
