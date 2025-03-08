using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Monster", menuName = "New Monster/monster")]
public class Monster : ScriptableObject
{
    public int level;
    public string title;
    public int maxHp;
    public int damage;
    public float speed;
    public List<Sprite> sprites;
    public int maxCount;

    public enum MonsterType
    {
        ZombieMelee_10001,
        ZombieMelee_10002,
        ZombieMelee_10003,
        ZombieMelee_10004,
    }
}
