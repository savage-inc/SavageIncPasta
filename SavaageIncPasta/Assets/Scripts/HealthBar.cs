using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image ImgHealthBar;
    public Text TxtHealth;
    private float _healthPercentage;
    public BattleCharacter BattleChracter;
    public Character Character;
    public bool FollowCharacter = true;

    private void Awake()
    {
        ImgHealthBar.type = Image.Type.Filled;
        ImgHealthBar.fillMethod = Image.FillMethod.Horizontal;
        ImgHealthBar.fillOrigin = (int)Image.OriginHorizontal.Left;
    }

    void Update()
    {
        if (BattleChracter != null)
        {
            transform.position = new Vector2(BattleChracter.transform.position.x, BattleChracter.transform.position.y + .6f);

            _healthPercentage = (float)BattleChracter.Character.CurrentHealth / (float)BattleChracter.Character.MaxHealth;
            ImgHealthBar.fillAmount = _healthPercentage;
            TxtHealth.text = BattleChracter.Character.CurrentHealth.ToString() + "/" + BattleChracter.Character.MaxHealth.ToString();
        }
        else if(Character != null)
        {
            _healthPercentage = (float)Character.CurrentHealth / (float)Character.MaxHealth;
            ImgHealthBar.fillAmount = _healthPercentage;
            TxtHealth.text = Character.CurrentHealth.ToString() + "/" + Character.MaxHealth.ToString();
        }
    }

}
