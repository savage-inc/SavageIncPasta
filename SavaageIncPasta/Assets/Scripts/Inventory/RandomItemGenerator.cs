using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomItemGenerator
{
    public static WeaponItemData RandomWeapon()
    {
        WeaponItemData weapon = ScriptableObject.CreateInstance<WeaponItemData>();
        //create guid
        weapon.DatabaseName = System.Guid.NewGuid().ToString();

        weapon.Rarity = (ItemRarity)Random.Range(0, 5);


        //decide what type of weapon to generate
        float randonValue = Random.value;
        if (randonValue <= .20f)
        {
            weapon.WeaponType = WeaponItemData.Type.eGUN;
        }
        else if (randonValue > .20f && randonValue <= 50.0f)
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
                    }
                    else if (randonValue > .33f && randonValue <= .66f) // longsword
                    {
                        weapon.WeaponSubType = WeaponItemData.SubType.eLONGSWORD;
                        weapon.BaseDamage = Random.Range(6, 9);
                        weapon.MinDamage = Mathf.Max(4, Random.Range(weapon.BaseDamage - 4, weapon.BaseDamage - 1));
                        weapon.MaxDamage = Random.Range(weapon.BaseDamage + 1, weapon.MaxDamage + 3);
                        weapon.VarianceDamage = Random.Range(3, 5);
                    }
                    else if (randonValue > .66f) // whip
                    {
                        weapon.WeaponSubType = WeaponItemData.SubType.eWHIP;
                        weapon.BaseDamage = Random.Range(3, 6);
                        weapon.MinDamage = Mathf.Max(1, Random.Range(weapon.BaseDamage - 4, weapon.BaseDamage - 1));
                        weapon.MaxDamage = Random.Range(weapon.BaseDamage + 1, weapon.MaxDamage + 3);
                        weapon.VarianceDamage = Random.Range(4, 7);
                    }
                }
                else if (weapon.StatType == WeaponItemData.StatTypes.eAGILTITY)
                {
                    weapon.WeaponSubType = WeaponItemData.SubType.eRAPIER; // rapier
                    weapon.BaseDamage = Random.Range(3, 6);
                    weapon.MinDamage = Mathf.Max(2, Random.Range(weapon.BaseDamage - 4, weapon.BaseDamage - 1));
                    weapon.MaxDamage = Random.Range(weapon.BaseDamage + 1, weapon.MaxDamage + 3);
                    weapon.VarianceDamage = Random.Range(1, 3);
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
                }
                else // bow
                {
                    weapon.WeaponSubType = WeaponItemData.SubType.eBOW;
                    weapon.BaseDamage = Random.Range(3, 6);
                    weapon.MinDamage = Mathf.Max(2, Random.Range(weapon.BaseDamage - 4, weapon.BaseDamage - 1));
                    weapon.MaxDamage = Random.Range(weapon.BaseDamage + 1, weapon.MaxDamage + 3);
                    weapon.VarianceDamage = Random.Range(2, 4);
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
                }
                else // pistol
                {
                    weapon.WeaponSubType = WeaponItemData.SubType.ePISTOL;
                    weapon.BaseDamage = Random.Range(4, 9);
                    weapon.MinDamage = weapon.BaseDamage;
                    weapon.MaxDamage = weapon.BaseDamage;
                    weapon.VarianceDamage = 0;
                    weapon.MissFire = 5.0f;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        //random magic 30 change the item has magic
        if (Random.value <= .3f)
        {
            weapon.MagicalType = (WeaponItemData.MagicType) Random.Range(1, 4);
            weapon.MagicalModifier = Random.Range(2, 5);
        }
        else
        {
            weapon.MagicalType = WeaponItemData.MagicType.eNONE;
        }

        //weapon.PreviewSprite =
        //Random Name depending on the stats and weapon type
        weapon.Name = "Random Weapon #" + Random.Range(0, 1000);
        //Random description
        weapon.Description = "TODO:: Add a random description";

        return weapon;
    }

    public static ArmourItemData RandomArmour()
    {
        ArmourItemData armourItem = ScriptableObject.CreateInstance<ArmourItemData>();

        armourItem.DatabaseName = System.Guid.NewGuid().ToString();

        //decide what type of armour to generate
        armourItem.ArmourSlotType = (ArmourItemData.SlotType) Random.Range(0, 3);
        armourItem.ArmourType = (ArmourItemData.Type)Random.Range(0, 3);


        //random magic 30 change the item has magic
        if (Random.value <= .3f)
        {
            armourItem.MagicalType = (ArmourItemData.MagicType)Random.Range(1, 4);
        }
        else
        {
            armourItem.MagicalType = ArmourItemData.MagicType.eNONE;
        }

        switch (armourItem.ArmourSlotType)
        {
            case ArmourItemData.SlotType.eHEAD:
                switch (armourItem.ArmourType)
                {
                    case ArmourItemData.Type.eLIGHT: //light helmet
                        if (armourItem.MagicalType != ArmourItemData.MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(1, 5);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(1, 3);
                        }
                        break;
                    case ArmourItemData.Type.eMEDIUM: // medium helmet
                        if (armourItem.MagicalType != ArmourItemData.MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(2, 6);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(2, 4);
                        }
                        break;
                    case ArmourItemData.Type.eHEAVY: // heavy helmet
                        if (armourItem.MagicalType != ArmourItemData.MagicType.eNONE)
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
                switch (armourItem.ArmourType)
                {
                    case ArmourItemData.Type.eLIGHT: //light chest
                        if (armourItem.MagicalType != ArmourItemData.MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(2, 6);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(2, 4);
                        }
                        break;
                    case ArmourItemData.Type.eMEDIUM: // medium chest
                        if (armourItem.MagicalType != ArmourItemData.MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(3, 7);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(3, 5);
                        }
                        break;
                    case ArmourItemData.Type.eHEAVY: // heavy chest
                        if (armourItem.MagicalType != ArmourItemData.MagicType.eNONE)
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
                switch (armourItem.ArmourType)
                {
                    case ArmourItemData.Type.eLIGHT: //light helmet
                        if (armourItem.MagicalType != ArmourItemData.MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(1, 5);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(1, 3);
                        }
                        break;
                    case ArmourItemData.Type.eMEDIUM: // medium helmet
                        if (armourItem.MagicalType != ArmourItemData.MagicType.eNONE)
                        {
                            armourItem.Value = Random.Range(2, 6);
                        }
                        else
                        {
                            armourItem.Value = Random.Range(2, 4);
                        }
                        break;
                    case ArmourItemData.Type.eHEAVY: // heavy helmet
                        if (armourItem.MagicalType != ArmourItemData.MagicType.eNONE)
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
        return armourItem;
    }
}
