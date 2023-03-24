using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using System;
using System.Security.Cryptography;

public class CinematicManager : MonoBehaviour
{
    public Transform gameCamera;
    public Transform[] cameraPositions;
    public Transform[] characterPositions;
    public GameObject[] Characters;
    public Transform player;
    public Transform instructionsText;

    public static bool playingCinematic;
    public enum CinematicCommandId
    {
        enterCinematicMode,
        exitCinematicMode,
        wait,
        log,
        showDialog,
        setCameraPosition,
        setCameraSize,
        cameraShake,
        setObjectActive,
        setObjectPosition,
        setPlayerFacing,
        setPlayerVelocity
    };

    [System.Serializable]
    public struct CinematicCommand
    {
        public CinematicCommandId id;
        public string param1;
        public string param2;
        public string param3;
        public string param4;
    };

    [System.Serializable]
    public struct CinematicSequence
    {
        public string name;
        public CinematicCommand[] commands;
    }

    [Header("Cinematic system")]
    public CinematicSequence[] sequences;

    [Header("Dialog system")]
    public Transform[] dialogCommon;
    public Transform[] dialogCharacters;
    public Transform dialogText;


    [System.Serializable]
    public struct DialogData
    {
        public int character;
        public string text;

    };

    // Cinematic system

    int sequenceIndex;
    int commandIndex;

    bool waiting;

    float waitTimer;

    // Dialogs system

    public bool isCinematicMode;

    public DialogData[] dialogsData;

    bool showingDialog;

    TextMeshProUGUI dialogTextC;

    int dialogIndex;

    PlayerMovementController P1;

    PlayerController PC;

    KeyCode[] debugKey = { KeyCode.S, KeyCode.T, KeyCode.A, KeyCode.R };
    int debugKeyProgress = 0;

    GameCamera gameCameraC;

    // Start is called before the first frame update
    void Start()
    {
        P1 = player.GetComponent<PlayerMovementController>();

        PC = player.GetComponent<PlayerController>();

        // Init state
        isCinematicMode = false;
        waiting = false;

        // Init dialog system
        showingDialog = false;
        dialogIndex = 0;

        dialogTextC = dialogText.GetComponent<TextMeshProUGUI>();

        gameCameraC = gameCamera.GetComponent<GameCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isCinematicMode)
        {
            playingCinematic = true;

            PC.playerState = PlayerController.PlayerStates.INTERACTING;

            if (showingDialog)
            {
                for (int i = 0; i < dialogCommon.Length; i++) { dialogCommon[i].gameObject.SetActive(true); }
                for (int i = 0; i < dialogCharacters.Length; i++) { dialogCharacters[i].gameObject.SetActive(false); }

                int character = dialogsData[dialogIndex].character;
                string text = dialogsData[dialogIndex].text;

                dialogCharacters[character].gameObject.SetActive(true);
                dialogTextC.text = text;

                instructionsText.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Return))
                {
                    showingDialog = false;

                    for (int i = 0; i < dialogCommon.Length; i++) { dialogCommon[i].gameObject.SetActive(false); }
                    for (int i = 0; i < dialogCharacters.Length; i++) { dialogCharacters[i].gameObject.SetActive(false); }
                    commandIndex++;

                    instructionsText.gameObject.SetActive(false);
                }

            }

            else if (waiting)
            {
                if (waitTimer <= 0)
                {
                    waiting = false;
                    commandIndex++;
                }
                else
                {
                    waitTimer -= Time.deltaTime;
                }
            }
            else if (commandIndex <= sequences[sequenceIndex].commands.Length)
            {
                CinematicCommand command = sequences[sequenceIndex].commands[commandIndex];

                if (command.id == CinematicCommandId.enterCinematicMode)
                {
                    gameCameraC.gameObject.SetActive(true);
                    gameCameraC.EnterCinematicMode();
                    isCinematicMode = true;
                }
                else if (command.id == CinematicCommandId.exitCinematicMode)
                {
                    gameCameraC.gameObject.SetActive(false);
                    gameCameraC.ExitCinematicMode();
                    isCinematicMode = false;
                    playingCinematic = false;
                }
                else if (command.id == CinematicCommandId.wait)
                {
                    float time = Single.Parse(command.param1);
                    waiting = true;
                    waitTimer = time;
                }
                else if (command.id == CinematicCommandId.log)
                {
                    string message = command.param1;
                    Debug.Log("Cinematic log" + message);
                }
                else if (command.id == CinematicCommandId.showDialog)
                {
                    int index = Int32.Parse(command.param1);
                    showingDialog = true;
                    dialogIndex = index;
                }
                else if (command.id == CinematicCommandId.setCameraPosition)
                {
                    int index = Int32.Parse(command.param1);

                    gameCamera.position = cameraPositions[index].position;
                    gameCamera.rotation = cameraPositions[index].rotation;
                }
                else if (command.id == CinematicCommandId.setCameraSize)
                {
                    float size = Single.Parse(command.param1);

                    gameCameraC.SetSize(size);
                }
                else if (command.id == CinematicCommandId.cameraShake)
                {
                    float duracion = Single.Parse(command.param1);
                    float amplitud = Single.Parse(command.param2);

                    for (int i = 0; i < duracion; i++)
                    {
                        gameCameraC.cameraShake(duracion, amplitud);
                    }
                }
                else if (command.id == CinematicCommandId.setObjectActive)
                {
                    int objectIndex = Int32.Parse(command.param1);
                    bool active = Boolean.Parse(command.param2);

                    Characters[objectIndex].SetActive(active);
                }
                else if (command.id == CinematicCommandId.setObjectPosition)
                {
                    int objectIndex = Int32.Parse(command.param1);
                    int positionIndex = Int32.Parse(command.param2);

                    Characters[objectIndex].transform.position = characterPositions[positionIndex].position;
                }
                else if (command.id == CinematicCommandId.setPlayerFacing)
                {
                    int facing = Int32.Parse(command.param1);

                    if (facing == 0)
                    {
                        player.transform.rotation = Quaternion.Euler(0, 0, 0);
                    }
                    else if (facing == 1)
                    {
                        player.transform.rotation = Quaternion.Euler(0, 180, 0);
                    }
                }
                else if (command.id == CinematicCommandId.setPlayerVelocity)
                {
                    int speed = Int32.Parse(command.param1);

                    //P1.speed = speed;
                }
                else
                {
                    Debug.Log("Comando de cinematica no implementado");
                }

                if (!waiting && !showingDialog)
                {
                    commandIndex++;
                }
            }
        }
        else
        {
            PC.playerState = PlayerController.PlayerStates.NONE;
            playingCinematic = false;
            isCinematicMode = false;
        }
    }

    public void OnTriggerCinematic(int index)
    {
        if (!isCinematicMode)
        {
            isCinematicMode = true;
            sequenceIndex = index;
            commandIndex = 0;
        }
        else
        {
            Debug.Log("No puede iniciarse la cinematica" + index + "porque ya hay una en ejecucion");
        }
    }

    public bool IsShowingDialog()
    {
        return showingDialog;
    }

    public bool IsCinematicMode()
    {
        return isCinematicMode;
    }
}
