using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class last_Portal_Door : Portal_Door
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        showRewards();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        SetInit(player.lastScenceName, player.lastScencePos);
        SetPlayerSpawnPosition();
    }
    private void showRewards()
    {
        MapRandomSeedManager.Instance.reward.SetActive(false);
        if (MapRandomSeedManager.Instance.currentMapSceneName == "GameScene2")
        {
            switch (MapRandomSeedManager.Instance.randomSeed2)
            {
                case 1:
                    MapRandomSeedManager.Instance.map_1.SetActive(true);
                    break;
                case 2:
                    MapRandomSeedManager.Instance.map_2.SetActive(true);
                    break;
                case 3:
                    MapRandomSeedManager.Instance.map_3.SetActive(true);
                    break;
            }

        }
        if (MapRandomSeedManager.Instance.currentMapSceneName == "GameScene3")
        {
            switch (MapRandomSeedManager.Instance.randomSeed3)
            {
                case 1:
                    MapRandomSeedManager.Instance.map_1.SetActive(true);
                    break;
                case 2:
                    MapRandomSeedManager.Instance.map_2.SetActive(true);
                    break;
                case 3:
                    MapRandomSeedManager.Instance.map_3.SetActive(true);
                    break;
            }
        }
    }
}
