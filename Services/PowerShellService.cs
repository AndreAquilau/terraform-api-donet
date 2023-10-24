using TerraformAPIDotnet.Handlers;
using TerraformAPIDotnet.Util;

namespace TerraformAPIDotnet.Services;

public class PowerShellService
{
    private readonly PowerShellHandler _ps;

    private readonly ILogger<PowerShellService> _logger;

    public PowerShellService(PowerShellHandler ps, ILogger<PowerShellService> logger)
    {
        _ps = ps;
        _logger = logger;
    }

    public PowerShellHandlerResponse ExecuteCommand(string script)
    {
        CleanString cleanString = new CleanString();

        var response = _ps.Command(script);

        response.result = cleanString.RemoveUnicodeANSI(response.result);

        return response;
    }
}

