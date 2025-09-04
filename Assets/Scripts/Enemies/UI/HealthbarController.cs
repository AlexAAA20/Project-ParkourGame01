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
    DummyEnemy alsoStatsLol;
    EnemyController emulator;
    public void Start ()
    {
        alsoStatsLol = transform.parent.GetComponent<DummyEnemy>( );
        emulator = transform.parent.GetComponent<EnemyController>( );
        Debug.Log( $"<color=green>yay you got it!" );
        stats.text = $"<color=#FFAA>{Mathf.Round( alsoStatsLol.speed / 20f )}x SPD " +
            $"<color=red>{Mathf.Round( alsoStatsLol.damageMulti * 1000 ) / 10}% DMG " +
            $"<color=green>{alsoStatsLol.maxHealth} HP";
        if ( alsoStatsLol.prop ) stealth.gameObject.SetActive( false );
    }

    // Update is called once per frame
    public void Update()
    {
        healthbar.fillAmount = alsoStatsLol.percent;
        healthbartext.text = Mathf.Round(alsoStatsLol.percent * 1000) / 10 + "%";
        if ( !alsoStatsLol.prop )
        {
            bool far = emulator.EmulateDetectEnemy( alsoStatsLol.sight );
            bool near = emulator.EmulateDetectEnemy( alsoStatsLol.crouchedSight );
            bool Rseen = emulator.inSight;

            if ( !Rseen )
            {
                stealth.sprite = unseen.sprite;
                stealth.color = unseen.color;
            }
            else
            {
                stealth.sprite = seen.sprite;
                stealth.color = seen.color;
            }
            if ( far && !near )
            {
                stealth.sprite = nearing.sprite;
                stealth.color = nearing.color;
            }
        }
    }
}
