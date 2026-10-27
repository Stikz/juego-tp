using UnityEngine.InputSystem;
using static PauseManager;

// ACA SE APLICA EL PATRON FACADE (una sola entrada para pausa, cursor e input)
public static class GameFlowFacade
{
    public static void StopGameplay()
    {
        GameState.Paused = true;
        CursorMode.SetUI();
        SetPlayerInput(false);
    }

    public static void ResumeGameplay()
    {
        GameState.Paused = false;
        CursorMode.SetGameplay();
        SetPlayerInput(true);
    }

    public static void PrepareSceneChange()
    {
        GameState.Paused = false;
        SetPlayerInput(true);
    }

    private static void SetPlayerInput(bool active)
    {
        if (PlayerInput.all.Count == 0) return;

        if (active)
            PlayerInput.all[0].ActivateInput();
        else
            PlayerInput.all[0].DeactivateInput();
    }
}
