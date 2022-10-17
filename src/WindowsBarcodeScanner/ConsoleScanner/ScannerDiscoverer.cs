using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Scanners;
using Windows.Foundation;

namespace ConsoleScanner
{
    public class ScannerDiscoverer
    {
        public ScannerDiscoverer()
        {
            InitDeviceWatcher();

            if (_scannerWatcher == null) throw new InvalidOperationException("_scannerWatcher was null at end of constructor");
        }

        public async Task<ImageScanner> GetImageScanner(string deviceId)
        {
            return await ImageScanner.FromIdAsync(deviceId);
        }

        public void StartDiscovery()
        {
            _scannerWatcher.Start();
        }

        public IReadOnlyList<DeviceInformation> Scanners => _devices.Select(kp => kp.Value).ToImmutableList();

        private void InitDeviceWatcher()
        {
            // Create a Device Watcher class for type Image Scanner for enumerating scanners
            _scannerWatcher = DeviceInformation.CreateWatcher(DeviceClass.ImageScanner);

            _scannerWatcher.Added += OnScannerAdded;
            _scannerWatcher.Removed += OnScannerRemoved;
            _scannerWatcher.EnumerationCompleted += OnScannerEnumerationComplete;

            StartDiscovery();
        }

        private void OnScannerAdded(DeviceWatcher sender, DeviceInformation deviceInfo)
        {
            //Console.WriteLine($"Scanner has been added with device id {deviceInfo.Id} and {deviceInfo.Name} - {deviceInfo.Kind} - {deviceInfo.Pairing} - {deviceInfo.EnclosureLocation}");

            _devices.TryAdd(deviceInfo.Id, deviceInfo);
        }

        private void OnScannerRemoved(DeviceWatcher sender, DeviceInformationUpdate deviceInfo)
        {
            //Console.WriteLine($"Scanner Removed {deviceInfo.Id}");

            _devices.Remove(deviceInfo.Id, out var _);
        }

        private void OnScannerEnumerationComplete(DeviceWatcher sender, object deviceInfo)
        {
            Console.WriteLine($"Scanner Enumeration Complete: {sender.Status}");

        }

        DeviceWatcher _scannerWatcher;
        ConcurrentDictionary<string, DeviceInformation> _devices = new ConcurrentDictionary<string, DeviceInformation>();
    }
}
