using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomItemGenerator
{
    public static List<string> WeaponAdjectives;
    public static List<string> ArmourAdjectives;

    public static void loadAdjvectives()
    {
        if(WeaponAdjectives == null)
        {
            WeaponAdjectives = new List<string>();
            TextAsset weaponAdjectives = (TextAsset)Resources.Load("Data/weaponAdjectives");
            WeaponAdjectives.AddRange(weaponAdjectives.text.Split('\n'));
        }

        if (ArmourAdjectives == null)
        {
            ArmourAdjectives = new List<string>();
            TextAsset armourAdjectives = (TextAsset)Resources.Load("Data/armourAdjectives");
            ArmourAdjectives.AddRange(armourAdjectives.text.Split('\n'));
        }
    }

    public static WeaponItemData RandomWeapon(SpriteManager spriteManager)
    {
        loadAdjvectives();

        string itemName = "";

        WeaponItemData weapon = ScriptableObject.CreateInstance<WeaponItemData>();
        //create guid
        weapon.DatabaseName = System.Guid.NewGuid().ToString();

        weapon.Rarity = (ItemRarity)Random.Range(0, 5);


        //decide what type of weapon to generate
        float randonValue = Random.value;
        if (randonValue <= .10f)
        {
            weapon.WeaponType = WeaponItemData.Type.eGUN;
        }
        else if (randonValue > .10f && randonValue <= .30f)
        {
            weapon.WeaponType = WeaponItemData.Type.eRANGE;
        }
        else
        {
            weapon.WeaponType = WeaponItemData.Type.eSWORD;
        }

        switch (weapon.WeaponType)
        {
            case WeaponItemData.Type.eSWORD:
                weapon.IsMelee = true;
                weapon.IsMainHand = true;


                //decide if the sword should use strength or agilty (rapier)
                if (Random.value <= .75f)
                {
                    weapon.StatType = WeaponItemData.StatTypes.eSTRENGTH;
                }
                else
                {
                    weapon.StatType = WeaponItemData.StatTypes.eAGILTITY;
                }

                //calculate random damage on weapon type and stat type and rarity
                if (weapon.StatType == WeaponItemData.StatTypes.eSTRENGTH)
                {
                    //decide if the weapon is a shortsword, longsword or a whip
                    randonValue = Random.value;
                    if (randonValue <= .33f) // shortsword
                    {
                        weapon.WeaponSubType = WeaponItemData.SubType.eSHORTSWORD;
                        weapon.BaseDamage = Random.Range(4, 7);
                        weapon.MinDamage = Mathf.Max(3, Random.Range(weapon.BaseDamage - 4, weapon.BaseDamage - 1));
                        weapon.MaxDamage = Random.Range(weapon.BaseDamage + 1, weapon.MaxDamage + 3);
                        weapon.VarianceDamage = Random.Range(2, 4);

                        itemName = "Penne";
                    }
                    else if (randonValue > .33f && randonValue <= .66f) // longsword
                    {
                        weapon.WeaponSubType = WeaponItemData.SubType.eLONGSWORD;
                        weapon.BaseDamage = Random.Range(6, 9);
                        weapon.MinDamage = Mathf.Max(4, Random.Range(weapon.BaseDamage - 4, weapon.BaseDamage - 1));
                        weapon.MaxDamage = Random.Range(weapon.BaseDamage + 1, weapon.MaxDamage + 3);
                        weapon.VarianceDamage = Random.Range(3, 5);

                        itemName = "Spaghetti";
                    }
                    else if (randonValue > .66f) // whip
                    {
                        weapon.WeaponSubType = WeaponItemData.SubType.eWHIP;
                        weapon.BaseDamage = Random.Range(3, 6);
                        weapon.MinDamage = Mathf.Max(1, Random.Range(weapon.BaseDamage - 4, weapon.BaseDamage - 1));
                        weapon.MaxDamage = Random.Range(weapon.BaseDamage + 1, weapon.MaxDamage + 3);
                        weapon.VarianceDamage = Random.Range(4, 7);

                        itemName = "Linguine";
                    }
                }
                else if (weapon.StatType == WeaponItemData.StatTypes.eAGILTITY)
                {
                    weapon.WeaponSubType = WeaponItemData.SubType.eRAPIER; // rapier
                    weapon.BaseDamage = Random.Range(3, 6);
                    weapon.MinDamage = Mathf.Max(2, Random.Range(weapon.BaseDamage - 4, weapon.BaseDamage - 1));
                    weapon.MaxDamage = Random.Range(weapon.BaseDamage + 1, weapon.MaxDamage + 3);
                    weapon.VarianceDamage = Random.Range(1, 3);

                    itemName = "Fedelini";
                }
                break;
            case WeaponItemData.Type.eRANGE:
                weapon.StatType = WeaponItemData.StatTypes.eAGILTITY;

                //decide if the weapon is a throwing star, bow
                randonValue = Random.value;
                if (randonValue <= .33f) //throwing star
                {
                    weapon.WeaponSubType = WeaponItemData.SubType.eTHROWINGSTAR;
                    weapon.BaseDamage = Random.Range(2, 5);
                    weapon.MinDamage = Mathf.Max(2, Random.Range(weapon.BaseDamage - 4, weapon.BaseDamage - 1));
                    weapon.MaxDamage = Random.Range(weapon.BaseDamage + 1, weapon.MaxDamage + 3);
                    weapon.VarianceDamage = Random.Range(1, 3);

                    itemName = "Conchiglie";
                }
                else // bow
                {
                    weapon.WeaponSubType = WeaponItemData.SubType.eBOW;
                    weapon.BaseDamage = Random.Range(3, 6);
                    weapon.MinDamage = Mathf.Max(2, Random.Range(weapon.BaseDamage - 4, weapon.BaseDamage - 1));
                    weapon.MaxDamage = Random.Range(weapon.BaseDamage + 1, weapon.MaxDamage + 3);
                    weapon.VarianceDamage = Random.Range(2, 4);

                    itemName = "Bucatini";
                }
                break;
            case WeaponItemData.Type.eGUN:
                weapon.StatType = WeaponItemData.StatTypes.eNONE;

                //decide if the weapon is a pistol, rifle
                randonValue = Random.value;
                if (randonValue <= .33f) //rifle
                {
                    weapon.WeaponSubType = WeaponItemData.SubType.eRIFLE;
                    weapon.BaseDamage = Random.Range(8, 14);
                    weapon.MinDamage = weapon.BaseDamage;
                    weapon.MaxDamage = weapon.BaseDamage;
                    weapon.VarianceDamage = 0;
                    weapon.MissFire = 10.0f;

                    itemName = "Fusilli Rifle";
                }
                else // pistol
                {
                    weapon.WeaponSubType = WeaponItemData.SubType.ePISTOL;
                    weapon.BaseDamage = Random.Range(4, 9);
                    weapon.MinDamage = weapon.BaseDamage;
                    weapon.MaxDamage = weapon.BaseDamage;
                    weapon.VarianceDamage = 0;
                    weapon.MissFire = 5.0f;

                    itemName = "Fusilli Pistol";
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        string magicName = string.Empty;
        //random magic 30 change the item has magic
        if (Random.value <= .45f)
        {
            weapon.MagicalType = (MagicType)Random.Range(1, 4);
            weapon.MagicalModifier = Random.Range(2, 5);

            switch (weapon.MagicalType)
            {
                case MagicType.eNONE:
                    break;
                case MagicType.eTOMATO:
                    magicName = "Tomato";
                    break;
                case MagicType.eCHEESE:
                    magicName = "Cheese";
                    break;
                case MagicType.ePESTO:
                    magicName = "Pesto";
                    break;
                default:
                    break;
            }
        }
        else
        {
            weapon.MagicalType = MagicType.eNONE;
        }

        //Random description
        weapon.Description = "TODO:: Add a random description";

        weapon.BaseMoneyValue = Random.Range(5, 30);

        weapon.PreviewSprite = spriteManager.GetRandomWeaponIcon(weapon.WeaponSubType);

        //select random name
        if (magicName != string.Empty)
        {
            weapon.Name = WeaponAdjectives[Random.Range(0, WeaponAdjectives.Count)] + " " + magicName + " " + itemName;
        }
        else
        {
            weapon.Name = WeaponAdjectives[Random.Range(0, WeaponAdjectives.Count)] + " " + itemName;
        }


        return weapon;
    }

    public static ArmourItemData RandomArmour(SpriteManager spriteManager)
    {
        loadAdjvectives();
        string itemName;
        ArmourItemData armourItem = ScriptableObject.CreateInstance<ArmourItemData>();

        armourItem.DatabaseName = System.Guid.NewGuid().ToString();


        //decide what type of armour to generate
        armourItem.ArmourSlotType = (ArmourItemData.SlotType)Random.Range(0, 3);
        armourItem.ArmourType = (ArmourItemData.Type)Random.Range(0, 3);

        string magicName = string.Empty;

        //random magic 30 change the item has magic
        if (Random.value <= .45f)
        {
            armourItem.MagicalType = (MagicType)Random.Range(1, 4);
            switch (armourItem.MagicalType)
            {
                case MagicType.eNONE:
                    break;
                case MagicType.eTOMATO:
                    magicName = "Tomato";
                    break;
                case MagicType.eCHEESE:
                    magicName = "Cheese";
                    break;
                case MagicType.ePESTO:
                    magicName = "Pesto";
                    break;
                default:
                    break;
            }
        }
        else
        {
            armourItem.MagicalType = MagicType.eNONE;
        }

        switch (armourItem.ArmourSlotType)
        {
            case ArmourItemData.SlotType.eHEAD:
                itemName = "Cicioneddos Helemt";
                switch (armourItem.ArmourType)
                {
                    case ArmourItemData.Type.eLIGHT: //light helmet
                        if (armourItem.MagicalType != MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(1, 5);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(1, 3);
                        }
                        break;
                    case ArmourItemData.Type.eMEDIUM: // medium helmet
                        if (armourItem.MagicalType != MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(2, 6);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(2, 4);
                        }
                        break;
                    case ArmourItemData.Type.eHEAVY: // heavy helmet
                        if (armourItem.MagicalType != MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(3, 7);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(3, 5);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            case ArmourItemData.SlotType.eCHEST:
                itemName = "Lasagna Chest";
                switch (armourItem.ArmourType)
                {
                    case ArmourItemData.Type.eLIGHT: //light chest
                        if (armourItem.MagicalType != MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(2, 6);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(2, 4);
                        }
                        break;
                    case ArmourItemData.Type.eMEDIUM: // medium chest
                        if (armourItem.MagicalType != MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(3, 7);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(3, 5);
                        }
                        break;
                    case ArmourItemData.Type.eHEAVY: // heavy chest
                        if (armourItem.MagicalType != MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(4, 8);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(4, 6);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            case ArmourItemData.SlotType.eLEGS:
                itemName = "Gnocchi Boots";
                switch (armourItem.ArmourType)
                {
                    case ArmourItemData.Type.eLIGHT: //light helmet
                        if (armourItem.MagicalType != MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(1, 5);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(1, 3);
                        }
                        break;
                    case ArmourItemData.Type.eMEDIUM: // medium helmet
                        if (armourItem.MagicalType != MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(2, 6);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(2, 4);
                        }
                        break;
                    case ArmourItemData.Type.eHEAVY: // heavy helmet
                        if (armourItem.MagicalType != MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(3, 7);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(3, 5);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        //Random description
        armourItem.Description = "TODO:: Add a random description";

        armourItem.BaseMoneyValue = Random.Range(5, 30);

        armourItem.PreviewSprite = spriteManager.GetRandomArmourIcon(armourItem.ArmourSlotType);

        //select random name
        if (magicName != string.Empty)
        {
            armourItem.Name = ArmourAdjectives[Random.Range(0, ArmourAdjectives.Count)] + " " + magicName + " " + itemName;
        }
        else
        {
            armourItem.Name = ArmourAdjectives[Random.Range(0, ArmourAdjectives.Count)] + " " + itemName;
        }


        return armourItem;
    }
}
