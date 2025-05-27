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

