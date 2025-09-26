
# Mastermind (C# • WPF & Konsol)

En moderne udgave af **Mastermind** skrevet i C# med både **konsol-app** og **WPF GUI**. 
Løsningen er opdelt i en **Core**-pakke med spil-logik, en **Wpf**-klient (MVVM) og en **Console**-klient.

---

## 📦 Løsningsstruktur

```
📦 Opgave-3---Spil-Mastermind
├─ 📄 Mastermind.sln
├─ 📄 README.md
├─ 📄 LICENSE
├─ 📄 appsettings.json
├─ 📄 appsettings.Development.json
├─ 📂 Properties
│  └─ 📄 launchSettings.json
├─ 📂 Database
│  ├─ 📄 Options.json
│  ├─ 📄 Statistik.json
│  └─ 📄 log.txt
└─ 📂 src
   ├─ 📂 Mastermind.Core
   │  ├─ 📂 Domain
   │  │  ├─ 📄 Farver.cs
   │  │  ├─ 📄 FarverHelper.cs
   │  │  ├─ 📄 Feedback.cs
   │  │  ├─ 📄 GameResultater.cs
   │  │  ├─ 📄 Options.cs
   │  │  ├─ 📄 Respons.cs
   │  │  └─ 📄 Sprog.cs
   │  ├─ 📂 Services
   │  │  ├─ 📄 Evaluering.cs
   │  │  ├─ 📄 Input.cs
   │  │  └─ 📄 SecretGenerator.cs
   │  ├─ 📂 Persistence
   │  │  ├─ 📄 IStatistikStore.cs
   │  │  ├─ 📄 JsonFilePaths.cs
   │  │  ├─ 📄 JsonStatistikStore.cs
   │  │  └─ 📄 OptionsRepository.cs
   │  ├─ 📂 Utils
   │  │  └─ 📄 StatistikTilføjer.cs
   │  └─ 📄 (projektfil)  — *hvis relevant*
   │
   ├─ 📂 Mastermind.Wpf
   │  ├─ 📄 App.xaml
   │  ├─ 📄 App.xaml.cs
   │  ├─ 📄 MainWindow.xaml
   │  ├─ 📄 MainWindow.xaml.cs
   │  ├─ 📄 MainViewModel.cs
   │  ├─ 📂 Infrastructure
   │  │  └─ 📄 RelayCommand.cs
   │  ├─ 📂 Resources
   │  │  ├─ 📄 Strings.da.xaml
   │  │  └─ 📄 Strings.en.xaml
   │  ├─ 📂 Views
   │  │  ├─ 📄 GameView.xaml
   │  │  ├─ 📄 GameView.xaml.cs
   │  │  ├─ 📄 OptionsView.xaml
   │  │  ├─ 📄 OptionsView.xaml.cs
   │  │  ├─ 📄 StatistikView.xaml
   │  │  └─ 📄 StatistikView.xaml.cs
   │  └─ 📄 Mastermind.Wpf.csproj
   │
   └─ 📂 Mastermind.Console
      ├─ 📄 Program.cs
      ├─ 📂 UI
      │  ├─ 📄 KonsolMenu.cs
      │  └─ 📄 Spilstyring.cs
      ├─ 📂 Utils
      │  └─ 📄 Statistik.cs
      └─ 📄 Mastermind.Console.csproj

```

---

## 🚀 Kørsel

Du kan køre hvert projekt direkte med `--project`:

**Konsol-app**
```powershell
dotnet run --project "src/Mastermind.Console"
```

**WPF-app**
```powershell
dotnet run --project "src/Mastermind.Wpf"
```

> Alternativt kan løsningen åbnes i Visual Studio og de enkelte startprojekter vælges derfra.

---

## 🧠 Hovedidé & gameplay

- Spillet genererer en hemmelig rækkefølge af farver (længde og forsøg er konfigurerbart via `Options`).  
- Spilleren gætter kombinationer indenfor et maks. antal forsøg.
- Feedback gives med **sorte** (rigtig farve på rigtig plads) og **hvide** (rigtig farve, forkert plads) markeringer.
- Resultater gemmes og kan aflæses i **Statistik** (WPF) og via konsol.

