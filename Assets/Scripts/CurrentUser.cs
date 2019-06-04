using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CurrentUser  {
    private static int id, gender;
    private static string username;
    private static int currentGames=1;
    private static List<int> finishedGames;
    private static string currentPicture;
    private static bool canNext;
    private static int screenType = 0;

    public static int ID {
        set
        {
            id = value;
        }
        get
        {
            return id;
        }
    }
    public static int Gender {
        set
        {
            gender = value;
        }
        get
        {
            return gender;
        }
    }
    public static string Username
    {
        set
        {
            username = value;
        }
        get
        {
            return username;
        }
    }

    public static string CurrentPicture
    {
        set
        {
            currentPicture = value;
        }
        get
        {
            return currentPicture;
        }
    }

    public static bool CanNext
    {
        set
        {
            canNext = value;
        }
        get
        {
            return canNext;
        }
    }

    public static void SetGameScreen(int gameScreen)
    {
        currentGames = gameScreen;
    }

    public static int GetGameScreen()
    {
        return currentGames;
        /*if (currentGames.ContainsKey(gameID))
        {
            return currentGames[gameID];
        }
        return 0; */
    }

    public static int ScreenType
    {
        set
        {
            screenType = value;
        }
        get
        {
            return screenType;
        }
    }
}
