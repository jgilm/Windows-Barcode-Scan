using ConsoleScanner;
using System.Threading;
using System;
using Windows.ApplicationModel.UserDataTasks.DataProvider;
using Windows.Devices.Scanners;
using Windows.Storage;

Console.WriteLine("Hello, World!");


ScannerDiscoverer ScannerDiscoverer = new ScannerDiscoverer();


Console.WriteLine("Enumerator started - Press any key to select scanner");
Console.ReadKey(true);

var scannerList = ScannerDiscoverer.Scanners;

for (int i = 0; i < scannerList.Count; i++)
{
    var scanner = scannerList[i];
    Console.WriteLine($"{i}. {scanner.Name}");
}

Console.WriteLine("Select the Scanner to scan from");
var key = Console.ReadKey(true);

if (int.TryParse(key.KeyChar.ToString(), out int scannerNumber))
{
    var di = scannerList[scannerNumber];
    Console.WriteLine($"Scanning from {di.Name} - {di.Kind} - {di.Pairing}");
    var imageScanner = await ScannerDiscoverer.GetImageScanner(di.Id);

    // Set configuration to support multiple pages
    imageScanner.FeederConfiguration.Format = ImageScannerFormat.Png;

    DisplayFeederConfigurations(imageScanner);


    var storageFolder = await StorageFolder.GetFolderFromPathAsync(@"C:\dev\");

    var progress = new Progress<UInt32>(ScanProgress);
    var result = await imageScanner.ScanFilesToFolderAsync(ImageScannerScanSource.Feeder,
        storageFolder).AsTask(CancellationToken.None, progress);

    Console.WriteLine("After scan to files called");

    Console.WriteLine($"Number of files dropped {result.ScannedFiles.Count}");

}

void ScanProgress(uint progress)
{
    Console.WriteLine($"Scan Progress: {progress}");
}

void DisplayFlatbedConfigurations(ImageScanner scanner)
{
    Console.WriteLine("Flatbed Configuration");
    var fb = scanner.FlatbedConfiguration;

    Console.WriteLine($"Actual Resolution       : {DisplayResolution(fb.ActualResolution)}");
    Console.WriteLine($"Auto Cropping Mode      : {fb.AutoCroppingMode}");
    Console.WriteLine($"Brightness              : {fb.Brightness}");
    Console.WriteLine($"Brightness Step         : {fb.BrightnessStep}");
    Console.WriteLine($"Color Mode              : {fb.ColorMode}");
    Console.WriteLine($"Contrast                : {fb.Contrast}");
    Console.WriteLine($"Contrast Step           : {fb.ContrastStep}");
    Console.WriteLine($"Default Brightness      : {fb.DefaultBrightness}");
    Console.WriteLine($"Default ColorMode       : {fb.DefaultColorMode}");
    Console.WriteLine($"Default Contrast        : {fb.DefaultContrast}");
    Console.WriteLine($"Default Format          : {fb.DefaultFormat}");
    Console.WriteLine($"Desired Resolution      : {DisplayResolution(fb.DesiredResolution)}");
    Console.WriteLine($"Format                  : {fb.Format}");
    Console.WriteLine($"Max Brightness          : {fb.MaxBrightness}");
    Console.WriteLine($"Max Contrast            : {fb.MaxContrast}");
    Console.WriteLine($"Max Resolution          : {DisplayResolution(fb.MaxResolution)}");
    Console.WriteLine($"Max ScanArea            : {fb.MaxScanArea}");
    Console.WriteLine($"Min Brightness          : {fb.MinBrightness}");
    Console.WriteLine($"Min Resolution          : {DisplayResolution(fb.MinResolution)}");
    Console.WriteLine($"Min ScanArea            : {fb.MinScanArea}");
    Console.WriteLine($"Optical Resolution      : {DisplayResolution(fb.OpticalResolution)}");
    Console.WriteLine($"Selected Scan Region    : {fb.SelectedScanRegion}");
}

void DisplayFeederConfigurations(ImageScanner scanner)
{
    Console.WriteLine("FEEDER Configuration");
    var fc = scanner.FeederConfiguration;

    Console.WriteLine($"Actual Resolution           : {DisplayResolution(fc.ActualResolution)}");
    Console.WriteLine($"Auto Cropping Mode          : {fc.AutoCroppingMode}");
    Console.WriteLine($"Auto Detect Page Size       : {fc.AutoDetectPageSize}");
    Console.WriteLine($"Brightness                  : {fc.Brightness}");
    Console.WriteLine($"Brightness Step             : {fc.BrightnessStep}");
    Console.WriteLine($"Can Auto Detect Page Size   : {fc.CanAutoDetectPageSize}");
    Console.WriteLine($"Can Scan Ahead              : {fc.CanScanAhead}");
    Console.WriteLine($"Can Scan Duplex             : {fc.CanScanDuplex}");
    Console.WriteLine($"Color Mode                  : {fc.ColorMode}");
    Console.WriteLine($"Contrast                    : {fc.Contrast}");
    Console.WriteLine($"Contrast Step               : {fc.ContrastStep}");
    Console.WriteLine($"Default Brightness          : {fc.DefaultBrightness}");
    Console.WriteLine($"Default ColorMode           : {fc.DefaultColorMode}");
    Console.WriteLine($"Default Contrast            : {fc.DefaultContrast}");
    Console.WriteLine($"Default Format              : {fc.DefaultFormat}");
    Console.WriteLine($"Desired Resolution          : {DisplayResolution(fc.DesiredResolution)}");
    Console.WriteLine($"Duplex                      : {fc.Duplex}");
    Console.WriteLine($"Format                      : {fc.Format}");
    Console.WriteLine($"Max Brightness              : {fc.MaxBrightness}");
    Console.WriteLine($"Max Contrast                : {fc.MaxContrast}");
    Console.WriteLine($"Max Resolution              : {DisplayResolution(fc.MaxResolution)}");
    Console.WriteLine($"Max Number of Pages         : {fc.MaxNumberOfPages}");
    Console.WriteLine($"Max ScanArea                : {fc.MaxScanArea}");
    Console.WriteLine($"Min Brightness              : {fc.MinBrightness}");
    Console.WriteLine($"Min Resolution              : {DisplayResolution(fc.MinResolution)}");
    Console.WriteLine($"Min ScanArea                : {fc.MinScanArea}");
    Console.WriteLine($"Optical Resolution          : {DisplayResolution(fc.OpticalResolution)}");
    Console.WriteLine($"Page Orientation            : {fc.PageOrientation}");
    Console.WriteLine($"Page Size                   : {fc.PageSize}");
    Console.WriteLine($"Page Size Dimensions        : {fc.PageSizeDimensions}");
    Console.WriteLine($"Scan Ahead                  : {fc.ScanAhead}");
    Console.WriteLine($"Selected Scan Region        : {fc.SelectedScanRegion}");



    
    

}

string DisplayResolution(ImageScannerResolution resolution)
{
    return $"DPI X: {resolution.DpiX} - DPI Y: {resolution.DpiY}";
}


Console.WriteLine("-------");
Console.WriteLine("Program Done");


