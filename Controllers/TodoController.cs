using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoAPI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TodoAPI.Controllers
{
    [Route("api/todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        //Constructor
        public TodoController(TodoContext context)
        {
            _context = context;

            if(_context.TodoItems.Count() == 0) 
            {
                //Create a new collection if to-doList is empty
                _context.TodoItems.Add(new TodoItem { Name = "Item"});
                _context.SaveChanges();
            }
        }

        //GET: api/to-do's
        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll()
        {
            return _context.TodoItems.ToList();
        }

        //GET: api/to-do/id
        [HttpGet("{id}", Name = "GetTodo")]
        public ActionResult<TodoItem> GetById(long id)
        {
            var todoItem = _context.TodoItems.Find(id);

            return todoItem == null ? (ActionResult<TodoItem>)NotFound() : (ActionResult<TodoItem>)todoItem;
        }

        //POST: api/add
        [HttpPost]
        public ActionResult Create(TodoItem item)
        {
            _context.TodoItems.Add(item);
            _context.SaveChanges();
            return CreatedAtRoute("CreateTodo", new { id = item.Id }, item);
        }

        //PUT: api/to-to/id
        [HttpPut("{id}")]
        public ActionResult UpdateTodo(long id, TodoItem item) 
        {
            var findTodoToUpdate = _context.TodoItems.Find(id);
            if(findTodoToUpdate == null) {
                return NotFound();
            }

            findTodoToUpdate.IsComplete = item.IsComplete;
            findTodoToUpdate.Name = item.Name;
            _context.TodoItems.Update(findTodoToUpdate);
            _context.SaveChanges();
            return NoContent();
        }

        //PATCH: api/to-do/id
        [HttpPatch("{id}")]
        public ActionResult PatchTodo(long id, TodoItem item)
        {
            var findTodoToUpdate = _context.TodoItems.Find(id);
            if (findTodoToUpdate == null)
            {
                return NotFound();
            }

            findTodoToUpdate.IsComplete = item.IsComplete;
            findTodoToUpdate.Name = item.Name;
            _context.TodoItems.Update(findTodoToUpdate);
            _context.SaveChanges();
            return NoContent();
        }

        //DELETE: api/to-do/id
        [HttpDelete("{id}")]
        public ActionResult DeleteTodo(long id) 
        {
            var todoToDelete = _context.TodoItems.Find(id);
            if(todoToDelete == null) {
                return NotFound();
            }
            _context.TodoItems.Remove(todoToDelete);
            _context.SaveChanges();
            return NoContent();
        }
    }
}