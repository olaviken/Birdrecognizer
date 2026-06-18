# Birdrecognizer - Bildegjenkjenning med ML.NET og C#

### Kort oppsummering
Dette prosjektet er utviklet for å utforske og demonstrere praktisk bruk av ML.NET-rammeverket og bildeklassifisering i et .NET-miljø.
Applikasjonen kan identifisere 14 ulike norske fuglearter ved hjelp av en trent maskinlæringsmodell og presenterer detaljert informasjon om arten gjennom en integrert Wikipedia-løsning.

### Tekniske Høydepunkter

* **Maskinlæring:** Bruker Microsoft.ML og Microsoft.ML.Vision for å trene og kjøre en bildeklassifiseringsmodell basert på TensorFlow.
* **Arkitektur:** Prosjektet er modulært bygget opp med tydelig skille mellom logikk for AI-konsumering, filhåndtering og grensesnitt.
* **Grensesnitt (GUI):** Utviklet i WPF med et brukervennlig design som inkluderer bildevisning og interaktive faner.
* **API- og Web-integrasjon:** Bruker WebView2 for å søke opp og vise sanntidsdata fra Wikipedia (både norsk og engelsk) basert på modellens resultat.

### Slik fungerer det
* **Analyse:** Brukeren laster opp et bilde, og systemet analyserer bildefunksjonene mot den trente modellen.
* **Identifisering:** Applikasjonen returnerer den mest sannsynlige fuglearten (label) sammen med en sannsynlighetsberegning.
* **Kunnskapsinnhenting:** Ved hjelp av fuglens vitenskapelige navn genereres automatiske forespørsler til Wikipedia, slik at brukeren får umiddelbar tilgang til utfyllende informasjon.

### Teknologier brukt
* **Programmeringsspråk:** C# (100%)
* **Rammeverk:** .NET, ML.NET, WPF
* **Biblioteker:** WebView2, Ookii.Dialogs, SciSharp.TensorFlow.Redist

### Potensielle bruksområder
Utover fuglegjenkjenning demonstrerer arkitekturen hvordan lignende løsninger kan implementeres for:
* Industriell kvalitetskontroll.
* Automatisert produktkategorisering.
* Sikkerhet og overvåking.
