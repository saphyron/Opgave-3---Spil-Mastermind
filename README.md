# Mastermind (C# konsol)

Tekstbaseret udgave af det klassiske brætspil **Mastermind**. Spillet vælger en hemmelig farvekombination; du har et begrænset antal forsøg til at gætte den. Efter hvert gæt får du feedback i form af **sorte** (rigtig farve + rigtig placering) og **hvide** (rigtig farve, forkert placering) pinde.

---

## Status (seneste)
- ✅ Konsolbaseret spil med **DA/EN** sprog (styres i `Options.sprog`).
- ✅ **Spil igen** efter vundet/tabt runde (`Spilstyring` spørger *ja/nej*).
- ✅ Input-parser accepterer både **danske og engelske farvenavne**.
- ✅ Evaluator håndterer **dubletter korrekt** (ingen dobbelttælling).
- ✅ Feedback kan vises med **emojis** (⚫/⚪) via `Options.showEmojis`.
- ✅ Ugyldigt input tæller **ikke** som et forsøg.
- ⚙️ Default: 4 pladser, 12 forsøg (kan ændres i `Options`).

---

## Krav
- .NET SDK **9.0** (projektet målretter `net9.0`).
- Windows PowerShell 5.1 eller nyere til at køre kommandoerne.

> **Bemærk:** Projektet bruger pt. `Microsoft.NET.Sdk.Web` (oprettet fra Web-skabelon), så `dotnet run` kan forsøge at åbne en browser. Se note under *Kørsel* hvis du kun vil køre i konsollen.

---

## Kørsel

```powershell
# Fra projektmappen (der indeholder csproj-filen)
dotnet build
dotnet run
```

Hvis du vil **undgå at åbne browseren** (pga. Web-SDKens launchSettings), kan du enten:
- midlertidigt køre uden launch profile:
  ```powershell
  dotnet run --no-launch-profile
  ```
- eller ændre `Properties/launchSettings.json` → `"launchBrowser": false` for aktive profiler.
- eller (anbefalet hvis ren konsol): skift `<Project Sdk="Microsoft.NET.Sdk.Web">` til `<Project Sdk="Microsoft.NET.Sdk">` i csproj.

---

## Sådan spilles

- Skriv **præcis 4 farver** adskilt af mellemrum, fx:
  ```
  rød blå grøn gul
  ```
  eller på engelsk:
  ```
  red blue green yellow
  ```
- Gyldige farver (DA/EN): `rød/red`, `grøn/green`, `blå/blue`, `gul/yellow`, `lilla/purple`, `orange/orange`, `hvid/white`, `sort/black`.
- Efter hvert gæt vises fx: `Sort: 2 | Hvid: 1  (⚫⚫⚪)`.
- Når runden er slut, bliver du spurgt: *Spil igen? (ja/nej)* / *Play again? (y/n)*.

---

## Konfiguration (`Options`)

```csharp
var options = new Options(
    længde: 4,         // antal pladser i koden
    maxForsøg: 12,     // maks. forsøg
    showEmojis: true,  // ⚫⚪ feedback
    sprog: Sprog.Da    // Sprog.Da eller Sprog.En
);
```

`Program.cs` viser et simpelt eksempel, hvor `Options` kan justeres direkte.

---

## Kode struktur

```text
📦 Mastermind
├─ 📂 Opgave 3 Mastermind/
│  ├─ 📂 Domain/
│  │  ├─ Farver.cs
│  │  ├─ FarverHelper.cs
│  │  ├─ Feedback.cs
│  │  ├─ Options.cs
│  │  ├─ Respons.cs
│  │  └─ Sprog.cs
│  ├─ 📂 Properties/
│  │  └─ launchSettings.json
│  ├─ 📂 Services/
│  │  ├─ Evaluering.cs
│  │  ├─ Input.cs
│  │  └─ SecretGenerator.cs
│  ├─ 📂 UI/
│  │  ├─ KonsolMenu.cs
│  │  └─ Spilstyring.cs
│  ├─ 📂 Utils/
│  ├─ appsettings.Development.json
│  ├─ appsettings.json
│  ├─ Opgave 3 Mastermind.csproj
│  └─ Program.cs
```

---

## Arkitektur (kort)

- **Domain**: kerne-typer (fx `Farve`, `Feedback`, `Options`, `Sprog`) og `FarverHelper` til parsing/visning.
- **Services**:
  - `SecretGenerator` — genererer hemmelig kode ud fra `Options`.
  - `Input` — validerer/oversætter brugerinput til `Farve[]` (DA/EN).
  - `Evaluering` — beregner sorte/hvide pinde uden dobbelttælling.
- **UI**:
  - `KonsolMenu` — al I/O og tekster (lokaliseret).
  - `Spilstyring` — spilflow: forsøg, win/lose, *spil igen*.
- **Program.cs** — binder det hele sammen.

---

## Test (valgfrit)
Test er ikke inkluderet endnu, og er planlagt til senere når tiden tillader det.

Simpel test kan skabes med følgende:
```powershell
dotnet new xunit -n Mastermind.Tests
dotnet add Mastermind.Tests/Mastermind.Tests.csproj reference "Opgave 3 Mastermind/Opgave 3 Mastermind.csproj"
```

Skriv fx enhedstests for `Evaluering` (sort/hvid-kombinationer inkl. dubletter) og `Input` (gyldige/ugyldige inputs).

---

## Kendte noter
- Projektet blev oprettet med Web-SDK, men kører som **konsol-app**. Det er fint, men hvis du vil undgå web-artefakter (appsettings/launchSettings), så skift til den almindelige `Microsoft.NET.Sdk` i csproj.
- `Utils/` er reserveret til evt. helper-klasser (pt. tom).

---

## Licens
Tilføj licens efter behov (fx MIT).

God fornøjelse — og held og lykke med kombinationen! 🎯


## Ressourcer
- [wikihow: How to Play Mastermind](https://www.wikihow.com/Play-Mastermind)