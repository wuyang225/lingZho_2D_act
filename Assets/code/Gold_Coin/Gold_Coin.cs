using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold_Coin : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rigi;
    [SerializeField] Vector2 konckback = new Vector2(4, 4);

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rigi = GetComponentInChildren<Rigidbody2D>();
    }
    public void CreatGold_Coin(float dri)
    {
        Vector2 strength = new Vector2(Random.Range(0, konckback.x), Random.Range(0, konckback.y));
        strength.x *= -dri;
        rigi.velocity = strength;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")|| collision.gameObject.layer == LayerMask.NameToLayer("Dash"))
        {
            OtherMusic.Instance.ChangeAudioClip("GetCoin", false);
            Player player = collision.gameObject.GetComponent<Player>();
            player.goldCoinNumber++;
            player.rbody.velocity = Vector2.zero;
            GoldNumber.Instance.ChangeGoldNumberUIText(player.goldCoinNumber);
            Destroy(this.gameObject);
        }
    }
}
