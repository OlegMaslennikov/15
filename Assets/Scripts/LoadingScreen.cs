using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    private string SAVE_FOLDER;
    private class SaveObject
    {
        public int time;
        public float moves;
        public int games;
    }
    public class Achievements
    {
        public bool moves100;
        public bool time120;
        public bool moves150;
        public bool time180;

    }

    static Achievements playersAchievements = new Achievements
    {
        moves100 = false,
        time120 = false,
        moves150 = false,
        time180 = false
    };

    public void Update()
    {
        SAVE_FOLDER = Application.persistentDataPath + "/Saves";

        if (!Directory.Exists(SAVE_FOLDER))
            Directory.CreateDirectory(SAVE_FOLDER);

        if (Application.persistentDataPath + "/Saves" != null)
            infoText.text = "Загружаем игру";

        if (!File.Exists(Application.persistentDataPath + "/Saves/save.txt"))
        {
            File.Create(Application.persistentDataPath + "/Saves/save.txt");
            StartCoroutine("WaitforCreatingSaveFile");
        }

        if (Application.persistentDataPath + "/Saves/save.txt" != null)
        {
            infoText.text = "Файл для сохранений существует";
        }

        if (!File.Exists(Application.persistentDataPath + "/Saves/achievements.txt"))
        {
            File.Create(Application.persistentDataPath + "/Saves/achievements.txt");
            StartCoroutine("WaitforCreatingAchievements");
        }

        if (Application.persistentDataPath + "/Saves/achievements.txt" != null)
        {
            infoText.text = "Подождите, идет загрузка";
        }

        if (Application.persistentDataPath + "/Saves" != null && Application.persistentDataPath + "/Saves/save.txt" != null && Application.persistentDataPath + "/Saves/achievements.txt" != null)
            StartCoroutine("WaitforLoading");
    }

    IEnumerator WaitforLoading()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Main Menu");
    }

    IEnumerator WaitforCreatingSaveFile()
    {
        infoText.text = "Осталось немного, загружаем игру";
        yield return new WaitForSeconds(5);
        SaveObject saveObject = new SaveObject
        {
            time = 0,
            moves = 0,
            games = 0,
        };
        string json = JsonUtility.ToJson(saveObject);
        File.WriteAllText(SAVE_FOLDER + "/save.txt", json);
    }

    IEnumerator WaitforCreatingAchievements()
    {
        infoText.text = "Создаем файлы игры";
        yield return new WaitForSeconds(5);
        string json = JsonUtility.ToJson(playersAchievements);
        File.WriteAllText(Application.persistentDataPath + "/Saves" + "/achievements.txt", json);
    }

}
