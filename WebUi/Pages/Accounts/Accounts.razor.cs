using BlazorBootstrap;
using Commons.Data.Results;
using Commons.Models;
using Microsoft.AspNetCore.Components;
using WebUi.Extensions;
using WebUi.Services;
using WebUi.UiParts.App;
using WebUi.UiParts.Base;

namespace WebUi.Pages.Accounts;

public partial class Accounts : ListPageComponentBase<Account>
{
    #region INJECTS
    [Inject] protected IAccountService AccountService { get; init; } = default!;
    #endregion INJECTS

    #region PROPS
    private AppGrid<Account> grid { get; set; } = default!;
    #endregion PROPS

    #region OVERRIDES
    protected override AppGrid<Account> Grid() => grid;
    protected override string[] AddButtonPermissions() => [Permission.CREATE_ACCOUNT];
    protected override string TypeName() => "Usuário";
    protected override string TypeNamePlural() => "Usuários";

    protected override async ValueTask<Result<ODataCountValue<Account>>> QueryItemsAsync(
       IODataService service, IEnumerable<FilterItem> filters, string queryStr)
    {
        return await service.QueryAccountAsync(
            query => { 
                query.Expand(x => x.Profile); 
            },
            queryStr
        );
    }

    protected override async Task<Result> DeleteItemAsync(Guid id)
    {
        return await AccountService.DeleteAsync(id);
    }
    #endregion OVERRIDES
}
