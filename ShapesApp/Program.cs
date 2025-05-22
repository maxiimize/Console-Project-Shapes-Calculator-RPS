using SharedLibrary;
using Microsoft.EntityFrameworkCore;
using DataAcessLayer.ModelsShapes;

namespace ShapesApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var menu = new MainMenu();
            menu.Run();
        }
    }
}
