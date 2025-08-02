using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using Shared.Utils;

public class MudPasswordField: MudTextField<string> {
    private bool isSecure = true;
    
    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        OnAdornmentClick = OnAdornmentClickCallback;
        
        SetTextFieldStyle();
    }

    private EventCallback<MouseEventArgs> OnAdornmentClickCallback =>
        new EventCallbackFactory().Create<MouseEventArgs>(this, ChangeTextFieldVisibility);

    private void ChangeTextFieldVisibility()
    {
        isSecure.Toggle();
        SetTextFieldStyle();
    }

    private void SetTextFieldStyle()
    {
        InputType = isSecure ? InputType.Password : InputType.Text;
        AdornmentIcon = isSecure ? Icons.Material.Filled.Visibility : Icons.Material.Filled.VisibilityOff;
    }
}