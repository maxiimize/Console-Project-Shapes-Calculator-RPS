14/05-2025
-----------

Idag påbörjade jag arbetet med projekten Shapes, Kalkylator och Sten Sax Påse. Först skapade jag samtliga projekt i en gemensam solution och satte upp ett repository på GitHub. 

Jag skapade också en separat gren (dev) för pågående utveckling för att säkerställa att ny kod testas ordentligt innan den pushas till den stabila master-grenen.

Huvudfokus denna veckas sprint är huvudmenyn, så jag började med att skapa ett class library för att förbättra filstruktur och hantering. 

Där skapade jag sedan klassen MainMenu.cs, där jag definierade menyalternativen som en lista och skrev ut dem i konsolen. 

Jag implementerade också navigeringen för att enkelt kunna starta respektive projekt.

Slutligen förbättrade jag användargränssnittet med hjälp av Spectre.Console för att göra upplevelsen mer användarvänlig, visuellt attraktiv och för att underlätta enklare validering av användarens val.




22/05-2025
-----------

Idag påbörjade jag arbetet med Shapes-modulen. Först skapade jag samtliga modellklasser (Shape, Rectangle, Parallelogram, Triangle, Rhombus) i DataAccessLayer-projektet, och satte upp databaskontexten (AllDbContext) med hjälp av Entity Framework Core och Code First-principen. Anledningen till att jag valde Code First var för att snabbt kunna definiera modellerna direkt i kod och låta ramverket automatiskt skapa databasen, vilket underlättar framtida förändringar av modellerna.

Efter detta genererade jag de nödvändiga migrationerna och uppdaterade databasen för att återspegla dessa förändringar. Att regelbundet migrera databasen efter varje ändring gör att jag kan undvika framtida problem med dataintegritet.

Nästa steg var att implementera grundläggande CRUD-funktionalitet (Create, Read, Update, Delete) för shapes i konsolapplikationen. Jag skapade därför en separat menyklass (ShapesMenu) som hanterar samtliga CRUD-operationer. Jag valde att använda Spectre.Console eftersom det ger ett modernt och tydligt gränssnitt, vilket förbättrar användarupplevelsen markant.




26/05-2025
-----------

Idag lade jag till stöd för avbrytnings­alternativet (0) i både uppdaterings- och raderings­metoderna i ShapesMenu. 

Detta gör att användaren kan avbryta och återgå till menyn snabbt om man ångrar sig eller valt fel utan att oavsiktligt göra ändringar i databasen. 

Genom att tydligt informera om avbryt­funktionen i prompt­texten och uppdatera valideringen säkerställde jag en robust flödeskontroll och minskar risken för felaktiga Id-inmatningar.

Jag förbättrade även CreateShape-flödet så att den nyskapade formen omedelbart visas i en tabell med kolumner för Id, typ, parametrar, area, omkrets och skapandedatum. 

Den direkta visuella återkopplingen ger användaren trygghet i att formen sparats korrekt och att beräkningarna är korrekta, samtidigt som det underlättar felsökning och verifiering.



27/05-2025
-----------
Idag påbörjade jag kalkylator-modulen. Jag kapslade in all kalkylatorlogik i klassen jag skapade, CalculatorMenu för att hålla koden modulär och återanvändbar, och injicerar AllDbContext via DI för att använda samma databaskontext som Shapes och RPS. 

Jag visar resultatet direkt i en Spectre.Console-tabell för omedelbar visuell återkoppling, vilket ökar användarens förtroende och underlättar felsökning. 

ID-valideringen inför jag för att förhindra felaktiga uppdateringar eller raderingar, och soft delete med IsDeleted plus globalt filter skyddar mot oavsiktlig databorttagning. Tillsammans ger dessa val ett strukturerat, robust och användarvänligt flöde som är lätt att underhålla och vidareutveckla.



05/06-2025
-----------

Idag jobbade jag först med att förbättra valideringen och felmeddelandena i kalkylator- och shapes-modulerna för att undvika ogiltiga inmatningar och ge användaren tydliga instruktioner (t.ex. undvika division med noll och för stora värden). 

Syftet var att säkerställa dataintegritet och ett robust användarflöde innan jag gick vidare.

Därefter uppdaterade jag RPS-modellen genom att lägga till WinRate i DataAccessLayer.RPS och justera AllDbContext för att skapa rätt kolumn i databasen. 

Sedan körde jag migrationen och kontrollerade i databasen att kolumnen lagts till korrekt. Detta gjordes för att kunna spara löpande vinstprocent i varje spelrad.

Jag skapade sedan RpsMenu i SharedLibrary med fyra alternativ: spela nytt spel, visa tidigare spel, visa statistik, och tillbaka till huvudmenyn. 

Därefter injicerade jag RpsMenu i DI, uppdaterade huvudmenyn så att RPS-modulen körs direkt istället för som placeholder.

I metoden för att spela ett nytt spel samlades datorns drag, utfall och vinstprocent in och sparades som en ny rad i tabellen. 

Listningsmetoden visade alla tidigare omgångar. Statistikalternativet summerade totalt antal spel, vinster och förluster, och beräknade procentuell vinst både totalt och per drag (Sten/Sax/Påse).



06/06-2025
-----------

Idag började jag med att skapa ViewModels för alla projekt för att tydligt separera datalagret från presentationen och undvika att exponera EF-entiteter direkt i konsolen, vilket gör koden både mer underhållbar och säkrare vid framtida ändringar. 

Därefter utökade jag RPS-statistiken med en kolumn för “Oavgjort”. 

Det var viktigt för att spelaren ska få en komplett bild av alla utfall, inte bara vinster och förluster.

Jag gick sedan igenom alla menyer och översatte all synlig text till svenska för att ge en konsekvent och professionell användarupplevelse.

Blandade språk kan skapa förvirring, så genom att anpassa varnings- och instruktions­texter till svenska säkerställde jag att användaren lätt förstår varje steg.

Nästa steg var att byta till Autofac-baserad Dependency Injection. Genom att ersätta Microsofts inbyggda DI-container med Autofac blev det tydligare hur beroenden hanteras, och koden blev mer modulär och lättare att konfigurera senare. 

Detta val stöder SOLID-principerna om löskoppling och gör det enkelt att byta ut eller lägga till tjänster i framtiden.

Slutligen refaktorerade jag kalkylatorn med Strategy-mönstret genom att införa ICalculationStrategy och en separat klass för varje operator, vilket isolerar beräkningslogiken från menyerna och underlättar för framtida tillägg, exempelvis exponentiering, utan att behöva röra huvudkoden. 

Avrundningsvis genomförde jag en slutlig testning av hela applikationen—Shapes-CRUD, Kalkylatorn och RPS-flödet—för att säkerställa att både DI-ändringarna och strategiimplementeringen fungerar som de ska och att inga nya buggar uppstått efter refaktoreringen.
