using UnityEngine;

public class Circle : MonoBehaviour
{
    private GameController gameController;
    private Material material;
    public static float minRadius { get; private set; } = 1.0f;
    public static float maxRadius { get; private set; } = 3.0f;
    public static float globalScale { get; set; } = 0.035f;
    public int score { get; set; }
    public float speed { get; set; }
    public Texture2D texture
    {
        get { return material.GetTexture("Texture2D_18ef609ef5704606a693cd05d4a9b1d0") as Texture2D; }
        set { material.SetTexture("Texture2D_18ef609ef5704606a693cd05d4a9b1d0", value); }
    }
    public Vector2 screenPosition
    {
        get { return GameController.GlobalToScreenPos(transform.position); }
        set { transform.position = GameController.ScreenToGlobalPos(value); }
    }
    public float radius
    {
        get { return transform.localScale.x / 2 / globalScale; }
        set
        {
            transform.localScale = new Vector3(value * 2 * globalScale, value * 2 * globalScale, transform.localScale.z * globalScale);
        }
    }

    void Awake()
    {
        gameController = FindObjectOfType<GameController>();
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        if (screenPosition.y < -GameController.screenHeight - radius)
            DestroyCircle();
    }
    public void OnClick()
    {
        gameController.Score += score;
        DestroyCircle();
        // print($"{radius}    {texture.width}");
    }
    void DestroyCircle()
    {
        gameController.circles.Remove(this);
        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(0, -0.01f * speed, 0);
    }
}
