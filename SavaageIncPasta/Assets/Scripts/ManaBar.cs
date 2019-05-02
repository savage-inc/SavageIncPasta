using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public Image ImgManaBar;
    public Text TxtMana;
    private float _percentage;
    public BattleCharacter BattleCharacter;
    public Character Character;
    public bool FollowCharacter = true;

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
        if (BattleCharacter != null)
        {
            _percentage = (float)BattleCharacter.Character.CurrentMana / (float)BattleCharacter.Character.MaxMana;
            ImgManaBar.fillAmount = _percentage;
            TxtMana.text = BattleCharacter.Character.CurrentMana.ToString() + "/" + BattleCharacter.Character.MaxMana.ToString();
        }
        else if (Character != null)
        {
            _percentage = (float)Character.CurrentMana / (float)Character.MaxMana;
            ImgManaBar.fillAmount = _percentage;
            TxtMana.text = Character.CurrentMana.ToString() + "/" + Character.MaxMana.ToString();
        }
    }
}
