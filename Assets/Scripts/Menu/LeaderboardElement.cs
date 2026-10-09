using UnityEngine;

public class LeaderboardElement : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI UsernameText;
    [SerializeField] private TMPro.TextMeshProUGUI ScoreText;
    [SerializeField] private TMPro.TextMeshProUGUI RankText;

    public void SetData(string username, string score, int rank)
    {
        I18nText.SetLiteralText(UsernameText, username);
        I18nText.SetLiteralText(ScoreText, score);
        I18nText.SetLiteralText(RankText, rank.ToString(I18n.CurrentCulture));
    }
}
