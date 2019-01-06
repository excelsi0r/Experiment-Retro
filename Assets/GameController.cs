using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public enum GameState
    {
        Playing,
        Pause
    }

    public GameState state;

    public bool left { get; private set; }
    public bool right { get; private set; }
    public bool up { get; private set; }
    public bool down { get; private set; }
    public bool crouch { get; private set; }
    public bool sprint { get; private set; }
    public bool jump { get; private set; }
    public bool attack { get; private set; }
    public bool interact { get; private set; }
    public bool grappling { get; private set; }
    public bool mouse { get; private set; }
    public Vector3 mouseCoords { get; private set; }

    public bool canSprint;
    public bool canGrapple;
    public bool canShoot;

    void Start()
    {
        state = GameState.Playing;
    }

    void Update()
    {
        if (state == GameState.Playing)
        {
            //True
            if (Input.GetKeyDown(KeyCode.A)) left = true;

            if (Input.GetKeyDown(KeyCode.D)) right = true;

            if (Input.GetKeyDown(KeyCode.W)) up = true;

            if (Input.GetKeyDown(KeyCode.S)) down = true;

            if (Input.GetKeyDown(KeyCode.LeftControl)) crouch = true;

            if (Input.GetKeyDown(KeyCode.LeftShift)) sprint = true;

            if (Input.GetKeyDown(KeyCode.F)) attack = true;

            if (Input.GetKeyDown(KeyCode.Space)) jump = true;

            if (Input.GetKeyDown(KeyCode.E)) interact = true;

            if (Input.GetMouseButtonDown(1)) grappling = true;

            if (Input.GetMouseButtonDown(0)) mouse = true;

            //False
            if (Input.GetKeyUp(KeyCode.A)) left = false;

            if (Input.GetKeyUp(KeyCode.D)) right = false;

            if (Input.GetKeyUp(KeyCode.W)) up = false;

            if (Input.GetKeyUp(KeyCode.S)) down = false;

            if (Input.GetKeyUp(KeyCode.LeftControl)) crouch = false;

            if (Input.GetKeyUp(KeyCode.LeftShift)) sprint = false;

            if (Input.GetKeyUp(KeyCode.F)) attack = false;

            if (Input.GetKeyUp(KeyCode.Space)) jump = false;

            if (Input.GetKeyUp(KeyCode.E)) interact = false;

            if (Input.GetMouseButtonUp(1)) grappling = false;

            if (Input.GetMouseButtonUp(0)) mouse = false;

            mouseCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //game variables
            if (!canSprint) sprint = false;

            if (!canShoot) attack = false;

            if (!canGrapple) grappling = false;
        }
        else
        {
            InvalidateControls();
        }
    }

    void InvalidateControls()
    {
        left = false;
        right = false;
        up = false;
        down = false;
        crouch = false;
        sprint = false;
        jump = false;
        attack = false;
        interact = false;
        grappling = false;
        mouse = false;
    }
}
