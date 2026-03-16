using System;
using System.Collections.Generic;
using System.Text;

namespace Helis.Files.Generator
{
    internal interface ILineGenerator
    {
        string GenerateLineFromList();
        string GenerateRandomLine();
    }
}
