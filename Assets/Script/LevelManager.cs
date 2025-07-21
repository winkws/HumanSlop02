using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Transform startPosition;
    [SerializeField] GameObject player;

    void Start()
    {
        player.transform.position = startPosition.position;
    }

    public void CompleteLevel()
    {
        Debug.Log("level complete, not bad for a dead guy huh");
    }
}
