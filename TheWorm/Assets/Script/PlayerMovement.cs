using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode right;
    public KeyCode left;
    public float time;
    public float screenOffset = 5;
    public float direction = 1;
    public bool hasMoved;

    public Vector3 screenPos;
    public Vector3 heightwidth;

    public GameObject gameOverUI;
    public GameObject snakeBodyPart;
    public AudioSource walkingSound;
    public AudioSource eatingSound;

    public FoodSpawner foodSpawner;
    public UIManager uiManager;
    public GameOverManager gameOverManager;

    public int points;
   
    float maxMoveTimer;
    float moveTimer;
    float timeMovementReset;
    bool gameOverActivated;
    bool StartedGame;
    Vector2 position;
    Vector2 moveToPosition;
    private int snakeSize;
    private List<Vector2> snakePositionList;
   
  
   
    private void Awake()
    {
        maxMoveTimer = time;
        moveTimer = maxMoveTimer;
        gameOverUI.SetActive(false);
        snakePositionList = new List<Vector2>();
        StartedGame = false;
    }
    private void Start()
    {
        foodSpawner.SpawnFood();
        points = 0;
        uiManager.SaveHighScore();
    }
    void Update()
    {
        PlayerInput();
        AutomaticMovement();
        PlayerOffScreen();
        GameOver();

        if (hasMoved)
        {
            timeMovementReset += Time.deltaTime;
            if (timeMovementReset >= maxMoveTimer)
            {
                hasMoved = false;
                timeMovementReset = 0;
            }
        }
    }

    public void PlayerInput()
    {
        /*Source: CodeMonkey:Making Snake in Unity: Movements https://www.youtube.com/watch?v=0vFucqblH-g&list=PLzDRvYVwl53ucaUs0YGfyyKXdgqh5OtiK&index=2&ab_channel=CodeMonkey*/
        if (!hasMoved)
        {
            if (Input.GetKeyDown(up))
            {
                if (moveToPosition.y != -direction)
                {
                    hasMoved = true;
                    moveToPosition.x = 0;
                    moveToPosition.y = +direction;

                    if (!StartedGame)
                        StartedGame = true;
                }
            }

            if (Input.GetKeyDown(down))
            {
                if (moveToPosition.y != direction)
                {
                    hasMoved = true;
                    moveToPosition.x = 0;
                    moveToPosition.y = -direction;

                    if (!StartedGame)
                        StartedGame = true;
                }
            }

            if (Input.GetKeyDown(right))
            {
                if (moveToPosition.x != -direction)
                {
                    hasMoved = true;
                    moveToPosition.x = +direction;
                    moveToPosition.y = 0;

                    if (!StartedGame)
                        StartedGame = true;
                }

            }

            if (Input.GetKeyDown(left))
            {
                if (moveToPosition.x != direction)
                {
                    hasMoved = true;
                    moveToPosition.x = -direction;
                    moveToPosition.y = 0;

                    if (!StartedGame)
                        StartedGame = true;
                }
            }


            if (Input.GetKeyDown(KeyCode.D))
            {
                PlayerPrefs.DeleteAll();
                uiManager.SaveHighScore();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                gameOverManager.OnClickQuitGame();
            }
        }
    }

    public void AutomaticMovement()
    {
        /*Source: CodeMonkey: Making Snake in Unity: Snake Grow: https://www.youtube.com/watch?v=KifUCu1LLgs&list=PLzDRvYVwl53ucaUs0YGfyyKXdgqh5OtiK&index=4&ab_channel=CodeMonkey*/
        moveTimer += Time.deltaTime;
        if (moveTimer >= maxMoveTimer)
        {
            snakePositionList.Insert(0, position);
            position += moveToPosition;
            moveTimer -= maxMoveTimer;
       
            if (snakePositionList.Count >= snakeSize + 1)
            {
                snakePositionList.RemoveAt(snakePositionList.Count - 1);
            }

            if (StartedGame)
                walkingSound.Play();

           for(int i = 0; i < snakePositionList.Count; i++)
            {
                Vector2 snakeBodyPosition = snakePositionList[i];
                GameObject snakePart = Instantiate(snakeBodyPart);
                snakePart.transform.position = new Vector2(snakeBodyPosition.x, snakeBodyPosition.y);
                
            }
            transform.position = new Vector3(position.x, position.y);
            transform.eulerAngles = new Vector3(0, 0, PlayerAngle(moveToPosition) - 90);

        }
    }

    public float PlayerAngle(Vector2 dir)
    {
        /*Source: CodeMonkey:Making Snake in Unity: Movements https://www.youtube.com/watch?v=0vFucqblH-g&list=PLzDRvYVwl53ucaUs0YGfyyKXdgqh5OtiK&index=2&ab_channel=CodeMonkey*/

        float n = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        if (n < 0)
            n += 360;
        return n;
    }
    public void PlayerOffScreen()
    {
        screenPos = Camera.main.WorldToScreenPoint(transform.position);
        heightwidth.x = Screen.width;
        heightwidth.y = Screen.height;

       
        if (screenPos.x >= (heightwidth.x-screenOffset)
            || screenPos.x <= screenOffset
            || screenPos.y >= (heightwidth.y-screenOffset) 
            || screenPos.y <= screenOffset)
            
            gameOverActivated = true;
                
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        
        if(coll.gameObject.tag == "Food")
        {
            foodSpawner.SpawnFood();
            Destroy(coll.gameObject);
            points++;
            snakeSize++;
            ParticleExplosion();
            eatingSound.Play();
        }

        if (coll.gameObject.tag == "Player")
        {
            gameOverActivated = true;
        }
        
    }

    public void GameOver()
    {
        if (gameOverActivated)
        {
            gameOverUI.SetActive(true);
            uiManager.SaveHighScore();
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;
    }

    public void ParticleExplosion()
    {
        ParticleSystem particleSystem = GetComponent<ParticleSystem>();
        particleSystem.Play();
    }
}
