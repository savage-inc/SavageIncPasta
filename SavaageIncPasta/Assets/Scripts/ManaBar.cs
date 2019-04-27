using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Image ImgManaBar;
    public Text TxtMana;
    private float _percentage;
    public BattleCharacter c;

    // Use this for initialization
    void Start ()
    {
        ImgManaBar.type = Image.Type.Filled;
        ImgManaBar.fillMethod = Image.FillMethod.Horizontal;
        ImgManaBar.fillOrigin = (int)Image.OriginHorizontal.Left;
    }
	
	// Update is called once per frame
	void Update ()
    {
        ImgManaBar.transform.position = new Vector2(c.transform.position.x, c.transform.position.y + .5f);
        TxtMana.transform.position = new Vector2(c.transform.position.x, c.transform.position.y + .5f);

        _percentage = (float)c.Character.CurrentMana / (float)c.Character.MaxMana;
        ImgManaBar.fillAmount = _percentage;
        TxtMana.text = c.Character.CurrentMana.ToString() + "/" + c.Character.MaxMana.ToString();
    }
}
