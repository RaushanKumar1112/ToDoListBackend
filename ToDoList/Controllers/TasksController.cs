using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using ToDoList.Models;
using ToDoList.Providers;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [SwaggerTag("Tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        /// <summary>
        /// Fetches all tasks from the database.
        /// </summary>
        /// <returns>List of tasks</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tasks>>> GetTasks()
        {
            var tasks = await _context.Tasks.ToListAsync<Tasks>();
            return Ok(tasks);
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tasks>> GetTaskById(int id)
        {
            var task = await _context.Tasks.FirstOrDefaultAsync(task => task.Id == id);
            if(task == null)
            {
                return NotFound("Task not found");
            }
            return Ok(task);
        }

        // POST: api/Tasks
        [HttpPost]
        public async Task<ActionResult> InsertTask(Tasks newTask)
        {
            _context.Tasks.Add(newTask);
            await _context.SaveChangesAsync();
            return Ok("Task added successfully");
        }

        // PUT: api/Tasks/5
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateTask(int id, Tasks updatedTask)
        {
            var task = _context.Tasks.FirstOrDefault(task => task.Id == id);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            task.Name = task.Name == updatedTask.Name ? task.Name : updatedTask.Name;
            task.IsCompleted = task.IsCompleted == updatedTask.IsCompleted ? task.IsCompleted : updatedTask.IsCompleted;
            task.UpdatedAt = DateTime.Now;
            if(task.IsCompleted == true)
            {
                task.CompletedAt = DateTime.Now;
            }

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Task updated successfully", Task = task });
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTaskById(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.Id == id);

            if(task == null)
            {
                return NotFound("Task not found");
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Task deleted successfully", Task = task });
        }

        // DELETE: api/Tasks/
        [HttpDelete]
        public async Task<ActionResult> DeleteAllTasks()
        {
            if (_context.Tasks == null)
            {
                return NotFound("Task not found");
            }

            _context.Tasks.RemoveRange(_context.Tasks);
            await _context.SaveChangesAsync();
            return Ok("Task deleted successfully");
        }
    }
}
