using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DiplomApi.Models;
using static DiplomApi.Models.DiplombdContext;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace DiplomApi.Controllers
{
    [Route("api/reviews")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly DiplombdContext _context;

        public ReviewsController(DiplombdContext context)
        {
            _context = context;
        }
        [HttpPost("addReview")]
        public async Task<IActionResult> CreateReview([FromBody] Review review)
        {
            if (review == null)
            {
                return BadRequest(new
                {
                    code = 400,
                    message = "Review is null."
                });
            }

            try
            {
                // Находим заказ по OrderId
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == review.OrderId);

                // Если заказ не найден, возвращаем ошибку
                if (order == null)
                {
                    return NotFound(new
                    {
                        code = 404,
                        message = $"Order with ID {review.OrderId} not found."
                    });
                }

                // Проверяем состояние заказа
                if (order.OrderState != "Done")
                {
                    return BadRequest(new
                    {
                        code = 400,
                        message = "Cannot add review. Order state must be 'Done'."
                    });
                }

                // Связываем заказ с отзывом
                review.Order = order;

                // Добавляем отзыв в контекст
                _context.Reviews.Add(review);

                // Обновляем состояние заказа на "Reviewed"
                order.OrderState = "Reviewed";

                // Сохраняем изменения в базе данных
                await _context.SaveChangesAsync();

                // Возвращаем успешный ответ
                return Ok(new
                {
                    code = 200,
                    message = "Review added successfully and order state updated to 'Reviewed'.",
                    review
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    message = $"Internal server error: {ex.Message}"
                });
            }
        }


        [HttpGet("getReviews")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            if (_context.Reviews == null)
            {
                return NotFound();
            }

            var reviews = await _context.Reviews.ToListAsync();
            return Ok(reviews);
        }
        [HttpGet("getReviewsApproved")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviewsApproved()
        {
            if (_context.Reviews == null)
            {
                return NotFound(new
                {
                    code = 404,
                    message = "Reviews not found in the database."
                });
            }

            try
            {
                var approvedReviews = await _context.Reviews
                    .Where(r => r.Approved == true)
                    .ToListAsync();

                if (!approvedReviews.Any())
                {
                    return NotFound(new
                    {
                        code = 404,
                        message = "No approved reviews found."
                    });
                }

                return Ok(approvedReviews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    message = $"Internal server error: {ex.Message}"
                });
            }
        }
        [HttpPatch("approveReview/{id}")]
        public async Task<IActionResult> ApproveReview(Guid id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                {
                    return NotFound(new
                    {
                        code = 404,
                        message = "Review not found."
                    });
                }

                if (review.Approved)
                {
                    return BadRequest(new
                    {
                        code = 400,
                        message = "Review is already approved."
                    });
                }

                review.Approved = true;
                _context.Entry(review).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    code = 200,
                    message = "Review approved successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    message = $"Internal server error: {ex.Message}"
                });
            }
        }
        [HttpDelete("deleteReview/{id}")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            try
            {
                var review = await _context.Reviews.FindAsync(id);
                if (review == null)
                {
                    return NotFound(new
                    {
                        code = 404,
                        message = "Review not found."
                    });
                }

                _context.Reviews.Remove(review);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    code = 200,
                    message = "Review deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    code = 500,
                    message = $"Internal server error: {ex.Message}"
                });
            }
        }





    }

}
