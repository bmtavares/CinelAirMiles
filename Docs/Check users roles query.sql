use CinelAirMiles;

select
	AspNetUsers.Email as 'User',
	iif(AspNetRoles.Name is not null, AspNetRoles.Name, 'No role (client)') as 'Role',
	iif(MilesProgramNumber is not null, MilesProgramNumber, 'Not a client') as 'Program number'
	from AspNetUsers
	left join AspNetUserRoles on Id = UserId
	left join AspNetRoles on AspNetRoles.Id = RoleId
	left join Clients on Clients.UserId = AspNetUsers.Id
	order by 'Role';


select
	Clients.Id as 'ID',
	Email as 'E-mail',
	ProgramTiers.Description as 'Description',
	AspNetUsers.PhoneNumber as 'Phone',
	AspNetUsers.FirstName as 'First name',
	AspNetUsers.LastName as 'Last name',
	Clients.FlownSegments as 'Flown segments'
	from AspNetUsers
	right join Clients on Clients.UserId = AspNetUsers.Id
	left join ProgramTiers on ProgramTiers.Id = Clients.ProgramTierId
	order by 'ID'