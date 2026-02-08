namespace ResturangtApi_samiharun_net24.Models.Dtos
{
    // Admins användarnamn som skickas från frontend

    public class LogInRequest
    {
        
            public string Username { get; set; } = null!;
        // Admins användarnamn som skickas från frontend

        public string Password { get; set; } = null!;
        // Admins lösenord som skickas från frontend

    }
}

