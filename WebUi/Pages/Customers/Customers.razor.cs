using BlazorBootstrap;
using Commons.Data.Results;
using Commons.Models;
using Microsoft.AspNetCore.Components;
using WebUi.Extensions;
using WebUi.Services;
using WebUi.UiParts.App;
using WebUi.UiParts.Base;

namespace WebUi.Pages.Customers;

public partial class Customers : ListPageComponentBase<Customer>
{
    #region INJECTS
    [Inject] protected ICustomerService CustomerService { get; init; } = default!;
    #endregion INJECTS

    #region PROPS
    private AppGrid<Customer> grid { get; set; } = default!;
    #endregion PROPS

    #region OVERRIDES
    protected override AppGrid<Customer> Grid() => grid;
    protected override string[] AddButtonPermissions() => [Permission.CREATE_CUSTOMER];
    protected override string TypeName() => "Cliente";
    protected override string TypeNamePlural() => "Clientes";

    protected override async ValueTask<Result<ODataCountValue<Customer>>> QueryItemsAsync(
        IODataService service, IEnumerable<FilterItem> filters, string queryStr)
    {
        return await service.QueryCustomerAsync(
            query => { },
            queryStr
        );
    }

    protected override async Task<Result> DeleteItemAsync(Guid id)
    {
        return await CustomerService.DeleteAsync(id);
    }
    #endregion OVERRIDES
}
