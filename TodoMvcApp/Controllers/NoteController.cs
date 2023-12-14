using Backend_Task01_API.Data;
using Backend_Task01_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_Task01_API.Controllers
{

    [ApiController]
    [Route("")]
    public class NoteController : ControllerBase
    {

        private readonly NotesContext dbContext;

        public NoteController(NotesContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet("notes")]
        public List<Note> GetNotes(bool? completed = null)
        {
            var notes = dbContext.Notes;

            if (completed == true)
            {
                var active = notes.Where(x => x.IsDone == true);
                return active.ToList();
            }
            else if (completed == false)
            {
                var notActive = notes.Where(x => x.IsDone == false);
                return notActive.ToList();
            }

            return notes.ToList();
        }

        [HttpGet("remaining")]
        public int RemaningTasks()
        {
            var notes = dbContext.Notes.Count(x => !x.IsDone);
            return notes;
        }

        [HttpPost("notes")]
        public ActionResult<Note> CreateNote(Note note)
        {
            dbContext.Notes.Add(note);
            dbContext.SaveChanges();

            return note;
        }

        [HttpPut("notes/{id}")]
        public ActionResult UpdateNote(int id, Note note)
        {
            if (id != note.Id)
            {
                return BadRequest();
            }

            dbContext.Entry(note).State = EntityState.Modified;
            dbContext.SaveChanges();

            if (!NoteExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("notes/{id}")]
        public ActionResult DeleteNoteById(int id)
        {
            var note = dbContext.Notes.Find(id);

            if (note == null)
            {
                return NotFound();
            }

            dbContext.Notes.Remove(note);
            dbContext.SaveChanges();

            return NoContent();
        }

        [HttpPost("toggle-all")]
        public void ToggleAll()
        {
            bool isDone = dbContext.Notes.All(x => x.IsDone);

            if(isDone == false)
            {
                foreach (var note in dbContext.Notes)
                {
                    note.IsDone = true;
                }
            }
            else
            {
                foreach(var note in dbContext.Notes)
                {
                    note.IsDone = false;
                }
            }
            dbContext.SaveChanges();
        }

        [HttpPost("clear-completed")]

        public void ClearAllCompleted()
        {
            var note = dbContext.Notes.Where(x => x.IsDone == true);
            
            foreach(var n in note)
            {
                dbContext.Notes.Remove(n);
                dbContext.SaveChanges();

            }
        }


        private bool NoteExists(int id)
        {
            return dbContext.Notes.Any(e => e.Id == id);
        }
    }

}