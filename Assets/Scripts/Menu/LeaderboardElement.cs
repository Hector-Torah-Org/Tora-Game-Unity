using UnityEngine;

public class LeaderboardElement : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI UsernameText;
    [SerializeField] private TMPro.TextMeshProUGUI ScoreText;
    [SerializeField] private TMPro.TextMeshProUGUI RankText;

    public void SetData(string username, string score, int rank)
    {
        UsernameText.text = username;
        ScoreText.text = score;
        RankText.text = rank.ToString();
    }
}
