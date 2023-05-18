using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingCircleUIBarrack : MonoBehaviour
{
    [SerializeField]
    private Barrack barrack;

    private float coolDown;
    private float readyTime;

    [SerializeField]
    private Image image;
    private float time;


    private void Awake()
    {
        barrack = GetComponentInParent<Barrack>();
        coolDown = barrack.GetCoolDown();
        image.fillAmount = 0;
    }

    private void Update()
    {

        if (barrack.GetState() == Barrack.State.Building)
        {
            time += Time.deltaTime;
            if (time >= coolDown)
            {
                barrack.SwitchState(Barrack.State.Ready);
                gameObject.SetActive(false);
            }
            image.fillAmount = time / coolDown;

        }

    }

    public void SetReadyTime(float time)
    {
        readyTime = time + coolDown;
    }
}
