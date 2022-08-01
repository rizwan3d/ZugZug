
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager instance;

    public GameObject topTilePrefab;
    public GameObject leftTilePrefab;

    public GameObject currentTile;

    private Stack<GameObject> _topTiles = new Stack<GameObject>();
    private Stack<GameObject> _leftTiles = new Stack<GameObject>();

    [SerializeField]
    Color[] colorList; 
     public FilterMode filterMode = FilterMode.Point;
    public TextureWrapMode wrapMode = TextureWrapMode.Clamp;
    public bool isLinear = false;
    public bool hasMipMap = false;


    int nextIndex = -1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        CreateTiles(100);

        for (int i = 0; i < 50; i++)
            instance.SpawnTile();
    }

    public void CreateTiles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            _topTiles.Push(Instantiate(topTilePrefab));
            _leftTiles.Push(Instantiate(leftTilePrefab));
            _topTiles.Peek().SetActive(false);
            _leftTiles.Peek().SetActive(false);
        }
    }

    public void SpawnTile()
    {
        if (_leftTiles.Count <= 0 || _topTiles.Count <= 0)
        {
            CreateTiles(100);
        }

        int index = Random.Range(0, 2);
        if (index == 0)
        {
            GameObject temp = _topTiles.Pop();
            temp.SetActive(true);
            temp.transform.position = currentTile.transform.GetChild(0).GetChild(index).position;
            currentTile = temp;
        }
        else if (index == 1)
        {
            GameObject temp = _leftTiles.Pop();
            temp.SetActive(true);
            temp.transform.position = currentTile.transform.GetChild(0).GetChild(index).position;
            currentTile = temp;
        }

        int pickupIndex = Random.Range(0, 10);
        if (pickupIndex == 0)
        {
            currentTile.transform.GetChild(1).gameObject.SetActive(true);
        }

        var cubeRenderer = currentTile.transform.GetChild (0).gameObject.GetComponent<Renderer>();

        int colorIndex = nextIndex < 0 ? 0 : nextIndex;     

        Color[] newColorList = new Color[2];
        newColorList[0] = colorList[colorIndex];


        nextIndex = Random.Range(0,colorList.Length);

        while(nextIndex == colorIndex){
            nextIndex = Random.Range(0,colorList.Length);
        }

        newColorList[1] = colorList[nextIndex];


        currentTile.transform.GetChild (0).gameObject.GetComponent<MeshRenderer>().material.mainTexture = GradientTextureMaker.Create(newColorList, wrapMode, filterMode, isLinear, hasMipMap);
        //cubeRenderer.material.SetColor("_Color", colorList[colorIndex]);
    }

    public void AddTopTile(GameObject obj)
    {
        _topTiles.Push(obj);
    }

    public void AddLeftTile(GameObject obj)
    {
        _leftTiles.Push(obj);
    }
}
