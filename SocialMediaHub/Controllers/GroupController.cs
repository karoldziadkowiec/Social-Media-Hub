using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaHub.Models;
using SocialMediaHub.Repositories;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SocialMediaHub.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupRepository _groupRepository;

        public GroupController(IGroupRepository groupRepository, UserRepository userRepository)
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
        public async Task<IActionResult> AddGroupAsync([FromBody] Models.Group group)
        {
            if (group == null)
                return BadRequest();

            await _groupRepository.AddGroup(group);

            return CreatedAtAction(nameof(GetGroupAsync), new { groupId = group.Id }, group);
        }

        // PUT: /api/groups/:id
        [HttpPut]
        public async Task<IActionResult> UpdateGroupAsync(int groupId, [FromBody] Models.Group group)
        {
            try
            {
                if (group == null)
                    return BadRequest("Invalid group data");

                var existingGroup = await _groupRepository.GetGroup(groupId);

                if (existingGroup == null)
                    return NotFound();

                existingGroup.Name = group.Name;
                existingGroup.Limit = group.Limit;

                await _groupRepository.UpdateGroup(group);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: /api/groups/:id
        [HttpDelete("{groupId}")]
        public async Task<IActionResult> RemoveGroupAsync(int groupId)
        {
            try
            {
                var user = await _groupRepository.GetGroup(groupId);

                if (user == null)
                    return NotFound();

                await _groupRepository.RemoveGroup(groupId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: /api/groups/csv
        [HttpGet("csv")]
        public async Task<IActionResult> GetGroupsInCsvFormatAsync()
        {
            try
            {
                var usersInCsv = await _groupRepository.GetGroupsInCsvFormat();

                using (var memoryStream = new MemoryStream())
                using (var writer = new StreamWriter(memoryStream, Encoding.UTF8))
                using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
                {
                    csv.WriteRecords(usersInCsv);

                    writer.Flush();
                    memoryStream.Position = 0;

                    return File(memoryStream.ToArray(), "text/csv", "groups.csv");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: /api/groups/:id/users
        [HttpGet("{groupId}/users")]
        public async Task<IActionResult> GetUsersByGroupIdAsync(int groupId)
        {
            var groups = await _groupRepository.GetUsersByGroupId(groupId);
            return Ok(groups);
        }

        // GET: /api/groups/:id/fill
        [HttpGet("{groupId}/fill")]
        public async Task<IActionResult> GetGroupFillPercentageAsync(int groupId)
        {
            var fillPercentage = await _groupRepository.GetGroupFillPercentage(groupId);

            if (double.IsNaN(fillPercentage))
            {
                return BadRequest("Invalid calculation.");
            }

            return Ok(fillPercentage);
        }

        // GET: /api/groups/byname
        [HttpGet("byname")]
        public async Task<IActionResult> GetGroupsByNameAsync()
        {
            var groups = await _groupRepository.GetGroupsByName();
            return Ok(groups);
        }

        // GET: /api/groups/empty
        [HttpGet("empty")]
        public async Task<IActionResult> GetEmptyGroupAsync()
        {
            var groups = await _groupRepository.GetEmptyGroup();
            return Ok(groups);
        }

        // GET: /api/groups/search
        [HttpGet("search")]
        public async Task<IActionResult> SearchGroupsAsync(string searchTerm)
        {
            var groups = await _groupRepository.SearchGroups(searchTerm);
            return Ok(groups);
        }

        // GET: /api/groups/partial
        [HttpGet("partial")]
        public async Task<IActionResult> SearchPartialGroupsAsync(string searchTerm)
        {
            var groups = await _groupRepository.SearchPartialGroups(searchTerm);
            return Ok(groups);
        }
    }
}
