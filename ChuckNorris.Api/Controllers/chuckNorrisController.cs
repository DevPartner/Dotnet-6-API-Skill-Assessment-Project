using ChuckNorris.Core.Models;
using ChuckNorris.Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChuckNorris.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ChuckNorrisController : ControllerBase
{
    public readonly IJokeHandler _jokeHandler;

    public ChuckNorrisController(IJokeHandler jokeHandler)
    {
        _jokeHandler = jokeHandler;
    }

    [HttpGet(Name = "get-facts")]
    public async Task<IActionResult> GetFactsAsync(int count, CancellationToken cancellationToken = default)
    {
        if (count < 0)
            return BadRequest("Count should be greater than or equals to zero.");

        var transformedJokeList = await _jokeHandler.GetTransformedJokesAsync(count, cancellationToken);

        if (!transformedJokeList.Any())
            return NotFound("No jokes found.");

        return Ok(transformedJokeList);
    }
}
