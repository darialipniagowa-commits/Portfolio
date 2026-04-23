namespace Portfolio.Services
{
    public class AdminState
    {
        public bool IsAdmin { get; private set; }

        public void Enable(string? key)
        {
            if (key == "edit-portfolio-2026")
                IsAdmin = true;
        }
    }
}