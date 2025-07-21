using UnityEngine;
using System.Collections;

public class Timers : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public void DashCoroutine()
    {
        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        player.DashOnCooldown = true;
        yield return new WaitForSeconds(player.PlayerData.dashCooldown);
        player.DashOnCooldown = false;
    }
}
