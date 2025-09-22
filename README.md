# Mastermind - Konsolbaseret Spil (C#)

## Projektbeskrivelse
Dette projekt er en konsolbaseret version af det klassiske brætspil Mastermind, implementeret i C#. Spillet genererer en hemmelig farvekode, som spilleren skal gætte ved at indtaste farver. Efter hvert gæt gives der feedback om antallet af korrekte farver og korrekte placeringer.

## Funktionalitet
- Spillet vælger en tilfældig kombination af fire farver fra listen: `red`, `green`, `yellow`, `blue`, `purple`, `orange`, `white`, `black`.
- Spilleren indtaster et gæt bestående af fire farver.
- Input valideres for korrekt antal og gyldighed af farver.
- Feedback gives efter hvert gæt:
    - Sort pind (⚫): korrekt farve på korrekt placering.
    - Hvid pind (⚪): korrekt farve på forkert placering.
- Spilleren har op til 12 forsøg til at gætte koden.
- Spillet afsluttes ved korrekt gæt eller når alle forsøg er brugt.

## Sådan kører du programmet
1. Sørg for at have .NET SDK installeret.
2. Download kildekoden fra GitHub-repositoriet.
3. Åbn en terminal i projektmappen og kør:
    ```bash
    dotnet run
    ```
4. Følg instruktionerne i konsollen for at spille.

## Eksempel på gameplay
```
Indtast dit gæt (fire farver adskilt af mellemrum): red blue green yellow
Feedback: ⚫⚪
Forsøg tilbage: 11
```

## Teknologi og kodeelementer
- Programmet er skrevet i C#.
- Funktioner til tilfældig farvekode, inputvalidering, feedback og spilstyring.
- Koden er kommenteret for bedre forståelse.

## Mulige forbedringer
- Udvidelse med grafisk brugergrænseflade (GUI).
- Mulighed for at vælge antal farver og pladser.
- Gemme highscore eller statistik.

## Ressourcer
- [wikihow: How to Play Mastermind](https://www.wikihow.com/Play-Mastermind)
