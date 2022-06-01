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

    private Candy selectedCandy;

    public const int minCandiesMatch = 2;

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

        CreateInitialBoard();
    }

    void CreateInitialBoard()
    {
        Vector2 offset = currentCandy.GetComponent<BoxCollider2D>().size;
        float outMargin = offset.x/1.2F;
        float inMargin = offset.x / 4;
        
        candies = new GameObject[xSize, ySize];

        float startX = this.transform.position.x + outMargin;
        float startY = this.transform.position.y + outMargin;

        int idx = -1;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newCandy = Instantiate(
                    currentCandy, 
                    new Vector3(startX + (offset.x*x) + (inMargin * x), startY + (offset.y*y) + (inMargin * y), 0F), 
                    currentCandy.transform.rotation
                );
                newCandy.name = string.Format("Candy[{0}][{1}]", x, y);

                do
                {
                    idx = Random.Range(0, prefabs.Count);
                }
                while (
                    x > 0 && idx == candies[x - 1, y].GetComponent<Candy>().id ||
                    y > 0 && idx == candies[x, y-1].GetComponent<Candy>().id
                );

                Sprite sprite = prefabs[idx];
                newCandy.GetComponent<SpriteRenderer>().sprite = sprite;
                newCandy.GetComponent<Candy>().id = idx;
                newCandy.transform.parent = this.transform;

                candies[x, y] = newCandy;
            }
        }
    }

    private List<GameObject> FindMatch(Vector2 direction)
    {
        List<GameObject> matchingCandies = new List<GameObject>();



        return matchingCandies;
    }
}
