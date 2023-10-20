using ToolBoxLibrary.InternalFunc;

namespace ToolBoxLibrary.TextBox;

public static class StringBox
{
    /// <summary>
    /// Capitalizza la prima lettera di una stringa e restituisce il risultato.
    /// </summary>
    /// <param name="str">La stringa da capitalizzare.</param>
    /// <exception cref="ArgumentNullException">Genera un'eccezione se la stringa di input è nulla.</exception>
    /// <exception cref="ArgumentException">Genera un'eccezione se la stringa di input è vuota.</exception>
    /// <returns>
    /// La stringa di input con la prima lettera maiuscola, oppure una stringa vuota se la stringa di input è nulla o vuota.
    /// </returns>
    public static string Capitalize(this string str) => 
            str == null
            ? ErrorManager.PrintException("la stringa in input è null", new ArgumentNullException(nameof(str)))
            : str == ""
            ? ErrorManager.PrintException("la stringa in input è vuota", new ArgumentException(nameof(str) + " can not be white"))
            : string.Concat(str[0].ToString().ToUpper(), str.AsSpan(1));
}
