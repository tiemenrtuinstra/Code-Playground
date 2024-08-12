using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions;

internal interface IConsoleExtensions
{
    public void StartPogram(string appTitle, string appConsoleTitle, string appDescription);

    public void EndProgram();
}
