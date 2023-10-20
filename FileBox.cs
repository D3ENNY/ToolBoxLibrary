using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

using ToolBoxLibrary.InternalFunc;

namespace ToolBoxLibrary.FileBox;
public class FileBox
{
    /// <summary>
    /// Legge un file XML e deserializza i dati in una lista di oggetti del tipo specificato.
    /// </summary>
    /// <typeparam name="T">Il tipo di oggetti da deserializzare.</typeparam>
    /// <param name="path">Il percorso del file XML da leggere.</param>
    /// <exception cref="FileNotFoundException">path passato in input un path di un file non trovato</exception>
    /// <exception cref="InvalidOperationException">si sono verificati errori duranti la serializzazione</exception>
    /// <exception cref="Exception">si è verificato un errore generico</exception>
    /// <returns>
    /// Una lista di oggetti del tipo specificato deserializzati dal file XML. Se si verifica un errore durante la deserializzazione,
    /// verrà restituita una lista vuota.
    /// </returns>
    public List<T> ReadXml<T>(string path)
    {
        List<T> result = new();

        try
        {
            XmlSerializer serializer = new(typeof(List<T>));

            using(FileStream fileStream = new(path, FileMode.Open)){
                result = serializer.Deserialize(fileStream) as List<T> ?? new();
            }
        }
        catch (FileNotFoundException ex)
        {
            ErrorManager.PrintException("Il file specificato non è stato trovato.", ex);
        }
        catch (InvalidOperationException ex)
        {
            ErrorManager.PrintException("Si è verificato un errore durante la deserializzazione.", ex);
        }
        catch (Exception ex)
        {
            ErrorManager.PrintException("Errore generico durante la deserializzazione:", ex);
        }

        return result;
    }

    /// <summary>
    /// Serializza una lista di oggetti e scrive i dati serializzati in un file XML specificato.
    /// </summary>
    /// <typeparam name="T">Il tipo di oggetti contenuti nella lista.</typeparam>
    /// <param name="list">La lista di oggetti da serializzare.</param>
    /// <exception cref="IOException">si sono verificati errori di I/O</exception>
    /// <exception cref="InvalidOperationException">si sono verificati errori duranti la serializzazione</exception>
    /// <exception cref="Exception">si è verificato un errore generico</exception>
    /// <param name="path">Il percorso del file XML in cui scrivere i dati serializzati.</param>
    /// <return> Void </return>
    public void WriteXml<T>(List<T> list, string path) where T : List<T>
    {
        try
        {
            XmlSerializer xml = new(list.GetType());

            using(StreamWriter sw = new(path)){
                xml.Serialize(sw, list);
            }
        }
        catch (InvalidOperationException ex)
        {
            ErrorManager.PrintException("Errore durante la serializzazione.", ex);
        }
        catch (IOException ex)
        {
            ErrorManager.PrintException("Errore di I/O durante la serializzazione.", ex);
        }
        catch (Exception ex)
        {
            ErrorManager.PrintException("Errore generico durante la serializzazione.", ex);
        }
    }

    /// <summary>
    /// Aggiunge un nuovo elemento serializzato in formato XML a un documento XML esistente.
    /// </summary>
    /// <typeparam name="T">Il tipo dell'elemento da serializzare e aggiungere.</typeparam>
    /// <param name="element">L'elemento da serializzare e aggiungere al documento XML.</param>
    /// <param name="path">Il percorso del documento XML esistente in cui aggiungere l'elemento.</param>
    /// <exception cref="FileNotFoundException">tato passato in input un path di un file non trovato<</exception>
    /// <exception cref="XmlException">si sono verificati errori durante il caricamento del documento XML</exception>
    /// <exception cref="Exception">si è verificato un errore generico</exception>
    /// <return> Void </return>
    public void AppendToXml<T>(T element, string path)    
    {
        try{
            XDocument xmlDoc = XDocument.Load(path);
            if(xmlDoc != null && xmlDoc.Root != null && element != null){
                XElement el = new(element.GetType().Name);
                List<PropertyInfo> getters = element.GetType().GetProperties()
                .Where(p => p.CanRead)
                .ToList();
                getters.ForEach(x => {
                    XElement childEl = new XElement(x.Name);
                    childEl.Value = x.GetValue(element)?.ToString() ?? string.Empty;
                    el.Add(childEl);
                });
                xmlDoc.Root.Add(el);
                xmlDoc.Save(path);
            }
        }
        catch (FileNotFoundException ex)
        {
            ErrorManager.PrintException("Errore: Il file specificato non è stato trovato.", ex);
        }
        catch (XmlException ex)
        {
            ErrorManager.PrintException("Errore durante il caricamento del documento XML.", ex);
        }
        catch (Exception ex)
        {
            ErrorManager.PrintException("Errore generico durante l'operazione di append XML.", ex);
        }
    }
}
