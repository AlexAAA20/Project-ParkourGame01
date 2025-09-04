using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarController : MonoBehaviour
{

    [Serializable]
    public class ComplexSprite
    {
        public Sprite sprite;
        public Color color;
    }

    public Image healthbar;
    public TMP_Text healthbartext;
    public TMP_Text stats;
    public Image stealth;
    public ComplexSprite unseen;
    public ComplexSprite nearing;
    public ComplexSprite seen;
    public DummyEnemy alsoStatsLol;
    public EnemyController emulator;
    public void Start ()
    {
        Debug.Log( $"<color=green>yay you got it!" );
        stats.text = $"<color=#FFAA>{Mathf.Round( alsoStatsLol.speed / 20f )}u " +
            $"<color=red>{Mathf.Round( alsoStatsLol.damageMulti * 1000 ) / 10}% " +
            $"<color=green>{alsoStatsLol.maxHealth}HP";
        if ( alsoStatsLol.prop ) stealth.gameObject.SetActive( false );
    }

    public void Update()
    {
        healthbar.fillAmount = alsoStatsLol.percent;
        healthbartext.text = Mathf.Round(alsoStatsLol.percent * 1000) / 10 + "%";
        if ( !alsoStatsLol.prop )
        {
            bool far = emulator.EmulateDetectEnemy( alsoStatsLol.sight );
            bool near = emulator.EmulateDetectEnemy( alsoStatsLol.crouchedSight );
            bool Rseen = emulator.inSight;

            if ( Rseen )
            {
                stealth.sprite = seen.sprite;
                stealth.color = seen.color;
            }
            else if ( far && !near )
            {
                stealth.sprite = nearing.sprite;
                stealth.color = nearing.color;
            }
            else
            {
                stealth.sprite = unseen.sprite;
                stealth.color = unseen.color;
            }
        }
    }
}
