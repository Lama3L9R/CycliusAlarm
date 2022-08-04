using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace CycliusAlarm;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private const double DiamondOscillationFactor = Math.Tau / 3;
    private const double RubyOscillationFactor = Math.Tau / 12;
    private const double JadeOscillationFactor = Math.Tau / 24;
    private const double MaximumIncrement = 0.15;

    // Since these timings are too short for a exchange refill(1hour) so I decide to exclude them.
    private static readonly List<(double begin, double end)> IgnoredLeadingTiming = new() { (3.6, 4), (15.6, 16) };

    private readonly DispatcherTimer _TimerService = new() { Interval = TimeSpan.FromSeconds(1) };

    private int _timezone = 0;
    
    public MainWindow()
    {
        InitializeComponent();

        this._TimerService.Tick += OnTimeTick;
        LaunchSwitchingTimer();
    }

    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        _timezone = ComboBox.SelectedIndex - 12;
    }

    private void LaunchSwitchingTimer()
    {
        this._TimerService.Start();
    }

    private void StopSwitchingTimer()
    {
        this._TimerService.Stop();
    }

    private void OnTimeTick(object? sender, EventArgs args)
    {
        var curr = GetCurrentTimeInHours();
        var diamond = CalculateCurrentDiamond(this._timezone);
        var ruby = CalculateCurrentRuby(this._timezone);
        var jade = CalculateCurrentJade(this._timezone);
        
        this.Dispatcher.Invoke(() =>
            {
                DiamondPercentage.Content = $"{diamond * 100:+##.00;-##.00;0.00}%";
                if (diamond > 0)
                {
                    DiamondPercentage.Foreground = new SolidColorBrush(Color.FromRgb(0x09, 0x89, 0x56));
                }
                else if (diamond < 0)
                {
                    DiamondPercentage.Foreground = new SolidColorBrush(Color.FromRgb(0xCD, 0x04, 0x04));
                }
                else
                {
                    DiamondPercentage.Foreground = new SolidColorBrush(Color.FromRgb(00, 0xA8, 0xFF));
                }
    
                RubyPercentage.Content = $"{ruby * 100:+##.00;-##.00;0.00}%";
                if (ruby > 0)
                {
                    RubyPercentage.Foreground = new SolidColorBrush(Color.FromRgb(0x09, 0x89, 0x56));
                }
                else if (ruby < 0)
                {
                    RubyPercentage.Foreground = new SolidColorBrush(Color.FromRgb(0xCD, 0x04, 0x04));
                }
                else
                {
                    RubyPercentage.Foreground = new SolidColorBrush(Color.FromRgb(00, 0xA8, 0xFF));
                }
                
                JadePercentage.Content = $"{jade * 100:+##.00;-##.00;0.00}%";
                
                if (jade > 0)
                {
                    JadePercentage.Foreground = new SolidColorBrush(Color.FromRgb(0x09, 0x89, 0x56));
                }
                else if (jade < 0)
                {
                    JadePercentage.Foreground = new SolidColorBrush(Color.FromRgb(0xCD, 0x04, 0x04));
                }
                else
                {
                    JadePercentage.Foreground = new SolidColorBrush(Color.FromRgb(00, 0xA8, 0xFF));
                }
            }
        );

        if ((from time in IgnoredLeadingTiming select time).Any(it => curr <= it.begin + this._timezone && curr <= it.end + this._timezone))
        {
            return;
        }

        var vals = new List<double> { diamond, ruby, jade };
        switch (vals.IndexOf(vals.Max()))
        {
            case 0:
                Switch("Diamond", diamond);
                break;
            case 1:
                Switch("Ruby", ruby);
                break;
            case 2:
                Switch("Jade", jade);
                break;
                
        }
    }

    private void Switch(string name, double value)
    {
        if (this.FullscreenCheckbox.IsChecked == true)
        {
            new Alarm(name).Show();
        }
        else
        {
            NotificationManager.PushNotification("更换 Cyclius 槽位", $"已经到了最佳更换 Cyclius 槽位的时候了！现在最大值的是: {name} 槽，加成值为 {value * 100:+##.00;-##.00;0.00}%");
        }
    }

    private float CalculateCurrentDiamond(int timezone)
    {
        return (float) Math.Round(MaximumIncrement * Math.Sin(DiamondOscillationFactor * (GetCurrentTimeInHours() - timezone)), 4);
    }
    
    private float CalculateCurrentRuby(int timezone)
    {
        return (float) Math.Round(MaximumIncrement * Math.Sin(RubyOscillationFactor * (GetCurrentTimeInHours() - timezone)), 4);
    }
    
    private float CalculateCurrentJade(int timezone)
    {
        return (float) Math.Round(MaximumIncrement * Math.Sin(JadeOscillationFactor * (GetCurrentTimeInHours() - timezone)), 4);
    }

    private float GetCurrentTimeInHours()
    {
        return (float) Math.Round(DateTime.Now.Hour + (double) DateTime.Now.Minute / 60 + (double) DateTime.Now.Second / 3600, 2);
    }

}
