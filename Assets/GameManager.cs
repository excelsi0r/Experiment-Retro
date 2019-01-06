using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public RawImage BlackScreen;
    public GameController gc;
    Animator bs;

    void Start()
    {
        bs = BlackScreen.GetComponent<Animator>();
        BlackScreen.gameObject.SetActive(true);
        bs.SetTrigger("FadeIn");
    }

    public void FadeId()
    {
        bs.SetTrigger("FadeIn");
    }

    public void FadeOut()
    {
        bs.SetTrigger("FadeOut");
    }

    public void FadeInOut()
    {
        TransitionIn();
        bs.SetTrigger("FadeInOut");
        Invoke("TransitionOut", 1f);
    }

    void TransitionIn()
    {
        gc.state = GameController.GameState.Transition;
    }

    void TransitionOut()
    {
        gc.state = GameController.GameState.Playing;
    }

}
