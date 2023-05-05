using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class AbilityButton : MonoBehaviour
{
    [SerializeField]
    private Button abilityButton;

    [SerializeField]
    private Player player;

    [SerializeField]
    private AreaDamageAbility areaDamageAbility;

    

    public void Update()
    {
        switch(abilityButton.name)
        {
            case "Ability1":
                if(areaDamageAbility.GetCoolDown() <= 0)
                {
                    abilityButton.onClick.AddListener(() => player.SwitchState(Player.AbilityState.Ability1));
                }
                break;
            case "Ability2":
                abilityButton.onClick.AddListener(() => player.SwitchState(Player.AbilityState.Ability2));
                break;
            case "Ability3":
                abilityButton.onClick.AddListener(() => player.SwitchState(Player.AbilityState.Ability3));
                break;
        }
    }
}
