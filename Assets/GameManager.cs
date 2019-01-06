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
        FadeIn();
    }

    public void FadeIn()
    {
        TransitionIn();
        bs.SetTrigger("FadeIn");
        Invoke("TransitionOut", 0.5f);
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


    /// <summary>
    /// Transition functions
    /// </summary>
    void TransitionIn()
    {
        gc.state = GameController.GameState.Transition;
    }

    void TransitionOut()
    {
        gc.state = GameController.GameState.Playing;
    }

}
