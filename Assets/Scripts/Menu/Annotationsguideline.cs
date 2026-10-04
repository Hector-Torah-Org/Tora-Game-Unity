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
    private string message = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        BuildPage(currentPage);
    }

    public void StartRound(Action actionAfterCompletion, string message = "")
    {
        if (message != "")
        {
            this.currentPage = 0;
            this.message = message;
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
                header.text = "Nächster Versuch";
                description.text = this.message;
                break;

            case 1:
                example1.gameObject.SetActive(false);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(false);
                result2.gameObject.SetActive(false);
                header.text = "Einleitung";
                description.text = "Buchstaben in Torarollen enthalten oftmals Dekorationen, beispielsweise in Form von Kringeln und Krönchen. Diese Dekorationen werden Tagin genannt. Ziel des Annotationsprojektes ist die Erarbeitung eines größeren Datensatzes mit annotierten Buchstaben auf Grundlage der dekorativen Elemente. Der Annotationsprozess soll im Rahmen eines Citizen-Science-Projekts stattfinden. Hierbei handelt es sich um einen Crowdsourcing-Ansatz, bei dem interessierte Personen am Annotationsprozess teilnehmen können und somit zur wissenschaftlichen Forschung beitragen. Im folgenden Leitfaden findet sich eine Beschreibung der relevanten Annotationseinheiten und -kategorien. Weiterhin werden problematische Fälle zur Illustration der Eigenheiten des Datensatzes bezüglich der zu annotierenden Einheiten vorgestellt.";
                break;

            case 2:
                example1.gameObject.SetActive(false);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(false);
                result2.gameObject.SetActive(false);
                header.text = "Annotationseinheiten";
                description.text = "Das Annotationsprojekt befasst sich auf der Ebene der Annotationseinheiten mit Buchstaben. Diese können dekorative Elemente enthalten. Die Buchstaben sind klar voneinander abgegrenzt. Wie bei Torarollen vorgegeben, handelt es sich um hebräische Buchstaben. Die Buchstaben liegen in handgeschriebener Form vor.\r\n";
                break;

            case 3:
                example1.gameObject.SetActive(false);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(false);
                result2.gameObject.SetActive(false);
                header.text = "Annotationskategorien";
                description.text = "Der Fokus des Annotationsprojekts liegt auf der Frage nach der Verteilung und dem Einsatz von Buchstabendekorationen in primär historischen Torarollen. Dementsprechend sind die zentralen Kategorien „dekoriert“ und „undekoriert“. Bei der ersten Kategorie handelt es sich um eine Zusammenfassung mehrerer Phänomene. Beispielsweise gibt es sowohl kringelartige als auch kronenartige dekorative Elemente, die den Buchstaben hinzugefügt wurden. Da der Fokus auf der allgemeinen Nutzung und Verteilung von Dekorationen liegt, wird im ersten hier beschriebenen Schritt nur zwischen den beiden Kategorien „dekoriert“ und „undekoriert“ unterschieden.\r\n";
                break;
               
            case 4:
                example1.gameObject.SetActive(false);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(false);
                result2.gameObject.SetActive(false);
                header.text = "Annotationskategorien";
                description.text = "Dementsprechend sind folgende Kategorien gegeben:\n" +
                                    "• Dekoriert: Die Annotationseinheit weist über die Struktur des eigentlichen Buchstabens hinaus weitere Elemente dekorativer Natur auf.\n" +
                                    "• Undekoriert: Die Annotationseinheit weist über die Struktur des eigentlichen Buchstabens hinaus keine weiteren Elemente dekorativer Natur auf; sie besteht also nur aus der Form des entsprechenden hebräischen Buchstabens.";
                break;

            case 5:
                example1.gameObject.SetActive(false);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(false);
                result2.gameObject.SetActive(false);
                header.text = "Problematische Fälle";
                description.text = "Aufgrund der handschriftlichen Natur des Datensatzes, variierender Digitalisierungsmethoden sowie von Fehlern bei der automatischen Buchstabenerkennung treten Herausforderungen auf. Diese sind in vier Kategorien zu unterteilen.";
                break;

            case 6:
                StartCoroutine(LoadImageFromUrl("https://api.digitale-sammlungen.de/iiif/image/v2/bsb00013593_00001/1715,61,116,119/full/0/default.jpg", example1));
                StartCoroutine(LoadImageFromUrl("https://api.digitale-sammlungen.de/iiif/image/v2/bsb00013593_00001/1990,1780,116,119/full/0/default.jpg", example2));
                example1.gameObject.SetActive(true);
                example2.gameObject.SetActive(true);
                result1.gameObject.SetActive(true);
                result2.gameObject.SetActive(true);
                result1.text = "Decorated";
                result2.text = "Undecorated";
                example1.sprite = null;
                example2.sprite = null;
                header.text = "Problematische Fälle";
                description.text = "1. Schlechte Bildqualität: Durch den Digitalisierungs- und Weiterverarbeitungsprozess liegen einige Scans der Torarollen nur in geringer Qualität gemessen an der Auflösung vor. Dementsprechend sehen sich Annotator*innen mit Unklarheiten aufgrund von verpixelten oder verschwommenen Buchstaben konfrontiert. Es können aufgrund der niedrigen Bildqualität Artefakte auftreten, die missinterpretiert werden können und somit fälschlicherweise als Dekoration annotiert werden können.";
                break;

            case 7:
                StartCoroutine(LoadImageFromUrl("https://api.digitale-sammlungen.de/iiif/image/v2/bsb00151582_00061/1274,3435,86,77/full/0/default.jpg", example1));
                StartCoroutine(LoadImageFromUrl("https://api.digitale-sammlungen.de/iiif/image/v2/bsb00151486_00002/3571,4280,65,74/full/0/default.jpg", example2));
                example1.gameObject.SetActive(true);
                example2.gameObject.SetActive(true);
                result1.gameObject.SetActive(true);
                result2.gameObject.SetActive(true);
                result1.text = "Decorated";
                result2.text = "Undecorated";
                example1.sprite = null;
                example2.sprite = null;
                header.text = "Problematische Fälle";
                description.text = "2. Handschriftliche Natur und undeutliche Dekorationen: Die handschriftliche Natur der Buchstaben stellt Annotator*innen vor weitere Herausforderungen. Die Identifizierung der Dekorationen gestaltet sich gerade bei sehr dünnen oder sehr dicken Buchstabenteilen als kompliziert. Weiterhin kann die verwendete Schriftart der Gruppe der Serifenschriften zugeordnet werden. Die entsprechenden Serifen innerhalb der Buchstaben können leicht mit dekorativen Elementen verwechselt werden.";
                break;

            case 8:
                StartCoroutine(LoadImageFromUrl("https://api.digitale-sammlungen.de/iiif/image/v2/bsb00151486_00005/3247,3250,112,95/full/0/default.jpg", example1));
                example1.gameObject.SetActive(true);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(true);
                result2.gameObject.SetActive(!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!false); //:D
                result1.text = "Undecorated";
                example1.sprite = null;
                example2.sprite = null;
                header.text = "Problematische Fälle";
                description.text = "3. Verschmutzungen: Basierend auf der handschriftlichen Natur sowie Digitalisierungs- und Lagerungsbedingungen finden sich Artefakte aufgrund von Verschmutzungen auf einigen Bilddaten. Diese heben sich jedoch zumeist hinreichend von der Schriftfarbe ab. Dementsprechend sind Verschmutzungen klar von Dekorationen zu trennen und stellen eine Herausforderung dar, die nur marginal ins Gewicht fällt.";
                break;

            case 9:
                StartCoroutine(LoadImageFromUrl("https://content.staatsbibliothek-berlin.de/dc/1809307333-0035/6938,1701,148,176/full/0/default.jpg",example1));
                example1.gameObject.SetActive(true);
                example2.gameObject.SetActive(false);
                result1.gameObject.SetActive(true);
                result2.gameObject.SetActive(false);
                result1.text = "Bad data";
                example1.sprite = null;
                example2.sprite = null;
                header.text = "Problematische Fälle";
                description.text = "4. Datensatzfehler: In seltenen Fällen sind die zu annotierenden Buchstaben nicht im Bildausschnitt sichtbar. Dies kann durch den automatisierten Buchstabenerkennungsprozess entstehen. Es treten beispielsweise Fälle auf, in denen nur Fragmente mehrerer Buchstaben gezeigt werden.";
                break;

            case 10:
                if (actionAfterCompletion != null) // Happens when activated by a forced Tutorial
                {
                    example1.gameObject.SetActive(false);
                    example2.gameObject.SetActive(false);
                    result1.gameObject.SetActive(false);
                    result2.gameObject.SetActive(false);
                    header.text = "";
                    description.text = "Bevor es mit dem Spiel losgeht, gibt es jetzt noch ein kleinen Test";
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
