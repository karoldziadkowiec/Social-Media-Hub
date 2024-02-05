using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaHub.Models;
using SocialMediaHub.Repositories;

namespace SocialMediaHub.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;

        public GroupController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        // GET: /api/groups
        [HttpGet]
        public async Task<IActionResult> GetAllGroupsAsync()
        {
            var groups = await _groupRepository.GetAllGroups();
            return Ok(groups);
        }

        // GET: /api/groups/:id 
        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetGroupAsync(int groupId)
        {
            var group = await _groupRepository.GetGroup(groupId);

            if (group == null)
                return NotFound();

            return Ok(group);
        }

        // POST: /api/groups
        [HttpPost]
        public async Task<IActionResult> AddGroupAsync([FromBody] Group group)
        {
            if (group == null)
                return BadRequest();

            await _groupRepository.AddGroup(group);

            return CreatedAtAction(nameof(GetGroupAsync), new { groupId = group.Id }, group);
        }
    }
}
