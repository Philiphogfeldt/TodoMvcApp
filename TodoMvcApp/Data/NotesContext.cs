using Backend_Task01_API.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics.Metrics;
using System;

namespace Backend_Task01_API.Data
{
    public class NotesContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }

        public NotesContext(DbContextOptions<NotesContext> options)
            : base(options)
        {
        }


    }
}
