using UnityEngine;

public class BodyHitTrigger : MonoBehaviour
{
    public string groundTag;
    public LevelManager levelManager;

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == groundTag && !levelManager.isCrashing)
        {
            levelManager.BeforeRestart();
        }
    }
}
