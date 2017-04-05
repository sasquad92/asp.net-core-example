using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly ITodoRepository _todoRepository;

        public TodoController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        // GET api/todo
        [HttpGet]
        public IEnumerable<TodoItem> GetAll()
        {
            return _todoRepository.GetAll();
        }

        // GET api/todo/{id}
        [HttpGet("{id}", Name = "GetTodo")]
        public IActionResult GetById(long id)
        {
            var item = _todoRepository.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            return new ObjectResult(item);
        }

        // POST api/todo
        [HttpPost]
        public IActionResult Create([FromBody] TodoItem item)
        {
            if (item == null)
            {
                return BadRequest();
            }

            _todoRepository.Add(item);

            return CreatedAtRoute("GetTodo", new { id = item.Key }, item);
        }

        // PUT api/todo/{id}
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] TodoItem item)
        {
            if (item == null || id != item.Key)
            {
                return BadRequest();
            }

            var todo = _todoRepository.Find(id);

            if (todo == null)
            {
                return NotFound();
            }

            todo.IsComplete = item.IsComplete;
            todo.Name = item.Name;

            _todoRepository.Update(todo);
            return new NoContentResult();
        }

        // DELETE api/todo/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var todo = _todoRepository.Find(id);
            if (todo == null)
            {
                return NotFound();
            }

            _todoRepository.Remove(id);
            return new NoContentResult();
        }

    }
}