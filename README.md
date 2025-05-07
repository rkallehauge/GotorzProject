

## README.md

# Projektnavn
GoTorzProjekt

En kort beskrivelse af, hvad projektet går ud på.

GodTur gør rejseoplevelsen enkel og effektiv. Kunderne kan kontakte virksomheden via WhatsApp eller messenger-tjenester for at forespørge om og købe rejser. Efter at have delt deres personlige oplysninger modtager de en elektronisk faktura via Stripe, som muliggør nem betaling med kreditkort. Efter betalingen modtager kunden e-billetter og hotelreservationer direkte via e-mail.
For at sikre en god kundeoplevelse leverer virksomheden også digitale rejseguider, der beskriver attraktioner og de bedste restauranter samt en rejseplan over transport fra lufthavnen til hotellet og en kortoversigt over hotellets beliggenhed og omgivelser.

Hvordan kan GodTur udvikle en webapplikation, der integrerer data fra både flyselskabers API og hotellers API for at skabe en samlet og overskuelig rejsepakke, som præsenteres for kunden som én samlet
reservation? Systemet skal være designet, så det skjuler de enkelte detaljer om fly og hotel for kunden, og det samlede tilbud præsenteres på en enkel måde. Integration af betalingssystemer og forbedret UI/UX betragtes som en ekstra bonus.

# Dette er et webapplikationsprojekt lavet i Blazor Server med Razor Pages og en SQL Server-database. Applikationen gør det muligt for brugere at [indsæt funktionalitet, fx oprette og administrere produkter i et katalog].

##  Screenshots

*Indsæt gerne billeder af brugergrænsefladen eller nøglefunktioner.*

##  Teknologier

Projektet er bygget med følgende teknologier:

- [.NET 8](https://dotnet.microsoft.com/)
- Blazor Server
- Razor Pages
- Entity Framework Core
- SQL Server
- Visual Studio 2022

##  Kom i gang

### Forudsætninger

Du skal have følgende installeret:

- Visual Studio 2022 med .NET 8 og Blazor Server workload
- SQL Server
- Git

### Installation og opsætning

1. Clone dette repository:

```bash
git clone https://github.com/brugernavn/projektnavn.git
````

2. Åbn projektet i Visual Studio.

3. Konfigurer databasen:

* Ret `appsettings.json`, så connection string passer til din lokale SQL Server:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ProjektDb;Trusted_Connection=True;"
}
```

4. Kør databasen (enten manuelt eller med migration):

```bash
dotnet ef database update
```

5. Kør projektet via Visual Studio (F5 eller Ctrl+F5).

##  Funktionalitet

Beskriv kort de vigtigste funktioner:

* Opret/redigér/slet \[fx produkter]
* Login-system med roller
* Søge- og filtreringsfunktioner
* Validering af input

##  Arkitektur

En kort beskrivelse af projektets opbygning, fx:

* `Pages/` – Razor-siderne
* `Data/` – Databasekontekst og datamodeller
* `Services/` – Services til logik/adgang til data
* `wwwroot/` – CSS, JS og billeder

##  Team 5

* Lars Jensen – rolle/bidrag
* Mathilde Johnsen-Zaavi – rolle/bidrag
* Esben Ejdesgaard – rolle/bidrag
* Martin Jensen – rolle/bidrag
* Rasmus Wissing Kallehauge – rolle/bidrag

##  Licens

Dette projekt er lavet som en del af et skoleprojekt og er ikke licenseret til produktion

---

## ✅ Tips

* **Brug markdown-syntaks** for at lave en læsbar og flot README.
* Inkludér billeder/gifs hvis muligt – det gør det mere visuelt.
* Hold det opdateret med jeres kode – især teknologier og opsætning.
* Skriv det som om det skal bruges af en person, der ikke har set koden før.

