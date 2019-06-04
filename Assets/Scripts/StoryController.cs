using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryController : MonoBehaviour {
    public SQLiteController SQLCtrl;

    public RawImage Image1, Image2, Image3, Largeimage;
    public Button ChoiceB1, ChoiceB2, ChoiceB3;
    public Text ChoiceT1, ChoiceT2, ChoiceT3;
    public RawImage ChoiceI1, ChoiceI2, ChoiceI3;
    public Text Text;
    public AudioSource Audio;
    public InputField ID;

    private List<RawImage> images;
    private int CorrectAnswer;

    public void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "TextAndImage")
        {
            RenderTextandImageScreen();
        }
        else if (scene.name == "Text")
        {
            RenderTextScreen();
        }
        else if (scene.name == "Question")
        {
            RenderQuestionScreen();
        }
    }

    public void next()
    {
        int currentScreen = CurrentUser.GetGameScreen();
        CurrentUser.SetGameScreen(currentScreen + 1);
        changeStoryScene();
    }

    public void isStartedBefore()
    {
        if (CurrentUser.GetGameScreen() != 1)
        {
            SceneManager.LoadScene("ContinueGame");
            return;
        }
        CurrentUser.SetGameScreen(1);
        changeStoryScene();
    }

    public void changeStoryScene()
    {
        int previousType = CurrentUser.ScreenType;
        int type = SQLCtrl.GetScreenType(CurrentUser.GetGameScreen());
        CurrentUser.ScreenType = type;
        if (type == 0)
        {
            CurrentUser.SetGameScreen(1);
            SceneManager.LoadScene("Main", LoadSceneMode.Single);
        }
        else if (type == (int)storyTypes.TextAndImage)
        {
            SceneManager.LoadScene("TextAndImage", LoadSceneMode.Single);
        }
        else if (type == (int)storyTypes.Text)
        {
            SceneManager.LoadScene("Text", LoadSceneMode.Single);
        }
        else if (type == (int)storyTypes.Camera)
        {
            SceneManager.LoadScene("Camera", LoadSceneMode.Single);
        }
        else if (type == (int)storyTypes.Navigation)
        {
            SceneManager.LoadScene("Navigation", LoadSceneMode.Single);
        }
        else if (type == (int)storyTypes.Question)
        {
            SceneManager.LoadScene("Question", LoadSceneMode.Single);
        }
        else if (type == (int)storyTypes.GoToMedal)
        {
            int nextID = SQLCtrl.NextID("medals");
            SQLCtrl.SaveMedal(nextID, CurrentUser.ID, "Detektiv", "spy.png");
            Input.location.Stop();
            SceneManager.LoadScene("Medals", LoadSceneMode.Single);
        }
    }

    public void ChangeToBegingOfStoryScene()
    {
        CurrentUser.SetGameScreen(1);
        changeStoryScene();
    }

    public void GoToID()
    {
        CurrentUser.SetGameScreen(int.Parse(ID.text));
        changeStoryScene();
    }

    public void RenderTextandImageScreen()
    {
        CurrentUser.CanNext = true;
        int screenId = CurrentUser.GetGameScreen();

        string text;
        List<string> pictures;
        string largePicture;
        string sound;

        if (!SQLCtrl.GetTextAndImageScreen(screenId, out pictures, out largePicture, out sound, out text))
        {
            Debug.Log("Unable to get Screen info");
        }

        images = new List<RawImage>() { Image1, Image2};

        Text.text = ToNorwegianCaracthers(text);

        for (int i = 0; i<pictures.Count; i++)
        {
            images[i].texture = LoadPicture(pictures[i]);
        }
        for (int i = 1; i >= pictures.Count; i--)
        {
            images[i].enabled = false;
        }
        Largeimage.texture = LoadPicture(largePicture);
        Debug.Log("mastergame sound " + sound);
        Audio.clip = LoadAudio(sound);
    }

    public void RenderTextScreen()
    {
        CurrentUser.CanNext = true;
        int screenId = CurrentUser.GetGameScreen();

        string text;
        List<string> pictures;
        string sound;

        if (!SQLCtrl.GetTextScreen(screenId, out pictures, out sound, out text))
        {
            Debug.Log("Unable to get Screen info");
        }

        images = new List<RawImage>() { Image1, Image2, Image3 };

        Text.text = ToNorwegianCaracthers(text);

        for (int i = 0; i < pictures.Count; i++)
        {
            images[i].texture = LoadPicture(pictures[i]);
        }
        for (int i = 2; i >= pictures.Count; i--)
        {
            images[i].enabled = false;
        }
        Audio.clip = LoadAudio(sound);
    }

    public void RenderQuestionScreen()
    {
        Debug.Log("mastergame render question screen");
        CurrentUser.CanNext = false;
        int screenId = CurrentUser.GetGameScreen();

        string text;
        int correctAnswer;
        List<string> choices;
        List<string> choicesImages;
        string sound;

        Image1.enabled = false;
        Image2.enabled = false;
        Image3.enabled = false;

        if (!SQLCtrl.GetQuestionScreen(screenId, out choices, out correctAnswer, out text, out choicesImages, out sound))
        {
            Debug.Log("Unable to get Screen info");
        }

        List<Button> choicesButtons = new List<Button>() { ChoiceB1, ChoiceB2, ChoiceB3 };
        List<Text> choicesTexts = new List<Text>() { ChoiceT1, ChoiceT2, ChoiceT3 };
        List<RawImage> choicesRawImages = new List<RawImage> { ChoiceI1, ChoiceI2, ChoiceI3 };

        Text.text = ToNorwegianCaracthers(text);

        for (int i = 0; i < choices.Count; i++)
        {
            choicesTexts[i].text = ToNorwegianCaracthers(choices[i]);
            RawImage image = choicesRawImages[i];
            Debug.Log("mastergame setting in question " + choicesImages[i]);
            image.texture = LoadPicture(choicesImages[i]);
        }
        for (int i = 2; i >= choices.Count; i--)
        {
            choicesButtons[i].enabled = false;
            Destroy(choicesButtons[i].gameObject);
            choicesRawImages[i].enabled = false;
        }
        CorrectAnswer = correctAnswer;
        Audio.clip = LoadAudio(sound);
    }

    public void checkAnswer()
    {
        if (CorrectAnswer == 4)
        {
            //all answers are correct
            Image1.enabled = true;
            Image2.enabled = true;
            Image3.enabled = true;
        }
        if (CorrectAnswer == 1)
        {
            Image1.enabled = true;
        }
        if (CorrectAnswer == 2)
        {
            Image2.enabled = true;
        }
        if (CorrectAnswer == 3)
        {
            Image3.enabled = true;
        }
        CurrentUser.CanNext = true;
    }

    public Texture LoadPicture(string absoluteImagePath)
    {
        string finalPath;
        WWW localFile;

        finalPath = "jar:file://" + Application.dataPath + "!/assets/" + absoluteImagePath;
        localFile = new WWW(finalPath);
        while (!localFile.isDone) { }

        return localFile.texture;
    }

    public AudioClip LoadAudio(string absolutePath)
    {
        string finalPath;
        WWW localFile;

        finalPath = "jar:file://" + Application.dataPath + "!/assets/" + absolutePath;
        localFile = new WWW(finalPath);
        while (!localFile.isDone) { }

        return localFile.audioClip;
    }

    private string ToNorwegianCaracthers(string text)
    {
        text = text.Replace("aa", "å");
        text = text.Replace("Aa", "Å");
        text = text.Replace("oe", "ø");
        text = text.Replace("Oe", "Ø");
        text = text.Replace("ae", "æ");
        text = text.Replace("Ae", "Æ");
        return text;
    }
}
