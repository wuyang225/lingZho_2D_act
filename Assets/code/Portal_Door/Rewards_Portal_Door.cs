using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rewards_Portal_Door : Portal_Door
{

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        player.SetLastScenceMessage(MapRandomSeedManager.Instance.currentMapSceneName,this.transform.position);
        SetPlayerSpawnPosition();
        showRewards();
    }

    private void showRewards()
    {
        MapRandomSeedManager.Instance.reward.SetActive(true);
        if (MapRandomSeedManager.Instance.currentMapSceneName == "GameScene2")
        {
            switch (MapRandomSeedManager.Instance.randomSeed2)
            {
                case 1:
                    MapRandomSeedManager.Instance.map_1.SetActive(false);
                    break;
                case 2:
                    MapRandomSeedManager.Instance.map_2.SetActive(false);
                    break;
                case 3:
                    MapRandomSeedManager.Instance.map_3.SetActive(false);
                    break;
            }

        }
        if (MapRandomSeedManager.Instance.currentMapSceneName == "GameScene3")
        {
            switch (MapRandomSeedManager.Instance.randomSeed3)
            {
                case 1:
                    MapRandomSeedManager.Instance.map_1.SetActive(false);
                    break;
                case 2:
                    MapRandomSeedManager.Instance.map_2.SetActive(false);
                    break;
                case 3:
                    MapRandomSeedManager.Instance.map_3.SetActive(false);
                    break;
            }
        }
        Destroy(this.gameObject);
    }
}
