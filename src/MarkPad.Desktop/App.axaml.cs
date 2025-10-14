using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MarkPad.Application.Interfaces;
using MarkPad.Application.Services;
using MarkPad.Infrastructure.Services;
using MarkPad.Desktop.Services;
using MarkPad.Desktop.ViewModels;

namespace MarkPad.Desktop;

public partial class App : Avalonia.Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Create main window first to get StorageProvider
            var mainWindow = new MainWindow();
            
            // Setup DI container
            var services = new ServiceCollection();
            
            // Register Infrastructure services
            services.AddSingleton<IFileService, FileService>();
            
            // Register Presentation services (needs StorageProvider from window)
            services.AddSingleton<IDialogService>(new AvaloniaDialogService(mainWindow.StorageProvider));
            services.AddSingleton<IMessageBoxService>(new AvaloniaMessageBoxService(mainWindow));
            
            // Register Application services
            services.AddSingleton<IDocumentService, DocumentService>();
            services.AddSingleton<IDocumentManagerService, DocumentManagerService>();
            
            // Build service provider
            var serviceProvider = services.BuildServiceProvider();
            
            // Create ViewModel with resolved dependencies
            var viewModel = new MainViewModel(
                serviceProvider.GetRequiredService<IDocumentManagerService>()
            );
            
            mainWindow.DataContext = viewModel;
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}
