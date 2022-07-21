using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private UI ui;
    [SerializeField]
    private GameObject circle;
    [SerializeField]
    private Texture2D circleImage;
    private List<Texture2D> circleTextures;
    private List<Texture2D> circleTexturesNext;
    private Coroutine generateCircles;
    private float circleCount;

    public List<Circle> circles { get; set; }
    public List<Sprite> backgrounds { get; set; }
    public int scoreForNextLevel { get; private set; }
    public static float screenWidth { get; private set; }
    public static float screenHeight { get; private set; }

    private int score;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            if (score >= scoreForNextLevel)
                NextLevel();
            ui.UpdateScore();
        }
    }
    public int level { get; private set; }
    public float speedFactor;


    private void Awake()
    {
        UpdateScreenSize();
        backgrounds = new List<Sprite>();
    }
    void Start()
    {
        circleTextures = new List<Texture2D>();
        circleTexturesNext = new List<Texture2D>();
        circles = new List<Circle>();
        ResetLevel();
        GetComponent<LoadBackground>().StartDownload();
    }
    public void StartGame()
    {
        scoreForNextLevel = (int)(Mathf.Pow(level, 2) / 8 * 15 + 20); // some level gradation
        score = 0;
        ui.ResetTimer();
        circleTextures = generateTextures_32x64x128x256();
        generateCircles = StartCoroutine(GenerateCircle());
    }
    private Texture2D ScaleAndColorTexture(Texture2D source, int targetWidth, int targetHeight, Color c)
    {
        Texture2D result = new Texture2D(targetWidth, targetHeight, source.format, false);
        for (int i = 0; i < result.height; ++i)
        {
            for (int j = 0; j < result.width; ++j)
            {
                Color newColor = source.GetPixelBilinear((float)j / result.width, (float)i / result.height);
                result.SetPixel(j, i, newColor * c);
            }
        }
        result.Apply();
        return result;
    }
    List<Texture2D> generateTextures_32x64x128x256()
    {
        List<Texture2D> res = new List<Texture2D>();
        System.Random rnd = new System.Random();
        for (int i = 32; i <= 256; i *= 2)
        {
            var c = new Color(rnd.Next(10) / 10.0f, rnd.Next(10) / 10.0f, rnd.Next(10) / 10.0f, 1);
            var t = ScaleAndColorTexture(circleImage, i, i, c);
            res.Add(t);
        }
        return res;
    }
    void NextLevel()
    {
        circleTextures.ForEach(x => { if (x != null) { Destroy(x); } });
        circleTextures.Clear();
        circleTextures = circleTexturesNext.Count == 0 ? generateTextures_32x64x128x256() : circleTexturesNext;
        circleTexturesNext = generateTextures_32x64x128x256();

        speedFactor += 0.2f;
        circleCount *= 1.2f;
        level++;
        ui.StartGame(level);
    }
    public void StopGame()
    {
        if (generateCircles != null)
            StopCoroutine(generateCircles);
        circles.ForEach(c => { if (c != null) { Destroy(c.gameObject); } });
        circles.Clear();
    }
    public void ResetLevel()
    {
        level = 1;
        speedFactor = 1f;
        circleCount = 1f;
    }
    private IEnumerator GenerateCircle()
    {
        while (true)
        { 
            System.Random rnd = new System.Random();
            float r, x, y;
            r = rnd.Next((int)Circle.minRadius, (int)Circle.maxRadius) + rnd.Next(10)/10.0f ;
            x = r + rnd.Next(1, (int)(screenWidth - r - 1));
            y = -r;
            
            Circle c = Instantiate(circle).GetComponent<Circle>();
            c.radius = r;
            c.speed = speedFactor * (8 / r);    // some speed gradation
            c.score = (int)(1 / r * 5 + 3);     // some score gradation
            c.screenPosition = new Vector2(x, y);

            var i = (r - Circle.minRadius) / (Circle.maxRadius - Circle.minRadius) * (circleTextures.Count - 1);
            c.texture = circleTextures[(int)Mathf.Round(i)];
            circles.Add(c);

            yield return new WaitForSeconds(1 / circleCount);
        }
    }

    public static Vector3 ScreenToGlobalPos(Vector3 v)
    {
        var cam = Camera.main;
        return new Vector3(v.x - screenWidth / 2 + cam.transform.position.x, -v.y + cam.orthographicSize + cam.transform.position.y, v.z);
    }
    public static Vector3 GlobalToScreenPos(Vector3 v)
    {
        var cam = Camera.main;
        return new Vector3(v.x + screenWidth / 2 - cam.transform.position.x, v.y - cam.orthographicSize - cam.transform.position.y, v.z);
    }

    void UpdateScreenSize()
    {
        var cam = Camera.main;
        screenHeight = cam.orthographicSize * 2.0f;
        screenWidth = screenHeight * cam.aspect;
    }
    void Update()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.touches[0].position);
            RaycastHit2D? hit = Physics2D.Raycast(pos, Vector2.zero);
            Circle circle = hit?.collider?.GetComponentInParent<Circle>();
            circle?.OnClick();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D? hit = Physics2D.Raycast(pos, Vector2.zero);
            Circle circle = hit?.collider?.GetComponentInParent<Circle>();
            circle?.OnClick();
        }       
    }
}
