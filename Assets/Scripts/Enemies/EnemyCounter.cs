using UnityEngine;

public class CountStatus
{
    public int counted;
    public int maximum;
    public float percentage;
    public float invPercentage;
    public string code = "AAAA";
}
public class EnemyCounter : MonoBehaviour
{
    public static CountStatus enemies = new();
    public void Start ( )
    {
        enemies.maximum = transform.childCount;
    }

    public void Update ( )
    {
        enemies.counted = transform.childCount;
        enemies.percentage = ( float ) enemies.counted / enemies.maximum;
        enemies.invPercentage = 1f - ( float ) enemies.counted / enemies.maximum;
    }

}
