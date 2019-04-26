using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image ImgHealthBar;
    public Text TxtHealth;
    private float _percentage;
    public BattleCharacter c;

    private void Awake()
    {
        ImgHealthBar.type = Image.Type.Filled;
        ImgHealthBar.fillMethod = Image.FillMethod.Horizontal;
        ImgHealthBar.fillOrigin = (int)Image.OriginHorizontal.Left;
    }

    void Update()
    {
        transform.position = new Vector2(c.transform.position.x, c.transform.position.y + .5f);


        _percentage = (float)c.Character.CurrentHealth / (float)c.Character.MaxHealth;
        ImgHealthBar.fillAmount = _percentage;
        TxtHealth.text = c.Character.CurrentHealth.ToString() + "/" + c.Character.MaxHealth.ToString();

    }

}
