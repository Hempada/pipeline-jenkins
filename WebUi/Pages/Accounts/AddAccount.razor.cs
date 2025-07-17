using Commons.Data;
using Commons.Data.Results;
using Commons.Models;
using Microsoft.AspNetCore.Components;
using System.Globalization;
using WebUi.Extensions;
using WebUi.Services;
using WebUi.UiParts.Base;

namespace WebUi.Pages.Accounts;

public partial class AddAccount : AddPageComponentBase<Account>
{
    #region INJECTS
    [Inject] protected IAccountService AccountService { get; init; } = default!;
    #endregion INJECTS

    #region PROPS
    private IEnumerable<Profile> Profiles { get; set; } = Enumerable.Empty<Profile>();
    private OnChangeValue<string> Name { get; set; } = default!;
    private OnChangeValue<string> Username { get; set; } = default!;
    private OnChangeValue<string> Email { get; set; } = default!;
    private OnChangeValue<Profile> Profile { get; set; } = default!;
    #endregion PROPS

    #region OVERRIDES
    protected override string TypeName() => "Usuário";

    protected override async ValueTask<Result<ODataCountValue<Account>>> QueryItemAsync(
       IODataService service, Guid id)
    {
        return await service.QueryAccountAsync(
            query =>
            {
                query.Expand(x => x.Profile);
                query.Filter(x => x.Id == id);
            }
        );
    }

    protected override async Task<Result> SaveItemAsync()
    {
        return await AccountService.SaveAsync(ItemId, Name.Value, Username.Value, Email.Value, Profile.Value.Id);
    }
    #endregion OVERRIDES

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        var result = await ODataService.QueryProfileAsync(query => { });
        if (result.Valid)
        {
            Profiles = result.Data?.Value ?? Enumerable.Empty<Profile>();
        }

        if (!Edit)
        {
            Name = new OnChangeValue<string>(OnDataChanged);
            Username = new OnChangeValue<string>(OnDataChanged);
            Email = new OnChangeValue<string>(OnDataChanged);
            Profile = new OnChangeValue<Profile>(OnDataChanged);
        }

        StateHasChanged();
    }

    protected override void OnLoadedItem(Account account)
    {
        Name = new OnChangeValue<string>(account.Name, OnDataChanged);
        Username = new OnChangeValue<string>(account.Username, OnDataChanged);
        Email = new OnChangeValue<string>(account.Email, OnDataChanged);
        Profile = new OnChangeValue<Profile>(account.Profile ?? default!, OnDataChanged);
    }

    private Func<Profile, string> ProfileToString = x => x.Name;
}
