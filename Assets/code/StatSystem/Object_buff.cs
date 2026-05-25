using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_buff : MonoBehaviour
{
    // Start is called before the first frame update
    public StatType buffType;
    public string buffName;
    public float changeValue; 
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Stat stat = collision.gameObject.GetComponent<Entity_stat>().GetStatByType(buffType);
            stat.AddBuffs(changeValue, buffName);
            //this.GetComponent<Collider2D>().enabled = false;
            //Destroy(this);
        }
    }
}
