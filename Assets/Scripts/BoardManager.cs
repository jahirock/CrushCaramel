using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager sharedInstace;
    public List<Sprite> prefabs = new List<Sprite>();
    public GameObject currentCandy;
    public int xSize, ySize;

    public bool isShifting { get; set; }

    GameObject[,] candies;

    // Start is called before the first frame update
    void Start()
    {
        if(sharedInstace == null)
        {
            sharedInstace = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Vector2 offset = currentCandy.GetComponent<BoxCollider2D>().size;
        CreateInitialBoard(offset);
    }

    void CreateInitialBoard(Vector2 offset)
    {
        candies = new GameObject[xSize, ySize];

        float startX = this.transform.position.x;
        float startY = this.transform.position.y;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newCandy = Instantiate(
                    currentCandy, 
                    new Vector3(startX + (offset.x*x), startY + (offset.y*y), 0F), 
                    currentCandy.transform.rotation
                );

                newCandy.name = string.Format("Candy[{0}][{1}]", x, y);

                candies[x, y] = newCandy;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
