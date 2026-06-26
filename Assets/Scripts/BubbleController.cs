using UnityEngine;

public class BubbleController : MonoBehaviour
{
    public enum BubbleSize { Large, Medium, Small }

    [Header("Config")]
    public BubbleSize currentSize = BubbleSize.Large;
    public GameObject bubblePrefab;

    private Rigidbody2D rb;
    private float horizontalSpeed;
    private float bounceForce;
    private bool initialized = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (!initialized)
        {
            ApplySize();
            float dir = Random.value > 0.5f ? 1f : -1f;
            rb.linearVelocity = new Vector2(dir * horizontalSpeed, bounceForce);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Floor"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
            AudioManager.Instance?.PlayBubbleBounce();
        }

        if (col.gameObject.CompareTag("Wall"))
        {
            float bounceDir = col.contacts[0].normal.x > 0 ? 1f : -1f;
            rb.linearVelocity = new Vector2(bounceDir * horizontalSpeed, rb.linearVelocity.y);
            AudioManager.Instance?.PlayBubbleBounce();
        }
    }

    public void ApplySize()
    {
        switch (currentSize)
        {
            case BubbleSize.Large:
                transform.localScale = Vector3.one * 1.4f;
                horizontalSpeed = 2.5f;
                bounceForce = 16f;
                GetComponent<SpriteRenderer>().color = Color.red;
                break;

            case BubbleSize.Medium:
                transform.localScale = Vector3.one * 0.9f;
                horizontalSpeed = 3.5f;
                bounceForce = 14f;
                GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0f);
                break;

            case BubbleSize.Small:
                transform.localScale = Vector3.one * 0.5f;
                horizontalSpeed = 5f;
                bounceForce = 14f;
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0f);
                break;
        }
    }

    public void InitChild(BubbleSize size, GameObject prefab, Vector2 velocity)
    {
        currentSize = size;
        bubblePrefab = prefab;
        initialized = true;

        rb = GetComponent<Rigidbody2D>();
        ApplySize();
        rb.linearVelocity = velocity;
    }

    public void GetHit()
    {
        if (this == null || !gameObject.activeInHierarchy) return;

        AudioManager.Instance?.PlayBubblePop();

        if (currentSize == BubbleSize.Large)
        {
            SpawnChildren(BubbleSize.Medium);
            Destroy(gameObject);
        }
        else if (currentSize == BubbleSize.Medium)
        {
            SpawnChildren(BubbleSize.Small);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        GameManager.Instance?.Invoke("CheckWin", 0.1f);
    }

    void SpawnChildren(BubbleSize childSize)
    {
        float childSpeed = childSize == BubbleSize.Medium ? 3.5f : 5f;
        float childBounce = childSize == BubbleSize.Medium ? 10f : 8f;

        GameObject prefabToSpawn = bubblePrefab != null ? bubblePrefab : gameObject;

        for (int i = 0; i < 2; i++)
        {
            float dir = (i == 0) ? -1f : 1f;
            Vector2 velocity = new Vector2(dir * childSpeed, childBounce);

            Vector3 spawnPos = transform.position + new Vector3(dir * 0.5f, 0f, 0f);

            GameObject child = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
            child.GetComponent<BubbleController>().InitChild(childSize, prefabToSpawn, velocity);
        }
    }
}