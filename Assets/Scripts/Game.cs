using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public static Vector3 terrainSize;
    public enum GAME { PLAY, PAUSE };
    public enum ENVIRONMENT { WALL, BORDER, INNER };
    public enum DIRECTION { FORWARD, BACK, LEFT, RIGHT }

    GAME game;
    static List<Enemy> enemies;
    List<ParticleSystem> particles;
    CharacterControll playerInfo;
    static Text hello, counter, wintext;
    int enemiesDistance = 2;
    Vector2 beginSize = new Vector2(25, 25);
    int height = 600;
    float timeToPlay = 5;


    [SerializeField]
    private int amountOfEnemies;
    [SerializeField]
    private CharacterControll player;
    [SerializeField]
    private Terrain terrain;
    [SerializeField]
    private GameObject EnemyUnfreezed;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private Slider sprintLevel;
    [SerializeField]
    private List<GameObject> externEnvironment;
    [SerializeField]
    private float rotationSpeedOfParticles;

    private void Awake()
    {
        int terrainMultipler = amountOfEnemies * enemiesDistance;
        terrainSize = new Vector3(beginSize.x + terrainMultipler, height, beginSize.y + terrainMultipler);
        terrain.terrainData.size = terrainSize;
        Cursor.visible = false;
        game = GAME.PLAY;

        externEnvironment = new List<GameObject>();
        enemies = new List<Enemy>();
        particles = new List<ParticleSystem>();

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

        /*
         * 
         * Create Particles
         * 
         * */
        SetParticles();

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

                    RotateParticles();
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

    void SetParticles()
    {
        ParticleSystem particle = GameObject.FindGameObjectWithTag("Particle").GetComponent<ParticleSystem>();
        for(int i=0; i<2; i++)
        {
            for(int j=0; j<2; j++)
            {
                particles.Add( Instantiate(particle, new Vector3(terrainSize.x * i ,-1.0f, terrainSize.z * j ) ,Quaternion.identity ) );
                particles[particles.Count - 1].startLifetime = 0.5f * amountOfEnemies; 
            }
        }
        Destroy(particle);
    }

    void RotateParticles()
    {
        foreach(ParticleSystem ps in particles)
        {
            ps.transform.eulerAngles = new Vector3(90.0f, (ps.transform.eulerAngles.y + (rotationSpeedOfParticles * Time.deltaTime)) % 360.0f , 0.0f);
        }
    }

    void GenerateEnvironment()
    {
        /* Begin is Front */
        int randomTrys = externEnvironment.Count;
        Vector2 currentPos = Vector2.zero;
        DIRECTION currentDir = DIRECTION.FORWARD;




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
