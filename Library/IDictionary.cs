using System;
using System.Collections.Generic;
using System.Text;

namespace algodat_gui
{
    interface IDictionary
    {
        bool insert(int value);
        bool delete(int value);
        bool search(int value);
        void print();
    }
}
