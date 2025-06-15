using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Lastik.Models.Order;
using MainComponents.Components;

namespace Lastik.ViewModels.Controls;


public partial class UserFormViewModel:ObservableValidator
{
    [ObservableProperty]
    [RegularExpression(@"\w+@\w+\.\w+")]
    [Required]
    [MinLength(7)]
    [NotifyDataErrorInfo]
    private string _email = string.Empty;

    [ObservableProperty]
    [Required]
    [NotifyDataErrorInfo]
    [RegularExpression(@"[А-Яа-я]+")]
    private string _name = string.Empty;

    [ObservableProperty]
    [Required]
    [NotifyDataErrorInfo]
    [RegularExpression(@"[А-Яа-я]+")]
    private string _surname = string.Empty;

    public Contacts Submit() => new(Email, Surname, Name);

    public bool Validate()
    {
        ValidateAllProperties();
        return !HasErrors;
    }
}