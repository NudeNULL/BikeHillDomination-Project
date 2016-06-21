using UnityEngine;

public class SilhouetteController : MonoBehaviour
{
    Vector3 startPosition;
    Vector3 endPosition;

    float xLimit = 8.5f;
    float yLimit = 4.5f;

    float startDistance;

    public float moveSpeed;

    public Sprite[] silhouettesSprites;

    SpriteRenderer spriteRenderer = null;

    Color tmpColor;

    float traveledDistance;

    // Use this for initialization
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = silhouettesSprites[Random.Range(0, silhouettesSprites.Length)];

        startPosition = new Vector3(Random.Range(-xLimit, xLimit), Random.Range(-yLimit, yLimit));
        endPosition = new Vector3 { x = startPosition.x + Random.Range(-1, 1) * Random.Range(2, 4), y = startPosition.y + Random.Range(-1, 1) * Random.Range(1, 3) };

        startDistance = Vector3.Distance(startPosition, endPosition);

        transform.position = startPosition;

        float scaleFactor = Random.Range(0.45f, 0.85f);

        transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, endPosition) > 0.05f)
        {
            tmpColor = spriteRenderer.color;

            traveledDistance = Vector3.Distance(startPosition, transform.position);

            if (traveledDistance / startDistance < 1f / 3f)
            {
                tmpColor.a = (traveledDistance / startDistance) * 3f;
            }
            else
            {
                if (traveledDistance / startDistance >= 2f / 3f)
                {
                    tmpColor.a = (startDistance - traveledDistance) / startDistance * 3f;
                }
                else
                {
                    tmpColor.a = 1f;
                }
            }

            spriteRenderer.color = tmpColor;
            transform.position = Vector3.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
