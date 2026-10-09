using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Annotationsguideline : MonoBehaviour
{
    public Image example1;
    public Image example2;
    public TextMeshProUGUI result1;
    public TextMeshProUGUI result2;
    public TextMeshProUGUI header;
    public TextMeshProUGUI description;
    public MenuNavigation menuNavigation;

    public Action actionAfterCompletion;

    private int currentPage = 1;
    private string messageKey = "";
    private object[] messageArguments = Array.Empty<object>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuildPage(currentPage);
    }

    public void StartRound(Action actionAfterCompletion, string messageKey = "", params object[] messageArguments)
    {
        if (messageKey != "")
        {
            this.currentPage = 0;
            this.messageKey = messageKey;
            this.messageArguments = messageArguments;
            BuildPage(currentPage);
        }
        Debug.Log("Starting Tutorial");
        this.gameObject.transform.parent.gameObject.SetActive(true);
        this.gameObject.SetActive(true);
        this.actionAfterCompletion = actionAfterCompletion;
    }

    public void NextPage()
    {
        currentPage++;
        BuildPage(currentPage);
    }

    public void LastPage()
    {
        if (currentPage > 1)
        {
            currentPage--;
            BuildPage(currentPage);
        }
    }

    private void BuildPage(int pageNumber)
    {
         switch (pageNumber)
        {
            case 0: //only gets triggered by StartRound if a message is passed, which happens only in the tutorial after a failed attempt
                example1.gameObject.SetActive(false);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(false);
                result2.gameObject.SetActive(false);
                I18nText.SetText(header, "tutorial.retry_header");
                I18nText.SetFormattedText(description, messageKey, messageArguments);
                break;

            case 1:
                example1.gameObject.SetActive(false);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(false);
                result2.gameObject.SetActive(false);
                I18nText.SetText(header, "tutorial.page.1.title");
                I18nText.SetText(description, "tutorial.page.1.body");
                break;

            case 2:
                example1.gameObject.SetActive(false);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(false);
                result2.gameObject.SetActive(false);
                I18nText.SetText(header, "tutorial.page.2.title");
                I18nText.SetText(description, "tutorial.page.2.body");
                break;

            case 3:
                example1.gameObject.SetActive(false);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(false);
                result2.gameObject.SetActive(false);
                I18nText.SetText(header, "tutorial.page.3.title");
                I18nText.SetText(description, "tutorial.page.3.body");
                break;
               
            case 4:
                example1.gameObject.SetActive(false);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(false);
                result2.gameObject.SetActive(false);
                I18nText.SetText(header, "tutorial.page.4.title");
                I18nText.SetText(description, "tutorial.page.4.body");
                break;

            case 5:
                example1.gameObject.SetActive(false);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(false);
                result2.gameObject.SetActive(false);
                I18nText.SetText(header, "tutorial.page.5.title");
                I18nText.SetText(description, "tutorial.page.5.body");
                break;

            case 6:
                StartCoroutine(LoadImageFromUrl("https://api.digitale-sammlungen.de/iiif/image/v2/bsb00013593_00001/1715,61,116,119/full/0/default.jpg", example1));
                StartCoroutine(LoadImageFromUrl("https://api.digitale-sammlungen.de/iiif/image/v2/bsb00013593_00001/1990,1780,116,119/full/0/default.jpg", example2));
                example1.gameObject.SetActive(true);
                example2.gameObject.SetActive(true);
                result1.gameObject.SetActive(true);
                result2.gameObject.SetActive(true);
                I18nText.SetText(result1, "tutorial.decorated");
                I18nText.SetText(result2, "tutorial.undecorated");
                example1.sprite = null;
                example2.sprite = null;
                I18nText.SetText(header, "tutorial.page.6.title");
                I18nText.SetText(description, "tutorial.page.6.body");
                break;

            case 7:
                StartCoroutine(LoadImageFromUrl("https://api.digitale-sammlungen.de/iiif/image/v2/bsb00151582_00061/1274,3435,86,77/full/0/default.jpg", example1));
                StartCoroutine(LoadImageFromUrl("https://api.digitale-sammlungen.de/iiif/image/v2/bsb00151486_00002/3571,4280,65,74/full/0/default.jpg", example2));
                example1.gameObject.SetActive(true);
                example2.gameObject.SetActive(true);
                result1.gameObject.SetActive(true);
                result2.gameObject.SetActive(true);
                I18nText.SetText(result1, "tutorial.decorated");
                I18nText.SetText(result2, "tutorial.undecorated");
                example1.sprite = null;
                example2.sprite = null;
                I18nText.SetText(header, "tutorial.page.7.title");
                I18nText.SetText(description, "tutorial.page.7.body");
                break;

            case 8:
                StartCoroutine(LoadImageFromUrl("https://api.digitale-sammlungen.de/iiif/image/v2/bsb00151486_00005/3247,3250,112,95/full/0/default.jpg", example1));
                example1.gameObject.SetActive(true);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(true);
                result2.gameObject.SetActive(false);
                I18nText.SetText(result1, "tutorial.undecorated");
                example1.sprite = null;
                example2.sprite = null;
                I18nText.SetText(header, "tutorial.page.8.title");
                I18nText.SetText(description, "tutorial.page.8.body");
                break;

            case 9:
                StartCoroutine(LoadImageFromUrl("https://content.staatsbibliothek-berlin.de/dc/1809307333-0035/6938,1701,148,176/full/0/default.jpg",example1));
                example1.gameObject.SetActive(true);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(true);
                result2.gameObject.SetActive(false);
                I18nText.SetText(result1, "tutorial.bad_data");
                example1.sprite = null;
                example2.sprite = null;
                I18nText.SetText(header, "tutorial.page.9.title");
                I18nText.SetText(description, "tutorial.page.9.body");
                break;

            case 10:
                if (actionAfterCompletion != null) // Happens when activated by a forced Tutorial
                {
                    example1.gameObject.SetActive(false);
                    example2.gameObject.SetActive(false);
                    result1.gameObject.SetActive(false);
                    result2.gameObject.SetActive(false);
                    I18nText.SetText(header, "", "");
                    I18nText.SetText(description, "tutorial.test_intro");
                }
                else
                {
                    menuNavigation.GoBack();
                    currentPage = 1;
                    BuildPage(currentPage);
                }
                break;

            case 11:    
                actionAfterCompletion.Invoke();
                this.gameObject.transform.parent.gameObject.SetActive(false);
                actionAfterCompletion = null;
                currentPage = 1;
                BuildPage(currentPage);
                break;


        }
    }

    private IEnumerator LoadImageFromUrl(string imageUrl, Image targetImage)
    {
        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Annotationsguideline: Failed to load image: " + request.error);
                yield break;
            }

            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            if (texture == null)
            {
                Debug.LogError("Annotationsguideline: Downloaded texture is null.");
                yield break;
            }

            Sprite sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f)
            );

            targetImage.sprite = sprite;
            targetImage.preserveAspect = true;
            targetImage.color = Color.white;
        }
    }
}
