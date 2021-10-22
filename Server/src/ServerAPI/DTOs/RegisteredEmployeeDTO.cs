namespace ServerAPI.DTOs
{
	public class RegisteredEmployeeDto
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string ConfirmedPassword { get; set; }
		//Domyslnie Employee
		public int RoleId { get; set; } = 2;
	}
}