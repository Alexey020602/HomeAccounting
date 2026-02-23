using MudBlazor;

namespace BlazorConsolidated.Users.Registration;

internal static class BoolForRegistrationForExtensions
{
    extension(bool isShow)
    {
        public InputType PasswordFieldType() => isShow 
            ? InputType.Text 
            : InputType.Password;

        public string PasswordFieldIcon() => isShow 
            ? Icons.Material.Filled.VisibilityOff 
            :  Icons.Material.Filled.Visibility;
    }
}