using UnityEngine;
using System.Collections.Generic;
public class PlayerMovement : MonoBehaviour
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode right;
    public KeyCode left;
    public float direction = .3f;
    public float time;
    public float screenOffset = 5;
    public float maxMoveTimer;
    public Vector3 screenPos;
    public Vector3 heightwidth;
    public GameObject gameOverUI;
    public GameObject snakeBodyPart;
    public FoodSpawner foodSpawner;
    public UIManager scoreUI;
    public int points;
   
    float moveTimer;
    Vector2 position;
    Vector2 moveToPosition;

    private int snakeSize;
    public List<Vector2> snakePositionList;

    bool gameOverActivated;

    private void Awake()
    {
        maxMoveTimer = time;
        moveTimer = maxMoveTimer;
        gameOverUI.SetActive(false);
        snakePositionList = new List<Vector2>();
    }
    private void Start()
    {
        foodSpawner.SpawnFood();
        points = 0;
        scoreUI.SaveHighScore();
    }
    void Update()
    {
        PlayerInput();
        AutomaticMovement();
        PlayerOffScreen();
        GameOver();
    }

    public void PlayerInput()
    {
        if (Input.GetKeyDown(up))
        {
            if (moveToPosition.y != -1)
            {
                moveToPosition.x = 0;
                moveToPosition.y = +1;
            }
        }

        if (Input.GetKeyDown(down))
        {
            if (moveToPosition.y != 1)
            {
                moveToPosition.x = 0;
                moveToPosition.y = -1;
            }
        }

        if (Input.GetKeyDown(right))
        {
            if (moveToPosition.x != -1)
            {
                moveToPosition.x = +1;
                moveToPosition.y = 0;
            }

        }

        if (Input.GetKeyDown(left))
        {
            if (moveToPosition.x != 1)
            {
                moveToPosition.x = -1;
                moveToPosition.y = 0;
            }
        }
    }

    public void AutomaticMovement()
    {
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
        if (screenPos.x > heightwidth.x
            || screenPos.x < 0f
            || screenPos.y > heightwidth.y
            || screenPos.y < 0f)
            
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
            scoreUI.SaveHighScore();
            Time.timeScale = 0;
        }
        else
            Time.timeScale = 1;
    }
}
