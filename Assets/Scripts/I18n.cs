using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public static class I18n
{
    public enum Language
    {
        English,
        German
    }

    private const string LanguagePreferenceKey = "language";
    private static bool initialized;
    private static Language currentLanguage;

    public static event Action LanguageChanged;

    private static readonly Dictionary<string, string> german = new Dictionary<string, string>
    {
        { "New Text", "" },
        { "Sign Up and Login", "Registrieren und anmelden" },
        { "First Name ", "Vorname " },
        { "Last Name", "Nachname" },
        { "User Name ", "Benutzername " },
        { "Login", "Anmelden" },
        { "Statistics", "Statistiken" },
        { "Tutorial", "Tutorial" },
        { "DECORATED", "VERZIERT" },
        { "UNDECORATED", "UNVERZIERT" },
        { "BAD DATA", "FEHLERHAFTE DATEN" },
        { "Save ", "Speichern " },
        { "Choose a classification", "Bitte eine Kategorie auswählen" },
        { "Leaderboards\n\n", "Bestenlisten\n\n" },
        { "Objectives/Story go in here", "Ziele und Geschichte erscheinen hier" },

        { "item.1.name", "Münze" },
        { "item.2.name", "Freischaltobjekt 1" },
        { "item.3.name", "Axt" },
        { "item.4.name", "Wassereimer" },
        { "item.5.name", "Hebel" },
        { "item.6.name", "Spitzhacke" },
        { "item.7.name", "Schlüssel" },
        { "item.8.name", "Schwert" },
        { "item.9.name", "Verfluchte Münze" },
        { "item.10.name", "Seil" },
        { "item.11.name", "Brückenreparatur" },
        { "item.12.name", "Brecheisen" },
        { "item.13.name", "TOG" },
        { "item.14.name", "TOD" },
        { "item.15.name", "TOL" },
        { "item.16.name", "Steintafel" },
        { "item.17.name", "Schriftrolle" },
        { "item.18.name", "Brief" },
        { "item.16.readable.title", "Steintafel – Kirche" },
        { "item.17.readable.title", "Schriftrolle – Dorf" },
        { "item.18.readable.title", "Brief – Dorf" },
        { "readable.placeholder", "----------------------------------------------------\n\n\n\n\n\t\t\tPlatzhalter\n\n\n\n---------------------------------------------------" },

        { "quest.hint.rubble", "Der Weg ist von Geröll versperrt! Wenn ich doch nur etwas hätte, um es zu beseitigen ..." },
        { "quest.find.rubble_tool", "Finde etwas, um das Geröll zu beseitigen" },
        { "quest.hint.mine", "Das Minentor ist vernagelt. Wie kann ich diese Bretter wohl aufbrechen ..." },
        { "quest.use.tool_on_planks", "Benutze ein Werkzeug, um die Bretter aufzubrechen" },
        { "quest.hint.pedestal", "Für diesen Sockel wird wohl etwas benötigt." },
        { "quest.find.pedestal_item", "Finde etwas für den Sockel" },
        { "quest.hint.water", "So viel Wasser! Ich muss etwas finden, um es zu beseitigen!" },
        { "quest.remove.water", "Beseitige das Wasser" },
        { "quest.hint.bridge", "Hier muss einmal eine Brücke über den Fluss geführt haben." },
        { "quest.restore.bridge", "Finde etwas Magisches, um die Brücke wiederherzustellen" },
        { "quest.hint.tile", "Ich muss diese Bodenplatte anheben." },
        { "quest.lift.tile", "Hebe die Bodenplatte mit einem Werkzeug an" },
        { "quest.hint.vines", "Diese verdammten Ranken versperren mir den Weg!" },
        { "quest.remove.vines", "Entferne die Ranken mit etwas Scharfem" },
        { "quest.hint.graveyard_gate", "Dieses Tor wirkt unheimlich. Etwas Unheimliches könnte es wohl öffnen ..." },
        { "quest.find.graveyard_item", "Finde etwas Unheimliches, das zu einem Friedhof passt" },
        { "quest.hint.church_symbol", "Das Symbol auf diesem Steintor kommt mir bekannt vor. Ich könnte schwören, es schon einmal in der Kirche gesehen zu haben." },
        { "quest.use.symbol_item", "Benutze einen Gegenstand mit diesem Symbol" },
        { "quest.hint.another_symbol", "Noch ein Symbol ..." },
        { "quest.find.skull_talisman", "Finde einen Talisman mit einem Totenkopf" },
        { "quest.hint.glowing_symbol", "Dieses leuchtende Symbol ..." },
        { "quest.find.tree_talisman", "Finde einen Talisman mit einem Baumsymbol" },
        { "quest.hint.dungeon_gate", "Dieses große Tor ist verschlossen ..." },
        { "quest.get.dungeon_key", "Finde einen Schlüssel für das Tor zum Verlies" },
        { "quest.hint.rope", "Da oben links ist etwas! Oh, aber ich habe kein Seil ..." },
        { "quest.use.rope", "Benutze ein Seil, um dort hinaufzukommen" },
        { "quest.find.water_place", "Finde einen Ort für das Wasser" },
        { "quest.find.axe_use", "Finde eine Verwendung für die Axt" },

        { "round.loading_images", "Bilder werden geladen ..." },
        { "round.loading_image", "Bild wird geladen ..." },
        { "round.choose_classification", "Bitte eine Kategorie auswählen" },
        { "round.sending_classifications", "Klassifizierungen werden gesendet ..." },
        { "round.image_counter", "Bild {0} / {1}" },
        { "login.failed", "Anmeldung fehlgeschlagen. Bitte überprüfe deine Eingaben und versuche es erneut." },
        { "signup.names_taken", "Diese Namen sind bereits vergeben." },
        { "tutorial.retry_header", "Nächster Versuch" },
        { "tutorial.retry_message", "Du hast nur {0}% richtig beantwortet und kannst deshalb noch nicht mit dem Spiel beginnen. Lies den Annotationsleitfaden bitte noch einmal sorgfältig durch, damit du falsche Annotationen vermeidest. Viel Erfolg beim nächsten Versuch!" },
        { "tutorial.test_intro", "Bevor das Spiel losgeht, gibt es noch einen kleinen Test." },
        { "tutorial.page.1.title", "Einleitung" },
        { "tutorial.page.1.body", "Buchstaben in Torarollen enthalten oftmals Dekorationen, beispielsweise in Form von Kringeln und Krönchen. Diese Dekorationen werden Tagin genannt. Ziel des Annotationsprojekts ist die Erarbeitung eines größeren Datensatzes mit annotierten Buchstaben auf Grundlage der dekorativen Elemente. Der Annotationsprozess soll im Rahmen eines Citizen-Science-Projekts stattfinden. Hierbei handelt es sich um einen Crowdsourcing-Ansatz, bei dem interessierte Personen am Annotationsprozess teilnehmen und somit zur wissenschaftlichen Forschung beitragen. Im folgenden Leitfaden findet sich eine Beschreibung der relevanten Annotationseinheiten und -kategorien. Weiterhin werden problematische Fälle zur Illustration der Eigenheiten des Datensatzes bezüglich der zu annotierenden Einheiten vorgestellt." },
        { "tutorial.page.2.title", "Annotationseinheiten" },
        { "tutorial.page.2.body", "Das Annotationsprojekt befasst sich auf der Ebene der Annotationseinheiten mit Buchstaben. Diese können dekorative Elemente enthalten. Die Buchstaben sind klar voneinander abgegrenzt. Wie bei Torarollen vorgegeben, handelt es sich um hebräische Buchstaben. Die Buchstaben liegen in handgeschriebener Form vor." },
        { "tutorial.page.3.title", "Annotationskategorien" },
        { "tutorial.page.3.body", "Der Fokus des Annotationsprojekts liegt auf der Frage nach der Verteilung und dem Einsatz von Buchstabendekorationen in primär historischen Torarollen. Dementsprechend sind die zentralen Kategorien „dekoriert“ und „undekoriert“. Bei der ersten Kategorie handelt es sich um eine Zusammenfassung mehrerer Phänomene. Beispielsweise gibt es sowohl kringelartige als auch kronenartige dekorative Elemente, die den Buchstaben hinzugefügt wurden. Da der Fokus auf der allgemeinen Nutzung und Verteilung von Dekorationen liegt, wird im ersten hier beschriebenen Schritt nur zwischen den beiden Kategorien „dekoriert“ und „undekoriert“ unterschieden." },
        { "tutorial.page.4.title", "Annotationskategorien" },
        { "tutorial.page.4.body", "Dementsprechend sind folgende Kategorien gegeben:\n• Verziert: Die Annotationseinheit weist über die Struktur des eigentlichen Buchstabens hinaus weitere Elemente dekorativer Natur auf.\n• Unverziert: Die Annotationseinheit weist über die Struktur des eigentlichen Buchstabens hinaus keine weiteren Elemente dekorativer Natur auf; sie besteht also nur aus der Form des entsprechenden hebräischen Buchstabens." },
        { "tutorial.page.5.title", "Problematische Fälle" },
        { "tutorial.page.5.body", "Aufgrund der handschriftlichen Natur des Datensatzes, variierender Digitalisierungsmethoden sowie von Fehlern bei der automatischen Buchstabenerkennung treten Herausforderungen auf. Diese sind in vier Kategorien zu unterteilen." },
        { "tutorial.page.6.title", "Problematische Fälle" },
        { "tutorial.page.6.body", "1. Schlechte Bildqualität: Durch den Digitalisierungs- und Weiterverarbeitungsprozess liegen einige Scans der Torarollen nur in geringer Auflösung vor. Annotator*innen können deshalb durch verpixelte oder verschwommene Buchstaben verunsichert werden. Bei niedriger Bildqualität können Artefakte auftreten, die missinterpretiert und fälschlicherweise als Dekoration annotiert werden." },
        { "tutorial.page.7.title", "Problematische Fälle" },
        { "tutorial.page.7.body", "2. Handschriftliche Natur und undeutliche Dekorationen: Die handschriftliche Form der Buchstaben stellt Annotator*innen vor weitere Herausforderungen. Die Identifizierung von Dekorationen ist besonders bei sehr dünnen oder sehr dicken Buchstabenteilen schwierig. Außerdem gehört die verwendete Schriftart zu den Serifenschriften. Die Serifen innerhalb der Buchstaben können leicht mit dekorativen Elementen verwechselt werden." },
        { "tutorial.page.8.title", "Problematische Fälle" },
        { "tutorial.page.8.body", "3. Verschmutzungen: Aufgrund der handschriftlichen Natur sowie der Digitalisierungs- und Lagerungsbedingungen finden sich auf einigen Bildern Artefakte durch Verschmutzungen. Diese heben sich jedoch meist ausreichend von der Schriftfarbe ab. Verschmutzungen sind daher klar von Dekorationen zu unterscheiden und stellen nur eine geringe Herausforderung dar." },
        { "tutorial.page.9.title", "Problematische Fälle" },
        { "tutorial.page.9.body", "4. Datensatzfehler: In seltenen Fällen sind die zu annotierenden Buchstaben im Bildausschnitt nicht sichtbar. Dies kann durch die automatisierte Buchstabenerkennung entstehen. Beispielsweise können nur Fragmente mehrerer Buchstaben zu sehen sein." },
        { "tutorial.decorated", "Verziert" },
        { "tutorial.undecorated", "Unverziert" },
        { "tutorial.bad_data", "Fehlerhafte Daten" }
    };

    private static readonly Dictionary<string, string> english = new Dictionary<string, string>
    {
        { "New Text", "" },
        { "item.1.name", "Coin" },
        { "item.2.name", "Unlock item 1" },
        { "item.3.name", "Axe" },
        { "item.4.name", "Bucket" },
        { "item.5.name", "Lever" },
        { "item.6.name", "Pickaxe" },
        { "item.7.name", "Key" },
        { "item.8.name", "Sword" },
        { "item.9.name", "Cursed Coin" },
        { "item.10.name", "Rope" },
        { "item.11.name", "Bridge Restorer" },
        { "item.12.name", "Crowbar" },
        { "item.13.name", "TOG" },
        { "item.14.name", "TOD" },
        { "item.15.name", "TOL" },
        { "item.16.name", "Stone Tablet" },
        { "item.17.name", "Scroll" },
        { "item.18.name", "Letter" },
        { "quest.hint.rubble", "The rubble is blocking the path! If only I had something to break it..." },
        { "quest.find.rubble_tool", "Find something to remove the rubble" },
        { "quest.hint.mine", "The mine seems to be nailed shut. How could I break these planks..." },
        { "quest.use.tool_on_planks", "Use some tool to break the planks" },
        { "quest.hint.pedestal", "That pedestal seems to need something." },
        { "quest.find.pedestal_item", "Find something for the pedestal" },
        { "quest.hint.water", "So much water! I need to find something to remove it!" },
        { "quest.remove.water", "Remove the water" },
        { "quest.hint.bridge", "There seems to have been a bridge across the river here." },
        { "quest.restore.bridge", "Find something magical to restore the bridge" },
        { "quest.hint.tile", "I need to lift this tile." },
        { "quest.lift.tile", "Lift the tile using a tool" },
        { "quest.hint.vines", "These damn vines are blocking my way!" },
        { "quest.remove.vines", "Remove the vines using something sharp" },
        { "quest.hint.graveyard_gate", "This gate seems ominous. Something ominous would probably open it..." },
        { "quest.find.graveyard_item", "Find something ominous fitting a graveyard" },
        { "quest.hint.church_symbol", "The symbol on this stone gate seems familiar. I could swear I have seen it before at the church." },
        { "quest.use.symbol_item", "Use an item with the symbol on it" },
        { "quest.hint.another_symbol", "Another symbol..." },
        { "quest.find.skull_talisman", "Find a talisman with a skull on it" },
        { "quest.hint.glowing_symbol", "That glowing symbol..." },
        { "quest.find.tree_talisman", "Find a talisman with a tree symbol" },
        { "quest.hint.dungeon_gate", "That big gate is locked..." },
        { "quest.get.dungeon_key", "Get a key for the dungeon gate" },
        { "quest.hint.rope", "I can see something up there on the left! Oh, but there is no rope..." },
        { "quest.use.rope", "Use a rope to get up there" },
        { "quest.find.water_place", "Find a place for the water" },
        { "quest.find.axe_use", "Find a use for the axe" },
        { "round.loading_images", "Loading images..." },
        { "round.loading_image", "Loading image..." },
        { "round.choose_classification", "Choose a classification" },
        { "round.sending_classifications", "Sending classifications..." },
        { "round.image_counter", "Image {0} / {1}" },
        { "login.failed", "Login failed. Please check your details and try again." },
        { "signup.names_taken", "Those names are already taken." },
        { "tutorial.retry_header", "Try again" },
        { "tutorial.retry_message", "You answered only {0}% correctly, so you cannot start the game yet. Please read the annotation guide carefully again to avoid incorrect annotations. Good luck on your next attempt!" },
        { "tutorial.test_intro", "Before the game begins, there is a short test." },
        { "tutorial.page.1.title", "Introduction" },
        { "tutorial.page.1.body", "Letters in Torah scrolls often contain decorations, such as curls and crowns. These decorations are called tagin. The goal of this annotation project is to create a larger dataset of annotated letters based on these decorative elements. The annotation process is intended to take place as a citizen science project: an approach in which interested people contribute to scientific research. This guide describes the relevant annotation units and categories, and presents problematic cases to illustrate characteristics of the dataset." },
        { "tutorial.page.2.title", "Annotation Units" },
        { "tutorial.page.2.body", "The annotation units in this project are letters. They may contain decorative elements and are clearly separated from one another. As expected for Torah scrolls, these are Hebrew letters written by hand." },
        { "tutorial.page.3.title", "Annotation Categories" },
        { "tutorial.page.3.body", "The project focuses on the distribution and use of letter decorations, primarily in historical Torah scrolls. The two main categories are “decorated” and “undecorated”. The first combines several phenomena: for example, curls and crown-like decorative elements added to letters. At this stage, the focus is on the general use and distribution of decorations, so annotations distinguish only between these two categories." },
        { "tutorial.page.4.title", "Annotation Categories" },
        { "tutorial.page.4.body", "The categories are:\n• Decorated: The annotation unit contains decorative elements in addition to the structure of the letter itself.\n• Undecorated: The annotation unit contains no additional decorative elements and consists only of the shape of the Hebrew letter." },
        { "tutorial.page.5.title", "Problematic Cases" },
        { "tutorial.page.5.body", "Challenges arise from the handwritten nature of the dataset, varying digitization methods, and errors in automatic letter recognition. These challenges fall into four categories." },
        { "tutorial.page.6.title", "Problematic Cases" },
        { "tutorial.page.6.body", "1. Poor image quality: Some Torah scroll scans have low resolution due to digitization and post-processing. Pixelated or blurred letters can make annotation uncertain. Low image quality can also produce artifacts that may be misinterpreted and incorrectly annotated as decorations." },
        { "tutorial.page.7.title", "Problematic Cases" },
        { "tutorial.page.7.body", "2. Handwriting and unclear decorations: Handwritten letters present further challenges. Decorations can be difficult to identify, especially on very thin or thick parts of letters. The typeface also has serifs, which can easily be mistaken for decorative elements." },
        { "tutorial.page.8.title", "Problematic Cases" },
        { "tutorial.page.8.body", "3. Dirt: Handwriting, digitization, and storage conditions have left dirt artifacts on some images. These usually differ sufficiently from the ink color, so dirt can be distinguished from decorations and presents only a minor challenge." },
        { "tutorial.page.9.title", "Problematic Cases" },
        { "tutorial.page.9.body", "4. Dataset errors: In rare cases, the letters to be annotated are not visible in the image crop. This can result from automated letter recognition; for example, an image may show only fragments of several letters." },
        { "tutorial.decorated", "Decorated" },
        { "tutorial.undecorated", "Undecorated" },
        { "tutorial.bad_data", "Bad data" }
    };

    public static Language CurrentLanguage
    {
        get
        {
            EnsureInitialized();
            return currentLanguage;
        }
    }

    public static string LanguageCode
    {
        get { return CurrentLanguage == Language.German ? "de" : "en"; }
    }

    public static CultureInfo CurrentCulture
    {
        get { return CultureInfo.GetCultureInfo(LanguageCode); }
    }

    public static void Initialize()
    {
        if (initialized)
            return;

        string savedLanguage = PlayerPrefs.GetString(LanguagePreferenceKey, "");
        if (savedLanguage == "de")
            currentLanguage = Language.German;
        else if (savedLanguage == "en")
            currentLanguage = Language.English;
        else
            currentLanguage = Application.systemLanguage == SystemLanguage.German ? Language.German : Language.English;

        initialized = true;
    }

    public static void SetLanguage(Language language)
    {
        EnsureInitialized();
        PlayerPrefs.SetString(LanguagePreferenceKey, language == Language.German ? "de" : "en");
        PlayerPrefs.Save();

        if (currentLanguage == language)
            return;

        currentLanguage = language;
        LanguageChanged?.Invoke();
    }

    public static string Translate(string key)
    {
        return Translate(key, key);
    }

    public static string Translate(string key, string fallback)
    {
        EnsureInitialized();
        string translation;
        Dictionary<string, string> table = currentLanguage == Language.German ? german : english;
        return table.TryGetValue(key, out translation) ? translation : fallback;
    }

    public static string Format(string key, params object[] arguments)
    {
        return string.Format(CurrentCulture, Translate(key), arguments);
    }

    internal static bool HasTranslation(string key)
    {
        EnsureInitialized();
        return german.ContainsKey(key) || english.ContainsKey(key);
    }

    private static void EnsureInitialized()
    {
        if (!initialized)
            Initialize();
    }
}
