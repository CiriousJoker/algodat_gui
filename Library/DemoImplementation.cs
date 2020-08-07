using System;

namespace algodat_gui.Library
{
    class DemoImplementation : IDictionary
    {
        public bool delete(int value)
        {
            Console.WriteLine("I'm trying so hard right now...");
            return true;
        }

        public bool insert(int value)
        {
            Console.WriteLine("Inserting... I swear!");
            return true;
        }

        public void print()
        {
            Console.WriteLine("Surely there must be something in here...");
        }

        public bool search(int value)
        {
            Console.WriteLine("Found something! Maybe? Idk, I'm just a demo implementation...");
            return true;
        }
    }
}
