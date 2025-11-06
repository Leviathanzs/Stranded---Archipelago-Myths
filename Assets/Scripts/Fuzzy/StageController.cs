using UnityEngine;  
public class StageController : MonoBehaviour
{
    private int killCount = 0;
    private StageTimer timer;
    private FuzzyStatGenerator fuzzy;
    private Character character;
    private FuzzyStats fuzzyStats; // ganti PlayerCharacter → FuzzyStats

    void Awake()
    {
        timer = GetComponent<StageTimer>();
        fuzzy = GetComponent<FuzzyStatGenerator>();
        fuzzyStats = FindObjectOfType<FuzzyStats>(); 
        character = FindObjectOfType<Character>();
        Debug.Log("StageController Awake");

    }

    void Start()
    {
        Debug.Log("StageController Start");
        OnStageStart();
    }


    public void OnStageStart()
    {
        killCount = 0;
        timer.StartTimer();
        Debug.Log("Stage timer started at: " + Time.time);

    }

    public void OnEnemyKilled()
    {
        killCount++;
    }

    public int GetKillCount()
    {
        return killCount;
    }

    public void OnStageEnd()
    {
        float completionTime = timer.StopTimer();
        FuzzyStatGenerator.Stats stat = fuzzy.GenerateStats(killCount, completionTime);

        if(character != null)
        {
            character.ApplyFuzzyStats(stat);
        }
        else
        {
            Debug.LogError("Character script tidak ditemukan di scene!");
        }
    }

}