14/05-2025
-----------

Idag påbörjade jag arbetet med projekten Shapes, Kalkylator och Sten Sax Påse. Först skapade jag samtliga projekt i en gemensam solution och satte upp ett repository på GitHub. 

Jag skapade också en separat gren (dev) för pågående utveckling för att säkerställa att ny kod testas ordentligt innan den pushas till den stabila master-grenen.

Huvudfokus denna veckas sprint är huvudmenyn, så jag började med att skapa ett class library för att förbättra filstruktur och hantering. 

Där skapade jag sedan klassen MainMenu.cs, där jag definierade menyalternativen som en lista och skrev ut dem i konsolen. 

Jag implementerade också navigeringen för att enkelt kunna starta respektive projekt.

Slutligen förbättrade jag användargränssnittet med hjälp av Spectre.Console för att göra upplevelsen mer användarvänlig, visuellt attraktiv och för att underlätta enklare validering av användarens val.




22-23/05-2025
-----------

Idag påbörjade jag arbetet med Shapes-modulen. Först skapade jag samtliga modellklasser (Shape, Rectangle, Parallelogram, Triangle, Rhombus) i DataAccessLayer-projektet, och satte upp databaskontexten (AllDbContext) med hjälp av Entity Framework Core och Code First-principen. Anledningen till att jag valde Code First var för att snabbt kunna definiera modellerna direkt i kod och låta ramverket automatiskt skapa databasen, vilket underlättar framtida förändringar av modellerna.

Efter detta genererade jag de nödvändiga migrationerna och uppdaterade databasen för att återspegla dessa förändringar. Att regelbundet migrera databasen efter varje ändring gör att jag kan undvika framtida problem med dataintegritet.

Nästa steg var att implementera grundläggande CRUD-funktionalitet (Create, Read, Update, Delete) för shapes i konsolapplikationen. Jag skapade därför en separat menyklass (ShapesMenu) som hanterar samtliga CRUD-operationer. Jag valde att använda Spectre.Console eftersom det ger ett modernt och tydligt gränssnitt, vilket förbättrar användarupplevelsen markant.