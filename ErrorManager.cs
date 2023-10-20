namespace ToolBoxLibrary.InternalFunc;

internal class ErrorManager
{
    /// <summary>
    /// Stampa un messaggio di errore e il messaggio dell'eccezione su Console.Error in rosso.
    /// </summary>
    /// <param name="errorMessage">Messaggio di errore personalizzato.</param>
    /// <param name="ex">Eccezione da stampare.</param>
    /// <return> Void </return>
    internal static string PrintException(string error, Exception ex){
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Error.WriteLine($"Errore: {error}\nMessaggio di Error: {ex.Message}");
        Console.ForegroundColor = ConsoleColor.White;
        return "";
    }
}
