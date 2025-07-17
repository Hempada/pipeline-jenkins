using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class ApiKeyAuthorizeAttribute : AuthorizeAttribute, IAllowApiKeyAuthenticationFilter
{ }

internal interface IAllowApiKeyAuthenticationFilter : IFilterMetadata;