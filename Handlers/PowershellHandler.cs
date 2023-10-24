using System.Management.Automation;
using System.Text;

namespace TerraformAPIDotnet.Handlers;

public class PowerShellHandler
{
    private readonly PowerShell _ps = PowerShell.Create();

    public PowerShellHandlerResponse Command(string script)
    {
        string errorMsg = string.Empty;
        string output = string.Empty;
        
        Console.WriteLine();
        Console.WriteLine(script);
        Console.WriteLine();
        

        _ps.AddScript(script);

        _ps.AddCommand("Out-String");

        PSDataCollection<PSObject> outputCollection = new();

        _ps.Streams.Error.DataAdded += (object s, DataAddedEventArgs e) =>
        {
            Console.WriteLine(((PSDataCollection<ErrorRecord>)s)[e.Index].ToString());

            errorMsg += ((PSDataCollection<ErrorRecord>)s)[e.Index].ToString();
        };

        IAsyncResult result = _ps.BeginInvoke<PSObject, PSObject>(null, outputCollection);

        _ps.EndInvoke(result);

        StringBuilder sb = new();

        foreach (var outputItem in outputCollection)
        {
            sb.AppendLine(outputItem.ToString());
        }

        _ps.Commands.Clear();

        if (!string.IsNullOrEmpty(errorMsg))
        {
            PowerShellHandlerResponse resultError = new PowerShellHandlerResponse()
            {
                error = true,
                result = errorMsg
            };

            Console.WriteLine(errorMsg);
            return resultError;
        }

        PowerShellHandlerResponse resultSuccess = new PowerShellHandlerResponse()
        {
            error = false,
            result = sb.ToString()
        };

        Console.WriteLine(sb.ToString());

        return resultSuccess;
    }
}

public class PowerShellHandlerResponse
{
    public bool error { get; set; } = false;
    public string result { get; set; } = string.Empty;
}
