using System;
using System.ComponentModel.DataAnnotations;

namespace TodoList.Api.DTOs
{
    public class TodoItemDTO
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Description is required"), MaxLength(500)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; }
    }
}
