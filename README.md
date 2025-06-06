Console Project – Shapes, Calculator & RPS
Detta projekt är en C#/.NET-konsolapplikation där du kan beräkna areor för olika geometriska figurer, använda en miniräknare samt spela Sten-Sax-Påse mot datorn. Projektet är byggt enligt moderna principer och metoder, och visar god arkitektur och kodstruktur. Det är framtaget som en del av en kursuppgift inom systemutveckling.

## Innehållsförteckning
- [Beskrivning av projektet](#beskrivning-av-projektet)
- [Tekniker och arkitektur](#tekniker-och-arkitektur)
- [Design och UX](#design-och-ux)
- [Metoder, patterns och principer](#metoder-patterns-och-principer)
- [Viktiga funktioner](#viktiga-funktioner)
- [Trello Board](#trello-board)
- [Installation](#installation)

## Beskrivning av projektet
Denna applikation erbjuder tre huvudsakliga funktioner:

Shapes: Beräkna area och omkrets för rektangel, triangel, parallellogram och romb.

Calculator: Utför grundläggande matematiska beräkningar (addition, subtraktion, multiplikation, division, modulus, kvadratroten).

Rock-Paper-Scissors (RPS): Spela Sten-Sax-Påse mot datorn. Statistik och historik sparas.

Syftet är att träna på:

Arkitektur enligt lagerprinciper (separation av presentation, logik och data)

Att använda design patterns och professionell kodstruktur

Databasanvändning och best practices

## Tekniker och arkitektur
C# / .NET 9 – Grund för applikationen

Entity Framework Core – För databasåtkomst (Code First)

SQL Server – Databasmotor

Autofac – Dependency Injection (IoC-container)

Spectre.Console – För bättre konsolupplevelse (menyer, tabeller, färg)

GitHub – Versionshantering, feature branches och beskrivande commit-meddelanden

Projektet är uppdelat i flera lager:

Presentation: Hanterar användargränssnitt och menyer

Logik: Hanterar all logik

Data: Entity Framework Core, DbContext och migrations

## Design och UX
Konsolgränssnittet använder Spectre.Console för att ge en mer professionell känsla.

Tydlig och lättnavigerad huvudmeny, logiskt flöde mellan delarna.

Presentationen av resultat och statistik sker i tabellform med färg och tydlighet.

Inputvalidering för att undvika felaktig inmatning.

## Metoder, patterns och principer
Projektet bygger på flera professionella metoder och designprinciper:

Objektorienterad programmering (OOP) – Små, fokuserade klasser/metoder.

Lagerarkitektur (Layered Architecture) – Tydlig separation av ansvar.

Dependency Injection (DI) – Lös koppling, enkelt att byta ut komponenter, möjliggör testbarhet.

Strategy Pattern – För kalkylatorns olika beräkningar.

Data Transfer Objects (DTOs) & ViewModels – Tydlig transport av data mellan lager.

DRY-principen – Ingen kodduplicering.

Separation of Concerns – Presentation, logik och data är separerat.

ModelState/Validering – Förhindrar felaktig inmatning.

Scrum (vid större projekt) – Arbetssätt med sprintar, dagliga standups och retrospectives.

## Viktiga funktioner
CRUD på shapes/kalkylatorhistorik/rps-spelomgångar (Create, Read, Delete)

Automatisk beräkning av area och omkrets för olika former

Olika strategier för kalkylatorn (exempelvis kvadratroten för endast ett tal)

Spara och visa historik/statistik för RPS-matcher

Visning av topplistor/statistik på matcher (RPS)

Inputvalidering överallt

Användarvänligt, tydligt och robust konsolgränssnitt


## Trello Board
Länk till min Trello board för arbetsuppföljning:
https://trello.com/invite/b/681a6b9eb373b37621f5afc9/ATTI6116934026d48aa2bdbafb5880971a7cF824F954/former-miniraknare-och-sten-sax-och-pase-console-project

## Installation
Klona detta repository via denna länk: https://github.com/maxiimize/Console-Project-Shapes-Calculator-RPS.git

Öppna projektet i Visual Studio

Bygg projektet
