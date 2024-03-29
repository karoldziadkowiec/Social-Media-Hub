﻿using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using SocialMediaHub.Database;
using SocialMediaHub.Models;

namespace SocialMediaHub.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IQueryable<Comment>> GetAllComments()
            => await Task.FromResult(_context.Comments.OrderBy(c => c.Id));

        public async Task<Comment> GetComment(int commentId)
            => await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

        public async Task<byte[]> GetCommentsCsvBytes()
        {
            var comments = await _context.Comments.ToListAsync();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Posts");

                worksheet.Cell(1, 1).Value = "Id";
                worksheet.Cell(1, 2).Value = "Content";
                worksheet.Cell(1, 3).Value = "CreationDate";
                worksheet.Cell(1, 4).Value = "UserId";
                worksheet.Cell(1, 5).Value = "PostId";

                var row = 2;
                foreach (var comment in comments)
                {
                    worksheet.Cell(row, 1).Value = comment.Id;
                    worksheet.Cell(row, 2).Value = comment.Content;
                    worksheet.Cell(row, 3).Value = comment.CreationDate;
                    worksheet.Cell(row, 4).Value = comment.UserId;
                    worksheet.Cell(row, 5).Value = comment.PostId;
                    row++;
                }

                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    memoryStream.Position = 0;
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
