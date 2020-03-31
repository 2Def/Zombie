using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public static Vector3 terrainSize;
    public enum GAME { PLAY, PAUSE };

    GAME game;
    static List<Enemy> enemies;
    CharacterControll playerInfo;
    static Text hello, counter, wintext;
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
    GameObject pauseMenu;
    [SerializeField]
    Slider sprintLevel;

    private void Awake()
    {
        int terrainMultipler = amountOfEnemies * enemiesDistance;
        terrainSize = new Vector3(beginSize.x + terrainMultipler, height, beginSize.y + terrainMultipler);
        terrain.terrainData.size = terrainSize;
        Cursor.visible = false;
        game = GAME.PLAY;

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
        wintext = canvas.transform.Find("WinText").GetComponent<Text>();

        wintext.enabled = false;

    }

    private void Update()
    {

        sprintLevel.value = player.GetSprint();

        switch(game)
        {
            case GAME.PLAY:
                {

                    if(Input.GetKeyDown(KeyCode.Escape))
                    {
                        game = GAME.PAUSE;
                        GameState(true);
                    }

                    if (timeToPlay > 0)
                    {
                        string currentTime = timeToPlay.ToString();
                        timeToPlay -= Time.deltaTime;
                        counter.text = currentTime;
                        if (timeToPlay <= 0)
                        {
                            counter.enabled = false;
                            hello.enabled = false;
                        }
                    }
                    else
                    {
                        foreach (Enemy e in enemies)
                        {
                            e.UpdateState(game);
                        }
                    }
                }
                break;
        }
    }

    void GameState(bool _s)
    {
        pauseMenu.SetActive(_s);
        Cursor.visible = _s;
        player.Pause(_s);
    }

    public static void CheckWinGame()
    {
        int amountOfEnemies = enemies.Count;
        int currentFreezed = 0;

        foreach(Enemy e in enemies)
        {
            if (e.state == Enemy.States.STOP)
                currentFreezed++;
        }
        if (amountOfEnemies == currentFreezed)
            wintext.enabled = true;
    }

    public void ExitGame()
    {
        
        Application.Quit();
    }

    public void ResumeGame()
    {
        game = GAME.PLAY;
        GameState(false);
    }
}
