using Microsoft.AspNetCore.Mvc;
using PhotographerPlatform.Workspace.Api.Models;
using PhotographerPlatform.Workspace.Api.Services;
using PhotographerPlatform.Workspace.Core.Services;

namespace PhotographerPlatform.Workspace.Api.Controllers;

[ApiController]
[Route("api/search")]
public sealed class SearchController : ControllerBase
{
    private readonly IWorkspaceService _workspace;

    public SearchController(IWorkspaceService workspace)
    {
        _workspace = workspace;
    }

    [HttpGet]
    public async Task<ActionResult<SearchResponse>> Search([FromQuery] string q, CancellationToken ct)
    {
        if (!RequestContext.TryGetAccountId(Request, out var accountId))
        {
            return BadRequest("AccountId is required.");
        }

        if (string.IsNullOrWhiteSpace(q))
        {
            return BadRequest("Query is required.");
        }

        var result = await _workspace.SearchAsync(accountId, q, ct).ConfigureAwait(false);
        return Ok(new SearchResponse { Projects = result.Projects, Clients = result.Clients });
    }
}
