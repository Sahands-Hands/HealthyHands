# HealthyHands
Version: 20230321.1

## How to setup and run:
 - Install dependencies
	 - Project
		 - net6.0
	 - Client
		 - Microsoft.AspNetCore.Components.WebAssembly *Version 6.0.0*
		 - Microsoft.AspNetCore.Components.WebAssembly.DevServer *Version 6.0.0*
		 - Microsoft.AspNetCore.Components.WebAssembly.Authentication *Version 6.0.0*
		 - Microsoft.Extensions.Http *Version 6.0.0*
	 - Server
		 - Microsoft.AspNetCore.Components.WebAssembly.Server *Version 6.0.0*
		 - Microsoft.VisualStudio.Web.CodeGeneration.Design *Version 6.0.12*
		 - Swashbuckle.AspNetCore *Version 6.3.0*
		 - Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore *Version 6.0.0*
		 - Microsoft.AspNetCore.Identity.EntityFrameworkCore *Version 6.0.0*
		 - Microsoft.AspNetCore.Identity.UI *Version 6.0.0*
		 - Microsoft.AspNetCore.ApiAuthorization.IdentityServer *Version 6.0.0*
		 - Microsoft.EntityFrameworkCore.SqlServer *Version 6.0.14*
		 - Microsoft.EntityFrameworkCore.Tools *Version 6.0.14*
	 - Shared
		 - None
 - Setup Database
		 **Note**: If There is a migrations folder in Server, delete it before running initial migration
	 - Check which database you want to use in *appsettings.json*
	 - Run `Add-Migration initial`
	 - Run `Update-Database`
 - Run App
	 - Run Server App
	 - Upon first run, the program will add the following to the database:
		 - User roles (Admin, User)
		 - Default admin user
			 - Email: admin@example.com
			 - Password: "P@55w0rd"
		- Test user with *FirstName, LastName, Height, Gender, Activity Level and Birthdate*
			- Email: test@example.com
			- Password: "P@55w0rd"