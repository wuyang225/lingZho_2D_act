using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Main : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rigi;
    public Animator anim;
    public float moveSpeed;
    private void Awake()
    {
        rigi = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponentInChildren<Animator>();
        UIManager.Instance.ShowPanel<MainPanel>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rigi.velocity = new Vector2(moveSpeed, rigi.velocity.y);
        if (rigi.velocity.y < -4)
            anim.SetBool("fall", true);
        else anim.SetBool("fall", false);
    }
}
