using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodResource;
    public int xMaxPos;
    public int xMinPos;
    public int yMaxPos;
    public int yMinPos;
  
    public Collider2D[] colliders;
    public float radius;
   
    public void SpawnFood()
    {
        bool canSpawnHere = false;
        Vector3 foodPosition = new Vector3(0,0,0);
        int safetyNet = 0;
        
        while (!canSpawnHere)
        {
            float spawnPosX = Random.Range(xMinPos, xMaxPos);
            float spawnPosY = Random.Range(yMinPos, yMaxPos);
            foodPosition = new Vector3(spawnPosX, spawnPosY);
            canSpawnHere = PreventSpawnOverlap(foodPosition);

            if (canSpawnHere)
                break;

            safetyNet++;

            if(safetyNet > 50)
            {
                break;
            }
            
       }
        Instantiate(foodResource, foodPosition, Quaternion.identity);
    }

    bool PreventSpawnOverlap(Vector3 spawnPosition)
    {
        colliders = Physics2D.OverlapCircleAll(transform.position, radius);

        for (int i = 0; i < colliders.Length; i++)
        {
            Vector3 centerPoint = colliders[i].bounds.center;
       
            if (spawnPosition == centerPoint)
            {
               return false;
            }

        }
        return true;
    }

}
