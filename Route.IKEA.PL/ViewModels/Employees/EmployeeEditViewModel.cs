namespace Route.IKEA.PL.ViewModels.Employees
{
    public class EmployeeEditViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public int? Age { get; set; }

        public string? Address { get; set; }

        public decimal Salary { get; set; }

        public bool IsActive { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string Gender { get; set; } = null!;

        public string EmployeeType { get; set; } = null!;

        public DateTime HiringDate { get; set; }

        public DateTime CreationDate { get; set; } = DateTime.Now;
    }
}

