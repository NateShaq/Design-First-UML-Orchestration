using Api.Domain;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/voting")]
public class VotingController : ControllerBase
{
    private readonly IGovernmentDataStore _store;

    public VotingController(IGovernmentDataStore store)
    {
        _store = store;
    }

    [HttpGet("voters")]
    public ActionResult<IEnumerable<VoterRecord>> GetVoters() => Ok(_store.Voters);

    [HttpPost("voters")]
    public ActionResult<VoterRecord> RegisterVoter([FromBody] VoterRecord voter)
    {
        voter.Id = Guid.NewGuid();
        _store.Voters.Add(voter);
        _store.AuditTrails.Add(new AuditTrail { ActorId = voter.CitizenId, Action = "VoterRegistered" });
        return CreatedAtAction(nameof(GetVoters), new { id = voter.Id }, voter);
    }

    [HttpGet("elections")]
    public ActionResult<IEnumerable<Election>> GetElections() => Ok(_store.Elections);

    [HttpPost("ballot-requests")]
    public ActionResult<BallotRequest> RequestBallot([FromBody] BallotRequest request)
    {
        request.Id = Guid.NewGuid();
        _store.BallotRequests.Add(request);
        _store.AuditTrails.Add(new AuditTrail { ActorId = request.VoterRecordId, Action = "BallotRequested" });
        return CreatedAtAction(nameof(GetBallotRequests), new { id = request.Id }, request);
    }

    [HttpGet("ballot-requests")]
    public ActionResult<IEnumerable<BallotRequest>> GetBallotRequests() => Ok(_store.BallotRequests);
}
