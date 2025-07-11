using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Transform startPosition;
    [SerializeField] GameObject player;

    void Start()
    {
        player.transform.position = startPosition.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            player.transform.position = startPosition.position;
            player.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        }
    }

    public void CompleteLevel()
    {
        Debug.Log("level complete, not bad for a dead guy huh");
    }
}