---

## 🧩 Arkitektur & kode

### Core (Mastermind.Core)
- **Domain**  
  `Farver`, `Options`, `Feedback`, `Respons`, `GameResultater` – de centrale domænemodeller.
- **Services**  
  `SecretGenerator` (genererer hemmelig kombination), `Input` (validerer/parsing), `Evaluering` (beregner sort/hvid feedback).
- **Persistence**  
  `OptionsRepository` gemmer/loader `Options` til/fra `Database/Options.json`.  
  `JsonStatistikStore` implementerer `IStatistikStore` og gemmer resultater i `Database/Statistik.json`.  
  `JsonFilePaths` sørger for mappe/sti-opsætning og `EnsureDir()`.
- **Utils**  
  `StatistikTilføjer` – hjælpefunktioner ifm. statistik.

### WPF (Mastermind.Wpf)
- **MVVM**  
  `MainViewModel` (INotifyPropertyChanged) holder `CurrentView` og eksponerer kommandoer:
  `NytSpilCmd`, `OptionsCmd`, `StatistikCmd`, `ExitCmd`.
- **Navigation**  
  Skifter mellem `Views/GameView`, `Views/OptionsView`, `Views/StatistikView` og afslutter via `ExitCmd`.
- **RelayCommand**  
  Let ICommand-implementering til bindinger.
- **Lokalisering**  
  `Resources/Strings.da.xaml` og `Strings.en.xaml` muliggør dansk/engelsk tekst i UI.

### Konsol (Mastermind.Console)
- `Program.cs` opretter `Options` (fx `længde: 3, maxForsøg: 9, showEmojis: true, sprog: Sprog.En`) samt `SecretGenerator`, `Input`, `Evaluering`, `KonsolMenu`, `Statistik`, `JsonStatistikStore` og starter `Spilstyring`.

---

## 💾 Persistens

- **Indstillinger**: `Database/Options.json` (læses/skrives via `OptionsRepository`).  
- **Statistik**: `Database/Statistik.json` (append/reset via `JsonStatistikStore`).  
- Hvis filer/mappen ikke findes, oprettes de ved brug (`EnsureDir`).

---

## 🛠 Krav & værktøjer

- **.NET SDK 9.0** (projekterne målretter `net9.0`/`net9.0-windows`).
- Windows 10/11 for WPF-klienten.
- PowerShell 5.1+ (eller en vilkårlig shell til `dotnet` CLI).

---

## 🌐 Sprog (UI)

WPF-UI understøtter dansk og engelsk via `Resources/Strings.da.xaml` og `Resources/Strings.en.xaml`.
Standard-sproget kan ændres i runtime afhængigt af bindinger/ressourcer (se XAML).

---

## 🧰 Fejlfinding / kendte issues

- **`InitializeComponent` findes ikke**:  
  - Fejl i IDE, kode virker som om de er tilstede og kører. Har ingen ide om hvorfor fejlen er der.
- **Auto-genererede filer**: Nogle builds genererer `*.g.cs` i stedet for `*.g.i.cs` – det er OK.
- **Filsystem-skriverettigheder**: Sørg for, at processen kan skrive til `Database/`.
- **Valideringsproblem i Konsol udgave**: Skal fixes når tid, Konsol har problemer med Sprog.

### Planlægning for næste version
- Fix Sprog problemer med GUI i henhold til Sprog i Tekst og Sprog I GUI.
- Fix Sprog Problemer i Konsol.
- Fix Persistent Statistik i Konsol.

---

## Versions Historik
* 0.5
    * GUI Tilføjet
    * Persistent Statistik tilføjet
    * Persistent Options tilføjet
    * Opdateret README.md
* 0.3
    * Tilføjet statistik
    * Opdateret README.md
    * Opdateret tidligere funktioner for at kunne håndtere statistik
* 0.2
    * Fuld Konsol Funktionalitet
* 0.1
    * Init

---

## 📄 Licens

Se [`LICENSE`](./LICENSE).

## Ressourcer
- [wikihow: How to Play Mastermind](https://www.wikihow.com/Play-Mastermind)
