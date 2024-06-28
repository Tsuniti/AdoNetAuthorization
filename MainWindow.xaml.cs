using AuthorizationFormWPF.Database;
using AuthorizationFormWPF.Entities;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Controls;

namespace AuthorizationFormWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    UsersDB db = new UsersDB();
    public bool IsDarkTheme { get; set; }
    private readonly PaletteHelper _paletteHelper;

    public MainWindow()
    {
        _paletteHelper = new PaletteHelper();
        InitializeComponent();

        SubmitButton.Click += LogIn;
        ChangeFormButton.Click += SignUpPaint;
    }

    private void ToggleTheme(object sender, RoutedEventArgs e)
    {
        ITheme theme = _paletteHelper.GetTheme();

        if (theme.GetBaseTheme() == BaseTheme.Dark)
        {
            IsDarkTheme = false;
            theme.SetBaseTheme(Theme.Light);
        }
        else
        {
            IsDarkTheme = true;
            theme.SetBaseTheme(Theme.Dark);
        }
        _paletteHelper.SetTheme(theme);
    }

    private void CloseApplication(object sender, RoutedEventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }

    protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
    {
        base.OnMouseLeftButtonDown(e);
        DragMove();
    }

    private void LogIn(object sender, RoutedEventArgs e)
    {
        ErrorLabel.SetResourceReference(Control.ForegroundProperty, "MaterialDesignValidationErrorBrush");
        bool IsFieldsEmpty = false;
        if (UsernameTextBox.Text == string.Empty)
        {
            UsernameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");

            IsFieldsEmpty = true;
        }
        if (PasswordTextBox.Password == string.Empty)
        {
            PasswordTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            IsFieldsEmpty = true;

        }
        if (IsFieldsEmpty)
        {
            ErrorLabel.Content = "Fields can't be empty";
            return;
        }


        var user = db.GetUserByUsername(UsernameTextBox.Text);


        if (user == null ||
            user.Password != Encryptor.ComputeSha256Hash(PasswordTextBox.Password))
        {
            ErrorLabel.Content = "Username or password does not match";
            UsernameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            PasswordTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            return;
        }
        
            ErrorLabel.Content = "Success login";
            ErrorLabel.Foreground = Brushes.Lime;
            UsernameTextBox.BorderBrush = Brushes.Lime;
            PasswordTextBox.BorderBrush = Brushes.Lime;
        ShowAllUsers();


    }


    private void SignUp(object sender, RoutedEventArgs e)
    {
        ErrorLabel.SetResourceReference(Control.ForegroundProperty, "MaterialDesignValidationErrorBrush");

        bool IsFieldsEmpty = false;
        if (UsernameTextBox.Text == string.Empty)
        {
            UsernameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            IsFieldsEmpty = true;
        }
        if (PasswordTextBox.Password == string.Empty)
        {
            PasswordTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            IsFieldsEmpty = true;

        }
        if (PasswordRepeatTextBox.Password == string.Empty)
        {
            PasswordRepeatTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            IsFieldsEmpty = true;

        }
        if (RealNameTextBox.Text == string.Empty)
        {
            RealNameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            IsFieldsEmpty = true;

        }
        if (AgeTextBox.Text == string.Empty)
        {
            AgeTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            IsFieldsEmpty = true;

        }
        if (IsFieldsEmpty)
        {
            ErrorLabel.Content = "Fields can't be empty";
            return;
        }
        if (UsernameTextBox.Text.Length <= 3)
        {
            ErrorLabel.Content = "Username must be longer than 2 characters";
            return;
        }

        if (db.GetUserByUsername(UsernameTextBox.Text) != null)
        {

            ErrorLabel.Content = "This username already taken";
            UsernameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            return;

        }
        if (PasswordTextBox.Password.Length <= 4)
        {
            ErrorLabel.Content = "Password must be longer than 3 characters";
            PasswordTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");

            return;
        }
        if (PasswordTextBox.Password != PasswordRepeatTextBox.Password)
        {
            ErrorLabel.Content = "Password mismatch";
            PasswordTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            PasswordRepeatTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignValidationErrorBrush");
            return;
        }

        db.Add(
            new User { Username = UsernameTextBox.Text,
                       Password = Encryptor.ComputeSha256Hash(PasswordTextBox.Password),
                       RegisteredAt = DateTime.Now,
                       RealName = RealNameTextBox.Text,
                       Email = EmailTextBox.Text,
                       Age = uint.Parse(AgeTextBox.Text)});
        ErrorLabel.Content = "Success signup";
        ErrorLabel.Foreground = Brushes.Lime;
        UsernameTextBox.BorderBrush = Brushes.Lime;
        PasswordTextBox.BorderBrush = Brushes.Lime;
        PasswordRepeatTextBox.BorderBrush = Brushes.Lime;
        RealNameTextBox.BorderBrush = Brushes.Lime;
        EmailTextBox.BorderBrush = Brushes.Lime;
        AgeTextBox.BorderBrush = Brushes.Lime;

        ShowAllUsers();



    }

    private void LogInPaint(object sender, RoutedEventArgs e)
    {
        HidePasswordRepeatTextBox();

        UsernameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        PasswordTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        PasswordRepeatTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        RealNameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        EmailTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        AgeTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        ErrorLabel.Content = String.Empty;

        WelcomeLabel.Content = "Welcome back!";
        SuggestionLabel.Content = "Log in to your existing account";

        SubmitButton.Click -= SignUp;
        SubmitButton.Click += LogIn;
        SubmitButton.Content = "LOG IN";

        ChangeFormButton.Click -= LogInPaint;
        ChangeFormButton.Click += SignUpPaint;
        ChangeFormButton.Content = "Create account";

    }


    private void SignUpPaint(object sender, RoutedEventArgs e)
    {
        UsernameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        PasswordTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        PasswordRepeatTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        RealNameTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        EmailTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        AgeTextBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");

        ErrorLabel.Content = String.Empty;
        WelcomeLabel.Content = "Welcome!";
        SuggestionLabel.Content = "Create a new account";

        ShowPasswordRepeatTextBox();
        SubmitButton.Click -= LogIn;
        SubmitButton.Click += SignUp;
        SubmitButton.Content = "SIGN UP";


        ChangeFormButton.Click -= SignUpPaint;
        ChangeFormButton.Click += LogInPaint;
        ChangeFormButton.Content = "Log in";


    }

    private async void ShowPasswordRepeatTextBox()
    {
        var opacityAnimation = new DoubleAnimation
        {
            From = 0,
            To = 1,
            Duration = TimeSpan.FromSeconds(0.3)
        };
        var marginTopAnimation = new ThicknessAnimation
        {
            From = new Thickness(0, -58, 0, 0),
            To = new Thickness(10, 10, 10, 10),
            Duration = TimeSpan.FromSeconds(0.3)
        };
        var marginLeftAnimation = new ThicknessAnimation
        {
            From = new Thickness(-310, 10, 10, 10),
            To = new Thickness(10, 10, 10, 10),
            Duration = TimeSpan.FromSeconds(0.3)
        };
        var marginTopLeftAnimation = new ThicknessAnimation
        {
            From = new Thickness(-310, -58, 10, 10),
            To = new Thickness(10, 10, 10, 10),
            Duration = TimeSpan.FromSeconds(0.3)
        };

        RealNameTextBox.BeginAnimation(System.Windows.Controls.Control.MarginProperty, marginLeftAnimation);
        RealNameTextBox.BeginAnimation(System.Windows.Controls.Control.OpacityProperty, opacityAnimation);

        AgeTextBox.BeginAnimation(System.Windows.Controls.Control.MarginProperty, marginLeftAnimation);
        AgeTextBox.BeginAnimation(System.Windows.Controls.Control.OpacityProperty, opacityAnimation);

        EmailTextBox.BeginAnimation(System.Windows.Controls.Control.MarginProperty, marginTopLeftAnimation);
        EmailTextBox.BeginAnimation(System.Windows.Controls.Control.OpacityProperty, opacityAnimation);

        PasswordRepeatTextBox.BeginAnimation(System.Windows.Controls.Control.MarginProperty, marginTopAnimation);
        PasswordRepeatTextBox.BeginAnimation(System.Windows.Controls.Control.OpacityProperty, opacityAnimation);


        await Task.Delay(TimeSpan.FromSeconds(0.3));
        PasswordRepeatTextBox.IsEnabled = true;

    }
    private async void HidePasswordRepeatTextBox()
    {
        PasswordRepeatTextBox.IsEnabled = false;

        DoubleAnimation opacityAnimation = new DoubleAnimation
        {
            From = 1,
            To = 0,
            Duration = TimeSpan.FromSeconds(0.3)
        };
        var marginTopAnimation = new ThicknessAnimation
        {
            From = new Thickness(10, 10, 10, 10),
            To = new Thickness(0, -58, 0, 0),
            Duration = TimeSpan.FromSeconds(0.3)
        };
        var marginLeftAnimation = new ThicknessAnimation
        {
            From = new Thickness(10, 10, 10, 10),
            To = new Thickness(-310, 10, 10, 10),
            Duration = TimeSpan.FromSeconds(0.3)
        };
        var marginTopLeftAnimation = new ThicknessAnimation
        {
            From = new Thickness(10, 10, 10, 10),
            To = new Thickness(-310, -58, 10, 10),
            Duration = TimeSpan.FromSeconds(0.3)
        };


        RealNameTextBox.BeginAnimation(System.Windows.Controls.Control.MarginProperty, marginLeftAnimation);
        RealNameTextBox.BeginAnimation(System.Windows.Controls.Control.OpacityProperty, opacityAnimation);

        AgeTextBox.BeginAnimation(System.Windows.Controls.Control.MarginProperty, marginLeftAnimation);
        AgeTextBox.BeginAnimation(System.Windows.Controls.Control.OpacityProperty, opacityAnimation);

        EmailTextBox.BeginAnimation(System.Windows.Controls.Control.MarginProperty, marginTopLeftAnimation);
        EmailTextBox.BeginAnimation(System.Windows.Controls.Control.OpacityProperty, opacityAnimation);

        PasswordRepeatTextBox.BeginAnimation(System.Windows.Controls.Control.MarginProperty, marginTopAnimation);
        PasswordRepeatTextBox.BeginAnimation(System.Windows.Controls.Control.OpacityProperty, opacityAnimation);

        await Task.Delay(TimeSpan.FromSeconds(0.3));

        PasswordRepeatTextBox.Password = String.Empty;
        RealNameTextBox.Text = String.Empty;
        AgeTextBox.Text = String.Empty;
        EmailTextBox.Text = String.Empty;


    }

    private void ChangeTextBoxesBorderColorToDefault()
    {
        if (UsernameTextBox.BorderBrush == (Brush)App.Current.FindResource("MaterialDesignDivider") &&
            PasswordTextBox.BorderBrush == (Brush)App.Current.FindResource("MaterialDesignDivider") &&
            PasswordRepeatTextBox.BorderBrush == (Brush)App.Current.FindResource("MaterialDesignDivider") &&
            RealNameTextBox.BorderBrush == (Brush)App.Current.FindResource("MaterialDesignDivider") &&
            AgeTextBox.BorderBrush == (Brush)App.Current.FindResource("MaterialDesignDivider"))
        {
            ErrorLabel.Content = String.Empty;
        }
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
            TextBox textBox = sender as TextBox;
            if (textBox.BorderBrush == (Brush)App.Current.FindResource("MaterialDesignValidationErrorBrush"))
            {
                textBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
            }


        ChangeTextBoxesBorderColorToDefault();
    }

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        PasswordBox passwordBox = sender as PasswordBox;
        if (passwordBox.BorderBrush == (Brush)App.Current.FindResource("MaterialDesignValidationErrorBrush"))
        {
            passwordBox.SetResourceReference(Control.BorderBrushProperty, "MaterialDesignDivider");
        }
        ChangeTextBoxesBorderColorToDefault();
    }
    private void ShowAllUsers()
    {
        string message = String.Empty;
        foreach (var item in db)
        {
            message += item.ToString() + Environment.NewLine + Environment.NewLine;
        }

        new MyMessageBox(message).ShowDialog();

       
    }
}