//using DG.Tweening;
using UnityEngine;
//using UnityEngine.UI;

public class Ability_Holder : MonoBehaviour
{
    //[SerializeField] private Image icon = default;
    //[SerializeField] private Image coolDownImage = default;

    // TESTING FUNCTION
    public Ability abilityNEW;

    // used variables from "Ability script"

    private float cooldownTime;
    private float activeTime;

    //[SerializeField] private SpriteRenderer CoolDownSprite, ReadySprite;

    [SerializeField] private GameObject ActiveSprite, CooldownSprite;

    // types of ability condition
    private enum Ability_State
    {
        ready,
        active,
        cooldown
    }

    // Ability state // INITIAL???

    private Ability_State state = Ability_State.ready;

    // Player INPUT

    // key input
    [SerializeField]
    private KeyCode key;

    private void Start()
    {
        PowerReady();
    }

    // Update is called once per frame
    private void Update()
    {
        FindAbilityStateInput(); //
    }

    /*
    public void SetIcon(Sprite s)
    {
        icon.sprite = s;
    }

    
    public void ShowUICooldown()
    {
        transform.DOComplete();
        coolDownImage.fillAmount = 0;
        coolDownImage.DOFillAmount(1, cooldownTime).SetEase(Ease.Linear).OnComplete(() => transform.DOPunchScale(Vector3.one / 10, .2f, 10, 1));
    }
    */
    private void FindAbilityStateInput()
    {
        switch (state)
        {
            // ready state
            case Ability_State.ready:
                if (Input.GetKeyDown(key))
                {
                    // change name
                    abilityNEW.Activate(gameObject);
                    activeTime = abilityNEW.activeTime;
                    state = Ability_State.active;

                    PowerReady();
                    //Debug.Log("Active: " + activeTime);
                }
                break;

            // activate state
            case Ability_State.active:

                if (activeTime > 0)
                {
                    activeTime -= Time.deltaTime;
                }
                else
                {
                    // call ability cooldown
                    abilityNEW.BeginCooldown(gameObject);
                    state = Ability_State.cooldown;
                    cooldownTime = abilityNEW.cooldownTime;

                    PowerDisabled();
                    //Debug.Log("Cooldown: " + cooldownTime);
                }

                break;

            case Ability_State.cooldown:

                if (cooldownTime > 0)
                {
                    cooldownTime -= Time.deltaTime;
                    PowerDisabled();
                }
                    
                else
                {
                    state = Ability_State.ready;
                    //cooldownTime = abilityNEW.cooldownTime;

                    PowerReady();
                }
                break;
        }
    }


    private void PowerReady()
    {
        ActiveSprite.SetActive(true);
        CooldownSprite.SetActive(false);
    }

    private void PowerDisabled()
    {
        ActiveSprite.SetActive(false);
        CooldownSprite.SetActive(true);
    }
    /*
    public void ShowCoolDown(float cooldown)
    {
        transform.DOComplete();
        coolDownImage.fillAmount = 0;
        coolDownImage.DOFillAmount(1, cooldown).SetEase(Ease.Linear).OnComplete(() => transform.DOPunchScale(Vector3.one / 10, .2f, 10, 1));
    }
    */
}