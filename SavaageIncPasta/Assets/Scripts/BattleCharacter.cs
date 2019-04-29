using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using System.Threading.Tasks;

public class Result<T>
{
    public T val;
}

public enum ActionChoice
{
    ePrimary,
    eSecondary
}

[RequireComponent(typeof(Sprite))]
public class BattleCharacter : MonoBehaviour {

    public Character Character;
    public ActionChoice CurrentAction = ActionChoice.ePrimary;
    public bool PrimaryAction = false;
    public bool SecondaryAction = false;
    public bool StartTurn = true;
    public int Initiative = 0;
    public float Defending = 1.0f;
    public int DamageTaken = 0;
    public int ClassModifier = 0;
    public int ChanceToHitModifier = 0;
    public bool SpikedBucatini = false;
    public float AttackBuffModifier = 1.0f;
    private bool _runningAnimation = false;
        
    private void Awake()
    {
        switch (Character.Class)
        {
            case ClassType.eWARRIOR:
                ClassModifier = Character.Strength + 2;
                break;
            case ClassType.eRANGER:
                ClassModifier = Character.Dexterity;
                break;
            case ClassType.eSHAMAN:
            case ClassType.eWIZARD:
                ClassModifier = Character.Intelligence;
                break;
            case ClassType.eENEMY:
                ClassModifier = Character.Strength;
                break;
        }

    }
    // Use this for initialization
    void Start () {
        GetComponent<SpriteRenderer>().sprite = FindObjectOfType<SpriteManager>().GetSprite(Character.SpritePreviewName);
    }

    // Update is called once per frame
    void Update () {
	}


    public IEnumerator MoveToAnimation(Vector2 target, float speed)
    {
        yield return StartCoroutine(MoveTo(target, speed));
    }

    public IEnumerator MissAnimation()
    {
        Vector2 start = transform.position;
        yield return StartCoroutine(MoveTo(start + Vector2.up * .25f, 5.0f ));
        yield return StartCoroutine(MoveTo(start, 5.0f));
    }

    public IEnumerator FireProjectile(GameObject projectile, Vector2 target, float speed)
    {
        //instantiate projectile if it has one
        if(projectile != null)
        {
            GameObject gameProjectile = Instantiate(projectile, new Vector3(transform.position.x,transform.position.y,0),Quaternion.identity);
            yield return StartCoroutine(MoveTo(gameProjectile.transform, target, speed));
            Destroy(gameProjectile);
        }
    }


    IEnumerator MoveTo(Vector2 target, float speed)
    {
        float t = 0.0f;
        Vector2 start = transform.position;
        Vector2 end = target;

        float distance = Vector3.Distance(start, end);
        while (distance >= 0.05f)
        {
            t += Time.deltaTime;
            transform.position = Vector2.Lerp(start, end, t * speed);
            distance = Vector2.Distance(transform.position, end);
            yield return null;
        }
    }

    IEnumerator MoveTo(Transform gameobject ,Vector2 target, float speed)
    {
        float t = 0.0f;
        Vector2 start = gameobject.position;
        Vector2 end = target;

        float distance = Vector3.Distance(start, end);
        while (distance >= 0.05f)
        {
            t += Time.deltaTime;
            gameobject.position = Vector2.Lerp(start, end, t * speed);
            distance = Vector2.Distance(gameobject.position, end);
            yield return null;
        }
    }
}
