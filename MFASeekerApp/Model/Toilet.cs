﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MFASeeker.Services;
using System.Collections.ObjectModel;

namespace MFASeeker.Model
{
    //public partial class Toilet
    //{
    //    public int Id { get; set; }
    //    public string Guid { get; set; }
    //    public string? Name { get; set; }
    //    public Location? Location { get; set; }
    //    public string? Description { get; set; }
    //    public double Rating { get; set; }
    //    public string? UserName { get; set; }
    //    public string? UserId { get; set; }
    //    public DateTime CreatedDate { get; set; }
    //    public ObservableCollection<ImageFile> Images { get; set; } // удалить после разработки photos

    //    public Toilet()
    //    {
    //        Id = 0;
    //        this.Guid = CreateGuid();
    //        Name = "";
    //        Location = null;
    //        Rating = 0;
    //        Description = "";
    //        CreatedDate = DateTime.Now;
    //        UserName = DeviceInfo.Current.Name;
    //        Images = []; // удалить после разработки photos
    //    }

    //    public string GetInfo()
    //    {
    //        return $"Name: {Name}, Location: {Location}, Description: {Description}, Rating: {Rating}, Created By: {UserId}, On: {CreatedDate}";
    //    }
    //    public bool IsValid()
    //    {
    //        return !string.IsNullOrWhiteSpace(Name);
    //    }
    //    private string CreateGuid()
    //    {
    //        return System.Guid.NewGuid().ToString().GetHashCode().ToString("x");
    //    }

    //}
}
