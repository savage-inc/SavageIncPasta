using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComfortBar : MonoBehaviour {

    public Image ImgComfortBar;
    public Text TxtComfort;
    private float _percentage;
    public BattleCharacter BattleCharacter;
    public Character Character;
    public bool FollowCharacter = true;

    // Use this for initialization
    void Start()
    {
        ImgComfortBar.type = Image.Type.Filled;
        ImgComfortBar.fillMethod = Image.FillMethod.Horizontal;
        ImgComfortBar.fillOrigin = (int)Image.OriginHorizontal.Left;
    }

    // Update is called once per frame
    void Update()
    {
        if (BattleCharacter != null)
        {
            _percentage = (float)BattleCharacter.Character.Comfort / (float)BattleCharacter.Character.MaxComfort;
            ImgComfortBar.fillAmount = _percentage;
            TxtComfort.text = BattleCharacter.Character.Comfort.ToString() + "/" + BattleCharacter.Character.MaxComfort.ToString();
        }
        else if (Character != null)
        {
            _percentage = (float)Character.Comfort / (float)Character.MaxComfort;
            ImgComfortBar.fillAmount = _percentage;
            TxtComfort.text = Character.Comfort.ToString() + "/" + Character.MaxComfort.ToString();
        }
    }
}
