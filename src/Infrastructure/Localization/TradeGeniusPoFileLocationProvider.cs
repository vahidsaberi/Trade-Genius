using TradeGenius.WebApi.Infrastructure.Common.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using OrchardCore.Localization;

namespace TradeGenius.WebApi.Infrastructure.Localization;

/// <summary>
/// Provides PO files for Trade-Genius Localization.
/// </summary>
public class TradeGeniusPoFileLocationProvider : ILocalizationFileLocationProvider
{
    private readonly IFileProvider _fileProvider;
    private readonly string _resourcesContainer;

    public TradeGeniusPoFileLocationProvider(IHostEnvironment hostingEnvironment, IOptions<LocalizationOptions> localizationOptions)
    {
        _fileProvider = hostingEnvironment.ContentRootFileProvider;
        _resourcesContainer = localizationOptions.Value.ResourcesPath;
    }

    public IEnumerable<IFileInfo> GetLocations(string cultureName)
    {
        // Loads all *.po files from the culture folder under the Resource Path.
        // for example, src\Host\Localization\en-US\Trade-Genius.Exceptions.po
        foreach (var file in _fileProvider.GetDirectoryContents(PathExtensions.Combine(_resourcesContainer, cultureName)))
        {
            yield return file;
        }
    }
}
