using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public static Vector3 terrainSize;

    List<Enemy> enemies;
    CharacterControll playerInfo;
    Text hello, counter;
    int enemiesDistance = 2;
    Vector2 beginSize = new Vector2(25, 25);
    int height = 600;
    float timeToPlay = 5;

    [SerializeField]
    private int amountOfEnemies;
    [SerializeField]
    CharacterControll player;
    [SerializeField]
    Terrain terrain;
    [SerializeField]
    GameObject EnemyUnfreezed;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    Slider sprintLevel;



    // Start is called before the first frame update

    private void Awake()
    {
        int terrainMultipler = amountOfEnemies * enemiesDistance;
        terrainSize = new Vector3(beginSize.x + terrainMultipler, height, beginSize.y + terrainMultipler);
        terrain.terrainData.size = terrainSize;
        Cursor.visible = false;

        enemies = new List<Enemy>();

        /*
         * Create Enemies
         * 
         */

         for(int i = 0; i < amountOfEnemies; i++ )
        {
            enemies.Add(Instantiate(EnemyUnfreezed, new Vector3(12 + (i * enemiesDistance), 0, 12 + (i * enemiesDistance)), Quaternion.identity).GetComponent<Enemy>());
        }

        Destroy(EnemyUnfreezed);


        /*
         * 
         * UI
         * 
         * */
        hello = canvas.transform.Find("Hello").GetComponent<Text>();
        counter = canvas.transform.Find("Counter").GetComponent<Text>();
    }

    private void Update()
    {
        sprintLevel.value = player.GetSprint();

        if(timeToPlay > 0)
        {
            string currentTime = timeToPlay.ToString();
            timeToPlay -= Time.deltaTime;
            counter.text = currentTime;
            if (timeToPlay <= 0)
            {
                counter.enabled = false;
                hello.enabled = false;
            }
        }else
        {
            foreach (Enemy e in enemies)
            {
                e.UpdateState();
            }
        }

    }
}
