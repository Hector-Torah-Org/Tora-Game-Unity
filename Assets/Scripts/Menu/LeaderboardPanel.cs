using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class LeaderboardPanel : MonoBehaviour
{
    [SerializeField] private ApiConnection apiConnection;
    [SerializeField] private GameObject AmountLeaderboardContent;
    [SerializeField] private GameObject ConfidenceLeaderboardContent;
    [SerializeField] private LeaderboardElement LeaderboardEntryPrefab;

    // Dictionaries containing page as key and list of entries as value
    private Dictionary<int, List<LeaderboardDTO.LeaderboardElementDTO>> amountEntries = new Dictionary<int, List<LeaderboardDTO.LeaderboardElementDTO>>();
    private Dictionary<int, List<LeaderboardDTO.LeaderboardElementDTO>> confidenceEntries = new Dictionary<int, List<LeaderboardDTO.LeaderboardElementDTO>>();

    int amountPage = 0;
    int confidencePage = 0;
    public int pagesize = 100;

    int playerPositionAmount;
    int playerPositionConfidence;

    List<LeaderboardElement> currentAmountEntryObjects = new List<LeaderboardElement>();
    List<LeaderboardElement> currentConfidenceEntryObjects = new List<LeaderboardElement>();

    public IEnumerator ReloadLeaderboard()
    {
        //===========Loading data if not already loaded===========//
        if (!amountEntries.ContainsKey(amountPage))
        {
            yield return StartCoroutine(apiConnection.GetLeaderboard(amountPage, pagesize, "amount", (leaderboardPage) => {
                amountEntries[amountPage] = leaderboardPage.leaderboardElementDTOS.ToList();
            }));
        }
        if (!confidenceEntries.ContainsKey(confidencePage))
        {
            yield return StartCoroutine(apiConnection.GetLeaderboard(confidencePage, pagesize, "confidence", (leaderboardPage) => {
                confidenceEntries[confidencePage] = leaderboardPage.leaderboardElementDTOS.ToList();
            }));
        }

        //===========Destroying old entries===========//
        foreach (LeaderboardElement entry in currentAmountEntryObjects)
        {
            Destroy(entry.gameObject);
        }
        currentAmountEntryObjects.Clear();
        foreach (LeaderboardElement entry in currentConfidenceEntryObjects)
        {
            Destroy(entry.gameObject);
        }
        currentConfidenceEntryObjects.Clear();

        //===========Creating new entries===========//
        foreach (LeaderboardDTO.LeaderboardElementDTO element in amountEntries[amountPage])
        {
            LeaderboardElement entry = Instantiate(LeaderboardEntryPrefab, AmountLeaderboardContent.transform);
            entry.SetData(element.username, element.score, element.place + 1);
            currentAmountEntryObjects.Add(entry);
        }

        foreach (LeaderboardDTO.LeaderboardElementDTO element in confidenceEntries[confidencePage])
        {
            LeaderboardElement entry = Instantiate(LeaderboardEntryPrefab, ConfidenceLeaderboardContent.transform);
            entry.SetData(element.username, element.score, element.place + 1);
            currentConfidenceEntryObjects.Add(entry);
        }
    }
}
