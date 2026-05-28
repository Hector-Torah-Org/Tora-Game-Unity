using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Collections;
using System;
using Unity.VisualScripting;
using System.Linq;

public class StatisticsPanel : MonoBehaviour
{

    [SerializeField] public GameObject StatisticGraphPanel;
    [SerializeField] private ApiConnection apiConnection;
    [SerializeField] private TMPro.TextMeshProUGUI MaxAmountText;
    [SerializeField] private Toggle ShowDailyDataToggle;
    [SerializeField] private TMPro.TextMeshProUGUI YearText;


    private float x;
    private float y;
    private float width;
    private float height;

    private int year = DateTime.Now.Year;

    private Dictionary<int, List<double>> confidenceDataRaw = new Dictionary<int, List<double>>();
    private Dictionary<int, List<double>> amountDataRaw = new Dictionary<int, List<double>>();
    private bool dailyDataShown = true;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        width = StatisticGraphPanel.GetComponent<RectTransform>().rect.width;
        height = StatisticGraphPanel.GetComponent<RectTransform>().rect.height;
        x = StatisticGraphPanel.GetComponent<RectTransform>().offsetMin.x;
        y = -StatisticGraphPanel.GetComponent<RectTransform>().offsetMax.y;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator GetData(int year)
    {
        int tasksDone = 0;
        StartCoroutine(apiConnection.GetAmountStatistic(year, success =>
        {
            if (success.year != year) throw new Exception("Received data for wrong year");
            amountDataRaw[year] = new List<double>(success.values);
            tasksDone++;
        }));
        StartCoroutine(apiConnection.GetConfidenceStatistic(year, success =>
        {
            if (success.year != year) throw new Exception("Received data for wrong year");
            List<double> confidenceData = new List<double>(success.values);
            for (int i = 0; i < confidenceData.Count; i++) // Just guessing should be 0 instead of 0.5
            {
                confidenceData[i] = confidenceData[i] - 0.5d;
                if (confidenceData[i] < 0) confidenceData[i] = 0;
                confidenceData[i] = confidenceData[i] * 2;
            }
            confidenceDataRaw[year] = confidenceData;
            tasksDone++;
        }));
        yield return new WaitUntil(() => tasksDone == 2);
    }

    public void OnShowDailyDataToggleChanged(bool value)
    {
        if (value)
        {
            dailyDataShown = true;
        }
        else
        {
            dailyDataShown = false;
        }
        redrawGraph();
    }

    public void YearUpButton()
    {
        year++;
        DrawGraph();
    }

    public void YearDownButton()
    {
        year--;
        DrawGraph();
    }

    public IEnumerator DrawGraph()
    {
        YearText.text = year.ToString();
        if (!amountDataRaw.ContainsKey(year) || !confidenceDataRaw.ContainsKey(year))
        {
            Debug.Log("Starting to get data");
            yield return GetData(year);
            Debug.Log("Data received");
        }
        Debug.Log("x: " + x + " y: " + y + " width: " + width + " height: " + height);
        Debug.Log("Amount data: " + string.Join(", ", amountDataRaw[year]));
        redrawGraph();
        Debug.Log("Confidence data: " + string.Join(", ", confidenceDataRaw[year]));
    }

    public void redrawGraph()
    {
        var root = StatisticGraphPanel.GetComponent<UIDocument>().rootVisualElement;
        root.generateVisualContent -= Draw;
        root.generateVisualContent += Draw;
        root.MarkDirtyRepaint();
    }

    void Draw(MeshGenerationContext context)
    {
        List<double> confidenceData = confidenceDataRaw[year];
        List<double> amountData = amountDataRaw[year];

        if (!dailyDataShown) // converting to weekly data
        {
            confidenceData = new List<double>();
            amountData = new List<double>();

            for (int i = 0; i < amountDataRaw[year].Count; i++)
            {
                if (i % 7 == 0)
                {
                    if(i != 0 && amountData[(i/7) - 1] != 0)
                    {
                        confidenceData[(i/7) - 1] /= amountData[(i/7) - 1];
                    }
                    amountData.Add(0);
                    confidenceData.Add(0);
                }
                amountData[i / 7] += amountDataRaw[year][i];
                confidenceData[i / 7] += confidenceDataRaw[year][i] * amountDataRaw[year][i];
            }
            if (amountData[amountData.Count - 1] != 0) confidenceData[confidenceData.Count - 1] /= amountData[amountData.Count - 1];
        }

        var painter = context.painter2D;
        float x = this.x;
        float y = this.y;
        float width = this.width;
        float height = this.height;
        
        // Draw amount Data

        double xStep = width / (amountData.Count - 1);
        double maxAmount = amountData.Max();

        painter.lineWidth = 2;
        painter.strokeColor = Color.red;
        painter.BeginPath();
        painter.MoveTo(new Vector2(x, y + height - (amountData[0] / maxAmount * height).ConvertTo<float>()));

        for (int i = 1; i < amountData.Count; i++)
        {
            float xPos = x + (float)(i * xStep);
            float yPos = y + height - (amountData[i] / maxAmount * height).ConvertTo<float>();
            painter.LineTo(new Vector2(xPos, yPos));
        }
        painter.Stroke();

        // Draw confidence Data

        painter.strokeColor = Color.green;
        painter.BeginPath();
        painter.MoveTo(new Vector2(x, y + height - (confidenceData[0] * height).ConvertTo<float>()));

        for (int i = 1; i < confidenceData.Count; i++)
        {
            float xPos = x + (float)(i * xStep);
            float yPos = y + height - (confidenceData[i] * height).ConvertTo<float>();
            
            Debug.Log("Drawing confidence point at x: " + xPos + " y: " + yPos + " confidence: " + confidenceData[i]);
            painter.LineTo(new Vector2(xPos, yPos));
        }
        painter.LineTo(new Vector2(x + width, y + height));
        painter.LineTo(new Vector2(x, y + height));

        painter.LineTo(new Vector2(x, y + height - (confidenceData[0] * height).ConvertTo<float>()));
        painter.ClosePath();
        painter.fillColor = new Color(0, 100, 0, 0.5f);
        painter.Fill();
        painter.Stroke();

        // Draw axes and max amount
        context.painter2D.lineWidth = 3;
        context.painter2D.strokeColor = Color.black;
        context.painter2D.BeginPath();
        context.painter2D.MoveTo(new Vector2(x, y));
        context.painter2D.LineTo(new Vector2(x, y + height));
        context.painter2D.LineTo(new Vector2(x + width, y + height));
        context.painter2D.LineTo(new Vector2(x + width, y));
        context.painter2D.Stroke();

        MaxAmountText.text = amountData.Max().ToString();

    }
}
