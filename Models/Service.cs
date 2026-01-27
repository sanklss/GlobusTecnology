using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace GlobusT.Models;

public partial class Service
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int IdDevelopmentArea { get; set; }

    public int Duration { get; set; }

    public DateTime Date { get; set; }
    public FontWeight TitleFontWeight
    {
        get
        {
            DateOnly date = DateOnly.FromDateTime(Date);
            var today = new DateOnly(2026, 6, 9);
            var leftDays = date.DayNumber - today.DayNumber;
            return leftDays is > 0 and < 7 ? FontWeights.Bold : FontWeights.Normal;
        }
    }

    public int Price { get; set; }

    public int IdTypeCommand { get; set; }

    public int FreeSlot { get; set; }
    public Brush BackGroundColor
    {
        get
        {
            if (FreeSlot < IdTypeCommandNavigation.Capacity * 0.1) return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFB6C1"));
            return new SolidColorBrush(Colors.White);
        }
    }

    public string Image { get; set; } = null!;
    public string ImageFullPath => !string.IsNullOrEmpty(Image) ? $"Images/{Image}" : "Images/icon.png";
    public virtual DevelopmentArea IdDevelopmentAreaNavigation { get; set; } = null!;

    public virtual TypeCommand IdTypeCommandNavigation { get; set; } = null!;

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
