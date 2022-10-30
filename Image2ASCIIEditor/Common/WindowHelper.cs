using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Runtime.InteropServices; // For DllImport
using WinRT; // required to support Window.As<ICompositionSupportsSystemBackdrop>()
using Microsoft.UI.Composition.SystemBackdrops;
using Microsoft.UI;

namespace Image2ASCIIEditor.Common;
public class Acrylic
{

    WindowsSystemDispatcherQueueHelper m_wsdqHelper; // See separate sample below for implementation
    Microsoft.UI.Composition.SystemBackdrops.MicaController m_micaController;
    Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController m_acrylicController;
    Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration m_configurationSource;

    public bool TrySetAcrylicBackdrop(ref Window window)
    {
        if (Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController.IsSupported())
        {
            m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Hooking up the policy object
            m_configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
            window.Activated += Window_Activated;
            window.Closed += Window_Closed;
            

            // Initial configuration state.
            m_configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme(ref window);

            m_acrylicController = new Microsoft.UI.Composition.SystemBackdrops.DesktopAcrylicController();
            m_acrylicController.TintColor = Colors.Transparent;
            m_acrylicController.TintOpacity = 0F;
            m_acrylicController.LuminosityOpacity = 0F;
            // Enable the system backdrop.
            // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
            m_acrylicController.AddSystemBackdropTarget(window.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            m_acrylicController.SetSystemBackdropConfiguration(m_configurationSource);
            return true; // succeeded
        }

        return false; // Acrylic is not supported on window system
    }

    private void Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        // Make sure any Mica/Acrylic controller is disposed so it doesn't try to
        // use window closed window.
        Window window = sender as Window;

        if (m_acrylicController != null)
        {
            m_acrylicController.Dispose();
            m_acrylicController = null;
        }
        window.Activated -= Window_Activated;
        m_configurationSource = null;
    }



    private void SetConfigurationSourceTheme(ref Window window)
    {
        switch (((FrameworkElement)window.Content).ActualTheme)
        {
            case ElementTheme.Dark: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
            case ElementTheme.Light: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
            case ElementTheme.Default: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
        }
    }
}

public class Mica
{

    WindowsSystemDispatcherQueueHelper m_wsdqHelper; // See separate sample below for implementation
    Microsoft.UI.Composition.SystemBackdrops.MicaController m_micaController;
    Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration m_configurationSource;

    public bool TrySetMicaBackdrop(ref Window window)
    {
        if (Microsoft.UI.Composition.SystemBackdrops.MicaController.IsSupported())
        {
            m_wsdqHelper = new WindowsSystemDispatcherQueueHelper();
            m_wsdqHelper.EnsureWindowsSystemDispatcherQueueController();

            // Hooking up the policy object
            m_configurationSource = new Microsoft.UI.Composition.SystemBackdrops.SystemBackdropConfiguration();
            window.Activated += Window_Activated;
            window.Closed += Window_Closed;
            

            // Initial configuration state.
            //m_configurationSource.IsInputActive = true;
            SetConfigurationSourceTheme(ref window);

            m_micaController = new Microsoft.UI.Composition.SystemBackdrops.MicaController();

            // Enable the system backdrop.
            // Note: Be sure to have "using WinRT;" to support the Window.As<...>() call.
            m_micaController.AddSystemBackdropTarget(window.As<Microsoft.UI.Composition.ICompositionSupportsSystemBackdrop>());
            m_micaController.SetSystemBackdropConfiguration(m_configurationSource);
            return true; // succeeded
        }

        return false; // Mica is not supported on window system
    }

    private void Window_Activated(object sender, WindowActivatedEventArgs args)
    {
        m_configurationSource.IsInputActive = args.WindowActivationState != WindowActivationState.Deactivated;
    }

    private void Window_Closed(object sender, WindowEventArgs args)
    {
        Window window = sender as Window;
        // Make sure any Mica/Acrylic controller is disposed so it doesn't try to
        // use window closed window.
        if (m_micaController != null)
        {
            m_micaController.Dispose();
            m_micaController = null;
        }
        window.Activated -= Window_Activated;
        m_configurationSource = null;
    }


    private void SetConfigurationSourceTheme(ref Window window)
    {
        switch (((FrameworkElement)window.Content).ActualTheme)
        {
            case ElementTheme.Dark:    m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Dark; break;
            case ElementTheme.Light:   m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Light; break;
            case ElementTheme.Default: m_configurationSource.Theme = Microsoft.UI.Composition.SystemBackdrops.SystemBackdropTheme.Default; break;
        }
    }

}
