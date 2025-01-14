﻿using Blog.Bases.Services;
using Blog.Entities.Comments;
using Blog.Mvc.Models;
using Blog.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.Mvc.Controllers
{
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Comment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CommentViewModel commentViewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                commentViewModel.CommentAuthorId = Guid.Parse(userId);

                var result = _commentService.CreateCommentAsync(new ServiceInput<CommentInput>()
                {
                    Input = new CommentInput()
                    {
                        PostId = commentViewModel.PostId,
                        CommentAuthorId= commentViewModel.CommentAuthorId,
                        Message = commentViewModel.Message
                    }
                }).Result;

                if (!result.Success && result.Errors.Any())
                {
                    // Armazena os erros na ViewBag
                    ViewBag.ErrorMessages = result.Errors.Select(x => x.Message).ToList();
                    //return RedirectToAction("Details", "Post", commentViewModel);
                }
            }

            return RedirectToAction("Details", "Post", new { id = commentViewModel.PostId });
        }

        public IActionResult Delete(Guid id)
        {
            var comment = _commentService.GetCommentAsync(id).Result;
            if (comment == null || comment.Output == null)
            {
                return NotFound();
            }

            var result = _commentService.RemoveCommentAsync(id).Result;

            return RedirectToAction("Details", "Post", new { id = comment.Output.PostId });
        }
    }
}
