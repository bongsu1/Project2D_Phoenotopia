using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DataManager : Singleton<DataManager>
{
    [SerializeField] int maxHp;
    [SerializeField] int hp;
    [SerializeField] float maxStamina;
    [SerializeField] float stamina;

    public UnityAction OnHpChanged;

    public int MaxHp { get { return maxHp; } }
    public int Hp { get { return hp; } set { hp = value; OnHpChanged?.Invoke(); } }

    public float MaxStamina { get { return maxStamina; } }
    public float Stamina { get { return stamina; } set { stamina = value; } }

    Coroutine staminaRegenRoutine;
    public void StartStaminaRegenRoutine()
    {
        StopStaminaRegenRoutine();
        staminaRegenRoutine = StartCoroutine(StaminaRegenRoutine());
    }

    IEnumerator StaminaRegenRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        while (stamina < maxStamina)
        {
            stamina += Time.deltaTime;
            yield return null;
        }

        stamina = maxStamina;
        yield return null;
        staminaRegenRoutine = null;
    }

    public void StopStaminaRegenRoutine()
    {
        if (staminaRegenRoutine == null)
            return;

        StopCoroutine(staminaRegenRoutine);
        staminaRegenRoutine = null;
    }

    public void RefillStamina()
    {
        StopStaminaRegenRoutine();
        stamina = maxStamina;
    }

    /*private GameData gameData;
    public GameData GameData { get { return gameData; } }

#if UNITY_EDITOR
    private string path => Path.Combine(Application.dataPath, $"Resources/Data/SaveLoad");
#else
    private string path => Path.Combine(Application.persistentDataPath, $"Resources/Data/SaveLoad");
#endif

    public void NewData()
    {
        gameData = new GameData();
    }

    public void SaveData(int index = 0)
    {
        if (Directory.Exists(path) == false)
        {
            Directory.CreateDirectory(path);
        }

        string json = JsonUtility.ToJson(gameData, true);
        File.WriteAllText($"{path}/{index}.txt", json);
    }

    public void LoadData(int index = 0)
    {
        if (File.Exists($"{path}/{index}.txt") == false)
        {
            NewData();
            return;
        }

        string json = File.ReadAllText($"{path}/{index}.txt");
        try
        {
            gameData = JsonUtility.FromJson<GameData>(json);
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Load data fail : {ex.Message}");
            NewData();
        }
    }

    public bool ExistData(int index = 0)
    {
        return File.Exists($"{path}/{index}.txt");
    }*/
}
