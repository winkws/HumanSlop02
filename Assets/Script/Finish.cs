using UnityEngine;

public class Finish : MonoBehaviour
{
    LevelManager levelManager;
    private void Start()
    {
        levelManager = FindAnyObjectByType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            levelManager.CompleteLevel();
        }
    }
}
